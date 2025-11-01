import { Component, signal, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DepartmentService } from '../services/department';
import { ValidationService } from '../services/validation';
import { Department } from '../models/department';

@Component({
  selector: 'app-create-user',
  imports: [ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './create-user.html',
  styleUrl: './create-user.scss'
})
export class CreateUserComponent implements OnInit {
  private departmentService = inject(DepartmentService);
  private validationService = inject(ValidationService);
  private router = inject(Router);

  // Signals for component state
  isSubmitting = signal(false);
  isLoadingDepartments = signal(true);
  departments = signal<Department[]>([]);

  // Reactive form with validation
  createUserForm = new FormGroup({
    userId: new FormControl('', {
      validators: [Validators.required, Validators.minLength(3), Validators.maxLength(20)],
      asyncValidators: [this.validationService.userIdValidator()]
    }),
    email: new FormControl('', {
      validators: [Validators.required],
      asyncValidators: [this.validationService.emailValidator()]
    }),
    firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    password: new FormControl('', [
      Validators.required,
      ValidationService.passwordStrengthValidator
    ]),
    confirmPassword: new FormControl('', [Validators.required]),
    departmentId: new FormControl('', [Validators.required])
  });

  ngOnInit() {
    this.loadDepartments();
    this.setupPasswordConfirmation();
  }

  private loadDepartments() {
    this.departmentService.getDepartments().subscribe({
      next: (departments) => {
        this.departments.set(departments);
        this.isLoadingDepartments.set(false);
      },
      error: (error) => {
        console.error('Error loading departments:', error);
        this.isLoadingDepartments.set(false);
      }
    });
  }

  private setupPasswordConfirmation() {
    // Add custom validator for password confirmation
    const passwordControl = this.createUserForm.get('password')!;
    const confirmPasswordControl = this.createUserForm.get('confirmPassword')!;

    confirmPasswordControl.addValidators((control) => {
      if (!control.value) return null;
      return control.value === passwordControl.value ? null : { passwordMismatch: true };
    });

    // Re-validate confirm password when password changes
    passwordControl.valueChanges.subscribe(() => {
      if (confirmPasswordControl.value) {
        confirmPasswordControl.updateValueAndValidity();
      }
    });
  }

  onSubmit() {
    if (this.createUserForm.valid) {
      this.isSubmitting.set(true);
      
      // Simulate API call
      console.log('Creating user:', this.createUserForm.value);
      
      // Simulate processing time
      setTimeout(() => {
        this.isSubmitting.set(false);
        // Navigate to success page or login
        this.router.navigate(['/login']);
      }, 2000);
    } else {
      // Mark all fields as touched to show validation errors
      this.createUserForm.markAllAsTouched();
    }
  }

  onCancel() {
    this.router.navigate(['/login']);
  }

  // Helper methods for template
  get userId() { return this.createUserForm.get('userId')!; }
  get email() { return this.createUserForm.get('email')!; }
  get firstName() { return this.createUserForm.get('firstName')!; }
  get lastName() { return this.createUserForm.get('lastName')!; }
  get password() { return this.createUserForm.get('password')!; }
  get confirmPassword() { return this.createUserForm.get('confirmPassword')!; }
  get departmentId() { return this.createUserForm.get('departmentId')!; }
}
