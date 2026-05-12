import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { EmployeeService } from '../../../core/services/employee.service';
import {
  EmployeeCreate,
  EmployeeUpdate
} from '../../../shared/interfaces/employee.interface';

@Component({
  selector: 'app-employee-form',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './employee-form.component.html',
  styleUrl: './employee-form.component.scss'
})
export class EmployeeFormComponent implements OnInit {

  employeeId: number | null = null;

  isEditMode = false;
  isLoading = false;
  isSaving = false;

  errorMessage = '';

  // Employee create/edit form
  employeeForm;

  constructor(
    private formBuilder: FormBuilder,
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private router: Router
  ) {

    this.employeeForm = this.formBuilder.group({
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]],
      department: ['', [Validators.required]],
      position: ['', [Validators.required]],
      accountNumber: ['', [Validators.required]],
      employmentStatus: ['Active', [Validators.required]]
    });
  }

  ngOnInit(): void {

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.employeeId = Number(id);
      this.isEditMode = true;
      this.loadEmployee(this.employeeId);
    }
  }

  // Load employee data for edit mode
  loadEmployee(id: number): void {

    this.isLoading = true;

    this.employeeService.getById(id).subscribe({
      next: employee => {
        this.employeeForm.patchValue({
          fullName: employee.fullName,
          email: employee.email,
          phone: employee.phone,
          department: employee.department,
          position: employee.position,
          accountNumber: employee.accountNumber,
          employmentStatus: employee.employmentStatus
        });

        this.isLoading = false;
      },
      error: error => {
        this.errorMessage = this.getErrorMessage(
          error,
          'Failed to load employee.'
        );

        this.isLoading = false;
      }
    });
  }

  // Create or update employee
  onSubmit(): void {

    this.errorMessage = '';

    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    const request =
      this.employeeForm.getRawValue() as EmployeeCreate | EmployeeUpdate;

    const saveRequest = this.isEditMode && this.employeeId
      ? this.employeeService.update(this.employeeId, request)
      : this.employeeService.create(request);

    saveRequest.subscribe({
      next: employee => {
        this.router.navigate(['/employees', employee.id]);
      },
      error: error => {
        this.errorMessage = this.getErrorMessage(
          error,
          'Failed to save employee.'
        );

        this.isSaving = false;
      }
    });
  }

  hasError(controlName: string): boolean {

    const control = this.employeeForm.get(controlName);

    return !!control && control.invalid && control.touched;
  }

  // Read backend error message safely
  private getErrorMessage(error: any, fallbackMessage: string): string {

    // Backend validation/message response
    if (error?.error?.message) {
      return error.error.message;
    }

    // Backend validation errors array
    if (error?.error?.errors?.length > 0) {
      return error.error.errors[0];
    }

    // Duplicate email fallback for 409 conflict
    if (error?.status === 409) {
      return 'Employee email already exists.';
    }

    return fallbackMessage;
  }
}
