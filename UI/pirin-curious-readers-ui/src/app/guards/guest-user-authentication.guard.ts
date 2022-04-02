import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class GuestUserAuthenticationGuard implements CanActivate {
    constructor(
      private router: Router, 
      private authService: AuthService
    ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    if (!this.authService.userIsLoggedIn())
      return true;
    else {
      this.router.navigate(['/']);
      return false;
    }
  }
}