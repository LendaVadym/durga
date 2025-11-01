import { Routes } from '@angular/router';
import { LoginComponent } from './login/login';
import { CreateUserComponent } from './create-user/create-user';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'create-user', component: CreateUserComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
