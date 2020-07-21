import { Component } from '@angular/core';
import { GoogleSigninComponent } from './login/google-signin.component';
import { FacadeService } from './services/facade.service';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ReferralsService } from './services/referrals.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [ GoogleSigninComponent ]
})
export class AppComponent   {
  title = 'recru-webapp';
  version = 'Angular version 9.1.11';
  showSpinner = true;
  showBigImage = true;
  loadingSubscription: Subscription;
  showBigImageSubscription: Subscription;
  displayNavAndSideMenu: boolean;

  constructor(
    private google: GoogleSigninComponent,
    private facade: FacadeService,
    private _referralsService: ReferralsService
  ) { }

  ngOnInit(): void {
    this.changeBg();
    this.loadingSubscription = this.facade.appService.loadingStatus.pipe(
      debounceTime(200)
    ).subscribe((value) => {
      this.showSpinner = value;
    });
    this.showBigImageSubscription = this.facade.appService.showBigImageStatus.pipe(
      debounceTime(200)
    ).subscribe((value) => {
      this.showBigImage = value;
    });

    this._referralsService._displayNavAndSideMenuSource.subscribe(instruction => this.displayNavAndSideMenu = instruction);
  }

  changeBg() {
    if (this.google.isUserAuthenticated()) {
      this.removeBgImage();
    } else {
      this.renderBgImage();
    }
  }

  isUserRole(roles: string[]): boolean {
    return this.facade.appService.isUserRole(roles);
  }

  renderBgImage() {
    this.facade.appService.showBgImage();
  }

  removeBgImage() {
    this.facade.appService.removeBgImage();
  }

  showLoading() {
    this.facade.appService.startLoading();
  }

  hideLoading() {
    this.facade.appService.stopLoading();
  }

  isAuthenticated() {
    return localStorage.getItem('currentUser') !== null;
  }
}
