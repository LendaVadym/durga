import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay } from 'rxjs';
import { Department } from '../models/department';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private http = inject(HttpClient);

  // Mock API endpoint - replace with actual API URL
  private readonly API_URL = '/api/departments';

  getDepartments(): Observable<Department[]> {
    // For now, return mock data. In production, replace with actual HTTP call:
    // return this.http.get<Department[]>(this.API_URL);
    
    const mockDepartments: Department[] = [
      { id: '1', name: 'Engineering', description: 'Software Development' },
      { id: '2', name: 'Human Resources', description: 'HR Management' },
      { id: '3', name: 'Marketing', description: 'Marketing and Sales' },
      { id: '4', name: 'Finance', description: 'Financial Operations' },
      { id: '5', name: 'Operations', description: 'Business Operations' }
    ];
    
    // Simulate network delay
    return of(mockDepartments).pipe(delay(500));
  }
}
