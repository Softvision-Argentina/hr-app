import { Component, AfterViewInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { User } from 'src/entities/user';
import { Router } from '@angular/router';
import { AppConfig } from './../app-config/app.config';
import { FacadeService } from '../services/facade.service';
import { JwtHelper } from 'angular2-jwt';
declare const gapi: any;

@Component({
  selector: 'google-signin',
  templateUrl: './google-signin.component.html',
  styleUrls: ['./google-signin.component.css']
})
export class GoogleSigninComponent implements AfterViewInit {

  @ViewChild("googleBtn")
  public googleBtn: ElementRef;

  private clientId: string = this.appConfig.getConfig("clientId");

  private scope = this.appConfig.getConfig("scopes").join(' ');

  public auth2: any;

  constructor(private jwtHelper: JwtHelper, private element: ElementRef, private router: Router, public zone: NgZone, private appConfig: AppConfig,
    private facade: FacadeService) {
  }

  public googleInit() {
    let that = this;
    sessionStorage.clear();
    gapi.load('auth2', function () {
      that.auth2 = gapi.auth2.init({
        client_id: that.clientId,
        cookiepolicy: 'single_host_origin',
        scope: that.scope
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
          ID: profile.getId(),
          Name: profile.getName(),
          ImgURL: profile.getImageUrl(),
          Email: profile.getEmail(),
          Role: '',
          Token: googleUser.getAuthResponse().id_token
        }

        // console.log(currentUser);
        // localStorage.setItem('currentUser', JSON.stringify(currentUser));
        // that.facade.userService.getRoles();

        that.externalLogin(currentUser);

      }, function (error) {
        that.zone.run(() => { that.router.navigate(['/unauthorized']); });
        this.facade.toastrService.error('An error has ocurred, please try again with another account.');
        that.eraseCookie('accounts.google.com');
      });
  }

  externalLogin(gUser: User) {
    this.facade.authService.externalLogin(gUser.Token)
      .subscribe(res => {

        if (res != null) {
          let currentUser: User = {
            ID: res.user.id,
            Name: res.user.firstName + " " + res.user.lastName,
            ImgURL: gUser.ImgURL,
            Email: res.user.username,
            Role: res.user.role,
            Token: res.token
          }

          localStorage.setItem('currentUser', JSON.stringify(currentUser));
          this.facade.userService.getRoles();
          this.facade.modalService.closeAll();
          this.zone.run(() => { this.navigateByRole(currentUser.Role) });
        }
      }, err => {
        this.zone.run(() => { this.router.navigate(['/unauthorized']); });
        this.eraseCookie('accounts.google.com');
        console.log(err);
      });
  }

  ngAfterViewInit() {
    this.googleInit();
    this.isUserAuthenticated();
  }

  isUserAuthenticated(): boolean {
    let currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser != null && !this.jwtHelper.isTokenExpired(currentUser.Token)) {
      return true;
    }
    else {
      localStorage.clear();
      return false;
    }
  }

  logout() {
    //////////////////////////////////////// Uncomment block for live deployment //////////////////////////////

    // var auth2 = gapi.auth2.getAuthInstance();
    // auth2.signOut().then(function () {
    localStorage.clear();
    this.router.navigate(['/login']);
    // });

    //////////////////////////////////////////////////////////////////////////////////////////////////////////
  }

  navigateByRole(role: string) {
    if (role === 'Common') {
      this.router.navigate(['/referrals']);
    }
    else {
      this.router.navigate(['/'])
    }
  }
  eraseCookie(domain: string) {
    document.cookie = domain + '=; Max-Age=-99999999;';
  }

}