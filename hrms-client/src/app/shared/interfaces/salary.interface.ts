export interface Salary {
  id: number;
  employeeId: number;
  basicSalary: number;
  bonus: number;
  deduction: number;
  effectiveFrom: string;
  createdAt: string;
  updatedAt: string;
}

export interface SalaryCreate {
  employeeId: number;
  basicSalary: number;
  bonus: number;
  deduction: number;
  effectiveFrom: string;
}

export interface SalaryUpdate {
  basicSalary: number;
  bonus: number;
  deduction: number;
  effectiveFrom: string;
}
