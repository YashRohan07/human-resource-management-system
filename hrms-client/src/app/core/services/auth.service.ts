import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';

import { environment } from '../../../environments/environment';

import { ApiResponse } from '../../shared/interfaces/api-response.interface';

import {
  AuthUser,
  LoginRequest,
  LoginResponse
} from '../../shared/interfaces/auth.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // Local storage key for auth data
  private readonly authStorageKey = 'hrms_auth_user';

  constructor(private http: HttpClient) { }

  // Login user and save auth info locally
  login(request: LoginRequest): Observable<AuthUser> {

    return this.http
      .post<ApiResponse<LoginResponse>>(
        `${environment.apiUrl}/auth/login`,
        request
      )
      .pipe(

        // Extract actual user data from API response
        map(response => {

          if (!response.data) {
            throw new Error(response.message || 'Login failed.');
          }

          return response.data;
        }),

        // Save logged in user after successful login
        tap(user => this.saveUser(user))
      );
  }

  // Remove user data during logout
  logout(): void {
    localStorage.removeItem(this.authStorageKey);
  }

  // Get logged in user from local storage
  getCurrentUser(): AuthUser | null {

    const storedUser = localStorage.getItem(this.authStorageKey);

    if (!storedUser) {
      return null;
    }

    try {
      return JSON.parse(storedUser) as AuthUser;
    }
    catch {

      // Clear broken auth data if parsing fails
      this.logout();

      return null;
    }
  }

  // Get JWT token
  getToken(): string | null {
    return this.getCurrentUser()?.token ?? null;
  }

  // Get current user role
  getUserRole(): string | null {
    return this.getCurrentUser()?.role ?? null;
  }

  // Check login status using token expiry
  isLoggedIn(): boolean {

    const user = this.getCurrentUser();

    if (!user) {
      return false;
    }

    return new Date(user.expiresAt) > new Date();
  }

  // Save auth user in local storage
  private saveUser(user: AuthUser): void {

    localStorage.setItem(
      this.authStorageKey,
      JSON.stringify(user)
    );
  }
}
