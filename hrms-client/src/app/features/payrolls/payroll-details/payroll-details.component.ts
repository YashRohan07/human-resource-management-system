import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { PayrollService } from '../../../core/services/payroll.service';
import { Payroll } from '../../../shared/interfaces/payroll.interface';

@Component({
  selector: 'app-payroll-details',
  imports: [CommonModule, RouterLink],
  templateUrl: './payroll-details.component.html',
  styleUrl: './payroll-details.component.scss'
})
export class PayrollDetailsComponent implements OnInit {

  payroll: Payroll | null = null;

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private payrollService: PayrollService
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadPayroll(id);
  }

  loadPayroll(id: number): void {
    this.isLoading = true;

    this.payrollService.getById(id).subscribe({
      next: payroll => {
        this.payroll = payroll;
        this.isLoading = false;
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to load payroll details.';
        this.isLoading = false;
      }
    });
  }

  getMonthName(month: number): string {
    return new Date(2000, month - 1, 1).toLocaleString('en-US', {
      month: 'long'
    });
  }
}
