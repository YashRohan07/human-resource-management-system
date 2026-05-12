// Login request sent to backend
export interface LoginRequest {
  email: string;
  password: string;
}

// Login response received from backend
export interface LoginResponse {
  token: string;
  email: string;
  fullName: string;
  role: string;
  expiresAt: string;
}

// User data stored in browser after login
export interface AuthUser {
  token: string;
  email: string;
  fullName: string;
  role: string;
  expiresAt: string;
}
