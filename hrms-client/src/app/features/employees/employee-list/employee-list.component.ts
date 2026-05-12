import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { EmployeeService } from '../../../core/services/employee.service';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Employee } from '../../../shared/interfaces/employee.interface';
import { PaginationMeta } from '../../../shared/interfaces/paged-result.interface';

@Component({
  selector: 'app-employee-list',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    ConfirmDialogComponent
  ],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.scss'
})
export class EmployeeListComponent implements OnInit {

  employees: Employee[] = [];

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  search = '';
  department = '';
  status = '';

  currentPage = 1;
  pageSize = 10;

  meta: PaginationMeta | null = null;

  showDeleteDialog = false;
  selectedEmployee: Employee | null = null;

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadEmployees();
  }

  get isAdmin(): boolean {
    return this.authService.getUserRole() === 'Admin';
  }

  // Load employees with filter and pagination
  loadEmployees(): void {

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.employeeService.getAll({
      page: this.currentPage,
      pageSize: this.pageSize,
      search: this.search,
      department: this.department,
      status: this.status,
      sortBy: 'fullName',
      sortOrder: 'asc'
    }).subscribe({
      next: result => {
        this.employees = result.items;
        this.meta = result.meta;
        this.isLoading = false;
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to load employees.';
        this.isLoading = false;
      }
    });
  }

  applyFilters(): void {
    this.currentPage = 1;
    this.loadEmployees();
  }

  clearFilters(): void {
    this.search = '';
    this.department = '';
    this.status = '';
    this.currentPage = 1;
    this.loadEmployees();
  }

  changePageSize(): void {
    this.currentPage = 1;
    this.loadEmployees();
  }

  nextPage(): void {

    if (!this.meta || this.currentPage >= this.meta.totalPages) {
      return;
    }

    this.currentPage++;
    this.loadEmployees();
  }

  previousPage(): void {

    if (this.currentPage <= 1) {
      return;
    }

    this.currentPage--;
    this.loadEmployees();
  }

  openDeleteDialog(employee: Employee): void {
    this.selectedEmployee = employee;
    this.showDeleteDialog = true;
  }

  closeDeleteDialog(): void {
    this.selectedEmployee = null;
    this.showDeleteDialog = false;
  }

  // Soft delete employee after confirmation
  confirmDelete(): void {

    if (!this.selectedEmployee) {
      return;
    }

    this.employeeService.delete(this.selectedEmployee.id).subscribe({
      next: response => {
        this.successMessage =
          response.message || 'Employee deleted successfully.';
        this.closeDeleteDialog();
        this.loadEmployees();
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to delete employee.';
        this.closeDeleteDialog();
      }
    });
  }
}
