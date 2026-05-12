import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/interfaces/api-response.interface';
import {
  Salary,
  SalaryCreate,
  SalaryUpdate
} from '../../shared/interfaces/salary.interface';

@Injectable({
  providedIn: 'root'
})
export class SalaryService {

  private readonly apiUrl = `${environment.apiUrl}/salaries`;

  constructor(private http: HttpClient) { }

  // Salary is employee-specific
  getByEmployeeId(employeeId: number): Observable<Salary> {
    return this.http
      .get<ApiResponse<Salary>>(`${this.apiUrl}/employee/${employeeId}`)
      .pipe(map(response => response.data!));
  }

  create(request: SalaryCreate): Observable<Salary> {
    return this.http
      .post<ApiResponse<Salary>>(this.apiUrl, request)
      .pipe(map(response => response.data!));
  }

  update(id: number, request: SalaryUpdate): Observable<Salary> {
    return this.http
      .put<ApiResponse<Salary>>(`${this.apiUrl}/${id}`, request)
      .pipe(map(response => response.data!));
  }
}
