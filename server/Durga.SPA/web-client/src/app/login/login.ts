import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  // Signal for form submission state
  isSubmitting = signal(false);
  
  // Reactive form with signal-based validation tracking
  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(3)]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)])
  });

  constructor(private router: Router) {}

  onSubmit() {
    if (this.loginForm.valid) {
      this.isSubmitting.set(true);
      console.log('Login form submitted:', this.loginForm.value);
      // Add your authentication logic here
      // Simulate API call
      setTimeout(() => this.isSubmitting.set(false), 1000);
    } else {
      // Mark all fields as touched to show validation errors
      this.loginForm.markAllAsTouched();
    }
  }

  // Helper methods for template - using getters for reactive access
  get username() { return this.loginForm.get('username')!; }
  get password() { return this.loginForm.get('password')!; }
}
