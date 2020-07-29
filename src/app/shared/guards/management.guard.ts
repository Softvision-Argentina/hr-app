import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanLoad, Route } from '@angular/router';
import { User } from '@shared/models/user.model';
import { AppConfig } from '@shared/utils/app.config';

@Injectable({ providedIn: 'root' })
export class ManagementGuard implements CanActivate, CanLoad {

  previousUrl: string;
  currentUrl: string;
  currentUser: User;
  roles: string[];

  private _allowedRoles = [
    'Admin',
    'HRManagement',
    'HRUser',
    'Recruiter'
  ];

  constructor(private jwtHelperService: JwtHelperService, private router: Router,  config: AppConfig) {
    this.roles = config.getConfig('roles');
  }

  canLoad(route: Route): boolean {
    return this.isUserAllowed(route.path);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.isUserAllowed(state.url);
  }

  isUserAllowed(returnUrl: string): boolean {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));

    if (!this.currentUser || this.jwtHelperService.isTokenExpired(this.currentUser.token)) {
      this.router.navigate(['login'], { queryParams: { returnUrl } });
      return false;
    }

    if (this.hasRole(this.roles, this.currentUser.role)
      && this.hasRole(this._allowedRoles, this.currentUser.role)) {
      return true;
    }

    this.router.navigate(['unauthorized'], { queryParams: { returnUrl } });
    return false;
  }

  hasRole(roles: string[], userRole: string): boolean {
    return roles.some(r => r === userRole);
  }
}
