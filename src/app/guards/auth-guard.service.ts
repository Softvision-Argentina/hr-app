import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router, NavigationEnd, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from 'src/entities/user';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  previousUrl: string;
  currentUrl: string;
  currentUser: User;

  constructor(private jwtHelperService: JwtHelperService, private router: Router) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.currentUser = JSON.parse(localStorage.getItem("currentUser"));

    if (this.currentUser && !this.jwtHelperService.isTokenExpired(this.currentUser.token)){
      return true;
    }
    this.router.navigate(["login"], { queryParams: { returnUrl: state.url} });
    return false;
  }
}