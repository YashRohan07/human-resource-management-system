export interface Payroll {
  id: number;
  employeeId: number;
  employeeName?: string;
  month: number;
  year: number;
  basicSalarySnapshot: number;
  bonusSnapshot: number;
  deductionSnapshot: number;
  tax: number;
  grossSalary: number;
  netSalary: number;
  generatedAt: string;
  createdAt: string;
  updatedAt: string;
}

export interface PayrollGenerate {
  month: number;
  year: number;
}

export interface PayrollQuery {
  page?: number;
  pageSize?: number;
  employeeId?: number;
  month?: number;
  year?: number;
}

export interface PayrollSummary {
  department: string;
  totalEmployees: number;
  totalSalary: number;
}
