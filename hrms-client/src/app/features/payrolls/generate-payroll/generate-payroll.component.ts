import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { PayrollService } from '../../../core/services/payroll.service';
import { Payroll } from '../../../shared/interfaces/payroll.interface';

@Component({
  selector: 'app-generate-payroll',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './generate-payroll.component.html',
  styleUrl: './generate-payroll.component.scss'
})
export class GeneratePayrollComponent {

  month = new Date().getMonth() + 1;
  year = new Date().getFullYear();

  generatedPayrolls: Payroll[] = [];

  isSaving = false;
  successMessage = '';
  errorMessage = '';

  constructor(private payrollService: PayrollService) { }

  generatePayroll(): void {
    this.successMessage = '';
    this.errorMessage = '';
    this.generatedPayrolls = [];

    if (this.isFutureMonth()) {
      this.errorMessage = 'Future month payroll cannot be generated.';
      return;
    }

    this.isSaving = true;

    this.payrollService.generate({
      month: Number(this.month),
      year: Number(this.year)
    }).subscribe({
      next: response => {
        this.generatedPayrolls = response.data || [];
        this.successMessage =
          response.message || 'Payroll generated successfully.';
        this.isSaving = false;
      },
      error: error => {
        if (error?.status === 409) {
          this.errorMessage =
            error?.error?.message ||
            'Payroll already exists for eligible employees.';
        } else {
          this.errorMessage =
            error?.error?.message || 'Payroll generation failed.';
        }

        this.isSaving = false;
      }
    });
  }

  isFutureMonth(): boolean {
    const now = new Date();
    const selectedDate = new Date(Number(this.year), Number(this.month) - 1, 1);
    const currentMonth = new Date(now.getFullYear(), now.getMonth(), 1);

    return selectedDate > currentMonth;
  }

  getMonthName(month: number): string {
    return new Date(2000, month - 1, 1).toLocaleString('en-US', {
      month: 'long'
    });
  }
}
