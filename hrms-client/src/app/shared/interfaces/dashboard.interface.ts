import { PayrollSummary } from './payroll.interface';

export interface DepartmentSummary {
  department: string;
  totalEmployees: number;
}

export interface DashboardSummary {
  totalEmployees: number;
  activeEmployees: number;
  currentMonthPayrollCost: number;
  departmentWiseEmployeeCount: DepartmentSummary[];
  departmentWisePayrollSummary: PayrollSummary[];
}
