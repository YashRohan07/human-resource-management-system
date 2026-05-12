import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { EmployeeService } from '../../../core/services/employee.service';
import { SalaryService } from '../../../core/services/salary.service';
import { Employee } from '../../../shared/interfaces/employee.interface';
import { Salary } from '../../../shared/interfaces/salary.interface';

@Component({
  selector: 'app-salary-details',
  imports: [CommonModule, RouterLink],
  templateUrl: './salary-details.component.html',
  styleUrl: './salary-details.component.scss'
})
export class SalaryDetailsComponent implements OnInit {

  employee: Employee | null = null;
  salary: Salary | null = null;

  employeeId = 0;
  isLoading = false;
  errorMessage = '';
  salaryMessage = '';

  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private salaryService: SalaryService
  ) { }

  ngOnInit(): void {
    this.employeeId = Number(this.route.snapshot.paramMap.get('id'));

    this.loadEmployee();
    this.loadSalary();
  }

  loadEmployee(): void {
    this.isLoading = true;

    this.employeeService.getById(this.employeeId).subscribe({
      next: employee => {
        this.employee = employee;
        this.isLoading = false;
      },
      error: error => {
        this.errorMessage =
          error?.error?.message || 'Failed to load employee.';
        this.isLoading = false;
      }
    });
  }

  // A missing salary is normal for a newly added employee.
  loadSalary(): void {
    this.salaryService.getByEmployeeId(this.employeeId).subscribe({
      next: salary => {
        this.salary = salary;
      },
      error: () => {
        this.salaryMessage = 'No salary assigned yet.';
      }
    });
  }
}
