import { JwtHelper } from 'angular2-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanLoad, Route } from '@angular/router';
import { User } from 'src/entities/user';
import { AppConfig } from '../app-config/app.config';

@Injectable()
export class HRGuard implements CanActivate, CanLoad {

  previousUrl: string;
  currentUrl: string;
  currentUser: User;
  roles: string[];

  private allowedRoles = [
    'Admin',
    'HRManagement',
    'HRUser',
    'Interviewer',
    'CommunityManager',
    'Recruiter'
  ];

  constructor(private jwtHelper: JwtHelper, private router: Router,  config: AppConfig) {
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

    if (!this.currentUser || !this.jwtHelper.isTokenExpired(this.currentUser.token)) {
      this.router.navigate(['login'], { queryParams: { returnUrl } });
      return false;
    }

    if (this.hasRole(this.roles, this.currentUser.role)
      && this.hasRole(this.allowedRoles, this.currentUser.role)) {
      return true;
    }

    this.router.navigate(['unauthorized'], { queryParams: { returnUrl } });
    return false;
  }

  hasRole(roles: string[], userRole: string): boolean {
    return roles.some(r => r === userRole);
  }
}
