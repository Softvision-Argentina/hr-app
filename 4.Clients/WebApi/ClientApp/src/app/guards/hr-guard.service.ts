import { JwtHelper } from 'angular2-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from 'src/entities/user';
import { AppConfig } from '../app-config/app.config';

@Injectable()
export class HRGuard implements CanActivate {

  previousUrl: string;
  currentUrl: string;
  currentUser: User;
  roles: string[];

  constructor(private jwtHelper: JwtHelper, private router: Router, config: AppConfig) {
    this.roles = config.getConfig("roles");
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.currentUser = JSON.parse(localStorage.getItem("currentUser"));

    if (this.currentUser && !this.jwtHelper.isTokenExpired(this.currentUser.token)) {
      if (this.roles.indexOf(this.currentUser.role) !== -1
        && (this.currentUser.role === "Admin" || this.currentUser.role === "HRManagement" || this.currentUser.role === "HRUser")) return true;
      else {
        this.router.navigate(["unauthorized"], { queryParams: { returnUrl: state.url } });
        return false;
      }
    }
    this.router.navigate(["login"], { queryParams: { returnUrl: state.url } });
    return false;
  }
}