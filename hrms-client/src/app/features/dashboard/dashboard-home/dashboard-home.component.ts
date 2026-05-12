import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

import { DashboardService } from '../../../core/services/dashboard.service';
import { DashboardSummary } from '../../../shared/interfaces/dashboard.interface';

@Component({
  selector: 'app-dashboard-home',
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html',
  styleUrl: './dashboard-home.component.scss'
})
export class DashboardHomeComponent implements OnInit {

  summary: DashboardSummary | null = null;

  isLoading = false;
  errorMessage = '';

  constructor(private dashboardService: DashboardService) { }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.dashboardService.getSummary().subscribe({
      next: summary => {
        this.summary = summary;
        this.isLoading = false;
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to load dashboard summary.';
        this.isLoading = false;
      }
    });
  }
}
