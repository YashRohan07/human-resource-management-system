import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/interfaces/api-response.interface';
import { PagedResult } from '../../shared/interfaces/paged-result.interface';
import {
  Employee,
  EmployeeCreate,
  EmployeeQuery,
  EmployeeUpdate
} from '../../shared/interfaces/employee.interface';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private readonly apiUrl = `${environment.apiUrl}/employees`;

  constructor(private http: HttpClient) { }

  // Get employees with search, filter, sort and pagination
  getAll(query: EmployeeQuery): Observable<PagedResult<Employee>> {

    let params = new HttpParams();

    Object.entries(query).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, value.toString());
      }
    });

    return this.http
      .get<ApiResponse<PagedResult<Employee>>>(this.apiUrl, { params })
      .pipe(map(response => response.data!));
  }

  getById(id: number): Observable<Employee> {
    return this.http
      .get<ApiResponse<Employee>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data!));
  }

  create(request: EmployeeCreate): Observable<Employee> {
    return this.http
      .post<ApiResponse<Employee>>(this.apiUrl, request)
      .pipe(map(response => response.data!));
  }

  update(id: number, request: EmployeeUpdate): Observable<Employee> {
    return this.http
      .put<ApiResponse<Employee>>(`${this.apiUrl}/${id}`, request)
      .pipe(map(response => response.data!));
  }

  delete(id: number): Observable<ApiResponse<object>> {
    return this.http.delete<ApiResponse<object>>(`${this.apiUrl}/${id}`);
  }
}
