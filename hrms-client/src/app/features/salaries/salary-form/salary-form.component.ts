import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { EmployeeService } from '../../../core/services/employee.service';
import { SalaryService } from '../../../core/services/salary.service';
import { Employee } from '../../../shared/interfaces/employee.interface';
import { Salary } from '../../../shared/interfaces/salary.interface';

@Component({
  selector: 'app-salary-form',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './salary-form.component.html',
  styleUrl: './salary-form.component.scss'
})
export class SalaryFormComponent implements OnInit {

  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private employeeService = inject(EmployeeService);
  private salaryService = inject(SalaryService);

  employee: Employee | null = null;
  salary: Salary | null = null;

  employeeId = 0;
  isLoading = false;
  isSaving = false;
  errorMessage = '';

  form = this.fb.group({
    basicSalary: [0, [Validators.required, Validators.min(0.01)]],
    bonus: [0, [Validators.required, Validators.min(0)]],
    deduction: [0, [Validators.required, Validators.min(0)]],
    effectiveFrom: ['', Validators.required]
  });

  ngOnInit(): void {
    this.employeeId = Number(this.route.snapshot.paramMap.get('id'));

    this.loadEmployee();
    this.loadSalary();
  }

  get isEditMode(): boolean {
    return this.salary !== null;
  }

  loadEmployee(): void {
    this.employeeService.getById(this.employeeId).subscribe({
      next: employee => {
        this.employee = employee;
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to load employee.';
      }
    });
  }

  // If salary exists, this form becomes update mode. Otherwise it stays create mode.
  loadSalary(): void {
    this.isLoading = true;

    this.salaryService.getByEmployeeId(this.employeeId).subscribe({
      next: salary => {
        this.salary = salary;

        this.form.patchValue({
          basicSalary: salary.basicSalary,
          bonus: salary.bonus,
          deduction: salary.deduction,
          effectiveFrom: this.toDateInputValue(salary.effectiveFrom)
        });

        this.isLoading = false;
      },
      error: () => {
        this.form.patchValue({
          effectiveFrom: this.toDateInputValue(new Date().toISOString())
        });

        this.isLoading = false;
      }
    });
  }

  save(): void {
    this.errorMessage = '';

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.getRawValue();

    const request = {
      basicSalary: Number(formValue.basicSalary),
      bonus: Number(formValue.bonus),
      deduction: Number(formValue.deduction),
      effectiveFrom: formValue.effectiveFrom || ''
    };

    this.isSaving = true;

    if (this.salary) {
      this.salaryService.update(this.salary.id, request).subscribe({
        next: () => {
          this.router.navigate(['/employees', this.employeeId, 'salary']);
        },
        error: error => {
          this.errorMessage =
            error?.error?.message || 'Failed to update salary.';
          this.isSaving = false;
        }
      });

      return;
    }

    this.salaryService.create({
      employeeId: this.employeeId,
      ...request
    }).subscribe({
      next: () => {
        this.router.navigate(['/employees', this.employeeId, 'salary']);
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to create salary.';
        this.isSaving = false;
      }
    });
  }

  private toDateInputValue(value: string): string {
    return value.split('T')[0];
  }
}
