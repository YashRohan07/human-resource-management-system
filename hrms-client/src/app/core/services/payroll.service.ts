import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/interfaces/api-response.interface';
import { PagedResult } from '../../shared/interfaces/paged-result.interface';
import {
  Payroll,
  PayrollGenerate,
  PayrollQuery,
  PayrollSummary
} from '../../shared/interfaces/payroll.interface';

@Injectable({
  providedIn: 'root'
})
export class PayrollService {

  private readonly apiUrl = `${environment.apiUrl}/payrolls`;

  constructor(private http: HttpClient) { }

  generate(request: PayrollGenerate): Observable<ApiResponse<Payroll[]>> {
    return this.http.post<ApiResponse<Payroll[]>>(
      `${this.apiUrl}/generate`,
      request
    );
  }

  getAll(query: PayrollQuery): Observable<PagedResult<Payroll>> {

    let params = new HttpParams();

    Object.entries(query).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, value.toString());
      }
    });

    return this.http
      .get<ApiResponse<PagedResult<Payroll>>>(this.apiUrl, { params })
      .pipe(map(response => response.data!));
  }

  getById(id: number): Observable<Payroll> {
    return this.http
      .get<ApiResponse<Payroll>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data!));
  }

  getByEmployeeId(employeeId: number): Observable<Payroll[]> {
    return this.http
      .get<ApiResponse<Payroll[]>>(`${this.apiUrl}/employee/${employeeId}`)
      .pipe(map(response => response.data!));
  }

  getSummary(month: number, year: number): Observable<PayrollSummary[]> {

    const params = new HttpParams()
      .set('month', month)
      .set('year', year);

    return this.http
      .get<ApiResponse<PayrollSummary[]>>(
        `${this.apiUrl}/summary`,
        { params }
      )
      .pipe(map(response => response.data!));
  }
}
