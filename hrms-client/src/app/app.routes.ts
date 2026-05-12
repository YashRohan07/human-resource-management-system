import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';

import { LoginComponent } from './features/auth/login/login.component';
import { MainLayoutComponent } from './features/layout/main-layout/main-layout.component';
import { DashboardHomeComponent } from './features/dashboard/dashboard-home/dashboard-home.component';
import { EmployeeListComponent } from './features/employees/employee-list/employee-list.component';
import { EmployeeFormComponent } from './features/employees/employee-form/employee-form.component';
import { EmployeeDetailsComponent } from './features/employees/employee-details/employee-details.component';
import { SalaryDetailsComponent } from './features/salaries/salary-details/salary-details.component';
import { SalaryFormComponent } from './features/salaries/salary-form/salary-form.component';
import { NotFoundComponent } from './features/not-found/not-found.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        component: DashboardHomeComponent
      },
      {
        path: 'employees',
        component: EmployeeListComponent
      },
      {
        path: 'employees/new',
        component: EmployeeFormComponent
      },
      {
        path: 'employees/:id/salary',
        component: SalaryDetailsComponent
      },
      {
        path: 'employees/:id/salary/edit',
        component: SalaryFormComponent
      },
      {
        path: 'employees/:id',
        component: EmployeeDetailsComponent
      },
      {
        path: 'employees/:id/edit',
        component: EmployeeFormComponent
      }
    ]
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];
