import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { EmployeeService } from '../../../core/services/employee.service';
import { SalaryService } from '../../../core/services/salary.service';
import { Employee } from '../../../shared/interfaces/employee.interface';
import { Salary } from '../../../shared/interfaces/salary.interface';

@Component({
  selector: 'app-employee-details',
  imports: [CommonModule, RouterLink],
  templateUrl: './employee-details.component.html',
  styleUrl: './employee-details.component.scss'
})
export class EmployeeDetailsComponent implements OnInit {

  employee: Employee | null = null;
  salary: Salary | null = null;

  isLoading = false;
  errorMessage = '';
  salaryMessage = '';

  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private salaryService: SalaryService
  ) { }

  ngOnInit(): void {

    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.loadEmployee(id);
    this.loadSalary(id);
  }

  // Load employee information
  loadEmployee(id: number): void {

    this.isLoading = true;

    this.employeeService.getById(id).subscribe({
      next: employee => {
        this.employee = employee;
        this.isLoading = false;
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to load employee details.';
        this.isLoading = false;
      }
    });
  }

  // Salary may not exist for every employee yet
  loadSalary(employeeId: number): void {

    this.salaryService.getByEmployeeId(employeeId).subscribe({
      next: salary => {
        this.salary = salary;
      },
      error: () => {
        this.salaryMessage = 'No salary assigned yet.';
      }
    });
  }
}
