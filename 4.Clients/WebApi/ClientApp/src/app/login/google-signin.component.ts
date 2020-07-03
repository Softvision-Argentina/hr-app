import { Component, AfterViewInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from 'src/entities/user';
import { AppConfig } from './../app-config/app.config';
import { FacadeService } from '../services/facade.service';
declare const gapi: any;

@Component({
  selector: 'google-signin',
  templateUrl: './google-signin.component.html',
  styleUrls: ['./google-signin.component.scss']
})
export class GoogleSigninComponent implements AfterViewInit {

  @ViewChild("googleBtn")
  public googleBtn: ElementRef;

  private clientId: string = this.appConfig.getConfig("clientId");

  private scope = this.appConfig.getConfig("scopes").join(' ');

  public auth2: any;

  constructor(private jwtHelperService: JwtHelperService, private element: ElementRef, private router: Router, public zone: NgZone, private appConfig: AppConfig,
    private facade: FacadeService) {
  }

  public googleInit() {
    let that = this;
    sessionStorage.clear();
    gapi.load('auth2', function () {
      that.auth2 = gapi.auth2.init({
        client_id: that.clientId,
        cookiepolicy: 'single_host_origin',
        scope: that.scope,
        sameSite: 'None',
        secure: true
      });
      that.attachSignin(that.element.nativeElement.firstChild);
    });
  }
  public attachSignin(element) {
    let that = this;
    this.auth2.attachClickHandler(element, {},
      function (googleUser) {

        let profile = googleUser.getBasicProfile();

        let currentUser: User = {
          id: profile.getId(),
          firstName: profile.getName().split(' ')[0],
          lastName: profile.getName().split(' ')[1],
          imgURL: profile.getImageUrl(),
          username: profile.getEmail(),
          role: '',
          token: googleUser.getAuthResponse().id_token,
          community: null,
          userDashboards: []
        }
        that.externalLogin(currentUser);


      }, function (error) {
        that.zone.run(() => { that.router.navigate(['/unauthorized']); });
        this.facade.toastrService.error('An error has ocurred, please try again with another account.');
        that.eraseCookie('accounts.google.com');
      });
  }

  externalLogin(gUser: User) {
    this.facade.authService.externalLogin(gUser.token)
      .subscribe(res => {

        if (res) {
          let currentUser: User = {
            id: res.user.id,
            firstName: res.user.firstName,
            lastName: res.user.lastName,
            imgURL: gUser.imgURL,
            username: res.user.username,
            role: res.user.role,
            token: res.token,
            community: res.user.community,
            userDashboards: []
          }

          localStorage.setItem('currentUser', JSON.stringify(currentUser));
          this.facade.userService.getRoles();
          this.facade.modalService.closeAll();
          this.zone.run(() => { this.navigateByRole(currentUser.role) });
        }
      }, err => {
        this.zone.run(() => { this.router.navigate(['/unauthorized']); });
        this.eraseCookie('accounts.google.com');
      });
  }

  ngAfterViewInit() {
    this.googleInit();
    this.isUserAuthenticated();
  }

  isUserAuthenticated(): boolean {
    let currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && !this.jwtHelperService.isTokenExpired(currentUser.token)) {
      return true;
    }
    else {
      localStorage.clear();
      return false;
    }
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  navigateByRole(role: string) {
    if (role === 'Common') {
      this.router.navigate(['/referrals']);
    } else if (role === 'Employee') {
      this.router.navigate(['/welcome']);
    } else {
      this.router.navigate(['/'])
    }
  }
  eraseCookie(domain: string) {
    document.cookie = domain + '=; Max-Age=-99999999;';
  }

}
