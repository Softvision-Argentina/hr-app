import { JwtHelper } from 'angular2-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router, NavigationEnd, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from 'src/entities/user';

@Injectable()
export class AuthGuard implements CanActivate {

  previousUrl: string;
  currentUrl: string;
  currentUser: User;

  constructor(private jwtHelper: JwtHelper, private router: Router) {

  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));

    if (this.currentUser && !this.jwtHelper.isTokenExpired(this.currentUser.token)) {
      return true;
    }
    this.router.navigate(['login'], { queryParams: { returnUrl: state.url} });
    return false;
  }
}
