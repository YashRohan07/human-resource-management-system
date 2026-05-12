import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  // Show logged in user's name
  get fullName(): string {
    return this.authService.getCurrentUser()?.fullName ?? 'User';
  }

  // Clear login data and return to login page
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
