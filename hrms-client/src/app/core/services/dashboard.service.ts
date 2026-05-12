import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/interfaces/api-response.interface';
import { DashboardSummary } from '../../shared/interfaces/dashboard.interface';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private readonly apiUrl = `${environment.apiUrl}/dashboard`;

  constructor(private http: HttpClient) { }

  // Dashboard summary cards and department data
  getSummary(): Observable<DashboardSummary> {
    return this.http
      .get<ApiResponse<DashboardSummary>>(this.apiUrl)
      .pipe(map(response => response.data!));
  }
}
