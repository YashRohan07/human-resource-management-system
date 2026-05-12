import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { PayrollService } from '../../../core/services/payroll.service';
import { Payroll, PayrollQuery } from '../../../shared/interfaces/payroll.interface';

@Component({
  selector: 'app-payroll-list',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './payroll-list.component.html',
  styleUrl: './payroll-list.component.scss'
})
export class PayrollListComponent implements OnInit {

  payrolls: Payroll[] = [];

  page = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 1;

  employeeId = '';
  month = '';
  year = '';

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private payrollService: PayrollService
  ) { }

  ngOnInit(): void {
    this.employeeId = this.route.snapshot.queryParamMap.get('employeeId') || '';
    this.month = this.route.snapshot.queryParamMap.get('month') || '';
    this.year = this.route.snapshot.queryParamMap.get('year') || '';

    this.loadPayrolls();
  }

  loadPayrolls(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const query: PayrollQuery = {
      page: this.page,
      pageSize: this.pageSize,
      employeeId: this.employeeId ? Number(this.employeeId) : undefined,
      month: this.month ? Number(this.month) : undefined,
      year: this.year ? Number(this.year) : undefined
    };

    this.payrollService.getAll(query).subscribe({
      next: result => {
        this.payrolls = result.items || [];
        this.totalCount = result.meta?.totalCount || 0;
        this.totalPages = result.meta?.totalPages || 1;
        this.page = result.meta?.currentPage || this.page;
        this.pageSize = result.meta?.pageSize || this.pageSize;

        this.isLoading = false;
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to load payrolls.';
        this.isLoading = false;
      }
    });
  }

  applyFilters(): void {
    this.page = 1;

    this.router.navigate(['/payrolls'], {
      queryParams: {
        employeeId: this.employeeId || null,
        month: this.month || null,
        year: this.year || null
      }
    });

    this.loadPayrolls();
  }

  clearFilters(): void {
    this.employeeId = '';
    this.month = '';
    this.year = '';
    this.page = 1;

    this.router.navigate(['/payrolls']);
    this.loadPayrolls();
  }

  goToPreviousPage(): void {
    if (this.page <= 1) {
      return;
    }

    this.page--;
    this.loadPayrolls();
  }

  goToNextPage(): void {
    if (this.page >= this.totalPages) {
      return;
    }

    this.page++;
    this.loadPayrolls();
  }

  onPageSizeChange(): void {
    this.page = 1;
    this.loadPayrolls();
  }

  getMonthName(month: number): string {
    return new Date(2000, month - 1, 1).toLocaleString('en-US', {
      month: 'long'
    });
  }
}
