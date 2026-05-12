export interface Employee {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  department: string;
  position: string;
  accountNumber: string;
  employmentStatus: string;
  createdAt: string;
  updatedAt: string;
}

export interface EmployeeCreate {
  fullName: string;
  email: string;
  phone: string;
  department: string;
  position: string;
  accountNumber: string;
  employmentStatus: string;
}

export interface EmployeeUpdate {
  fullName: string;
  email: string;
  phone: string;
  department: string;
  position: string;
  accountNumber: string;
  employmentStatus: string;
}

export interface EmployeeQuery {
  page?: number;
  pageSize?: number;
  search?: string;
  department?: string;
  status?: string;
  sortBy?: string;
  sortOrder?: string;
}
