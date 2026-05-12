import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../services/auth.service';

// Handles common API errors in one place
export const errorInterceptor: HttpInterceptorFn = (request, next) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {

      const isLoginRequest = request.url.includes('/auth/login');

      // Expired or missing token
      if (error.status === 401 && !isLoginRequest) {
        authService.logout();
        router.navigate(['/login']);
      }

      return throwError(() => error);
    })
  );
};
