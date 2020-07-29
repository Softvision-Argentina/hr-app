import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '@shared/models/user.model';
import { AppConfig } from '@shared/utils/app.config';

@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanActivate {

  previousUrl: string;
  currentUrl: string;
  currentUser: User;
  roles: string[];

  constructor(private jwtHelperService: JwtHelperService, private router: Router, config: AppConfig) {
    this.roles = config.getConfig("roles");
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.currentUser = JSON.parse(localStorage.getItem("currentUser"));

    if (this.currentUser && !this.jwtHelperService.isTokenExpired(this.currentUser.token)) {
      if (this.roles.indexOf(this.currentUser.role) !== -1 && this.currentUser.role === "Admin") return true;
      else {
        this.router.navigate(["unauthorized"], { queryParams: { returnUrl: state.url } });
        return false;
      }
    }
    this.router.navigate(["login"], { queryParams: { returnUrl: state.url } });
    return false;
  }
}