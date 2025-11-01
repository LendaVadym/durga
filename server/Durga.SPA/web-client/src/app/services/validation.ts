import { Injectable, inject } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {
  private http = inject(HttpClient);

  // Mock API endpoints - replace with actual API URLs
  private readonly USER_CHECK_URL = '/api/users/check';  

  /**
   * Async validator to check if user ID is unique
   */
  userIdValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value) {
        return of(null);
      }

      // In production, replace with actual HTTP call:
      // return this.http.get<{exists: boolean}>(`${this.USER_CHECK_URL}/userid/${control.value}`)
      //   .pipe(
      //     map(result => result.exists ? { userIdTaken: true } : null)
      //   );

      // Mock validation - simulate checking against existing user IDs
      const existingUserIds = ['admin', 'user1', 'test', 'demo'];
      const exists = existingUserIds.includes(control.value.toLowerCase());

      return of(exists ? { userIdTaken: true } : null).pipe(delay(300));
    };
  }

  /**
   * Async validator to check if email is unique
   */
  emailValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value) {
        return of(null);
      }

      // Basic email format check first
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(control.value)) {
        return of({ email: true });
      }

      // In production, replace with actual HTTP call:
      // return this.http.get<{exists: boolean}>(`${this.USER_CHECK_URL}/email/${control.value}`)
      //   .pipe(
      //     map(result => result.exists ? { emailTaken: true } : null)
      //   );

      // Mock validation - simulate checking against existing emails
      const existingEmails = [
        'admin@company.com',
        'user@company.com',
        'test@company.com',
        'demo@company.com'
      ];
      const exists = existingEmails.includes(control.value.toLowerCase());

      return of(exists ? { emailTaken: true } : null).pipe(delay(400));
    };
  }

  /**
   * Custom password strength validator
   */
  static passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;

    if (!value) {
      return null;
    }

    const hasNumber = /[0-9]/.test(value);
    const hasUpper = /[A-Z]/.test(value);
    const hasLower = /[a-z]/.test(value);
    const hasSpecial = /[#?!@$%^&*-]/.test(value);
    const isLengthValid = value.length >= 8;

    const passwordValid = hasNumber && hasUpper && hasLower && hasSpecial && isLengthValid;

    if (!passwordValid) {
      return {
        passwordStrength: {
          hasNumber,
          hasUpper,
          hasLower,
          hasSpecial,
          isLengthValid
        }
      };
    }

    return null;
  }
}
