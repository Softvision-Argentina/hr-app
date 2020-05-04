import { Component, Renderer2, OnInit, OnDestroy } from '@angular/core';
import { GoogleSigninComponent } from './login/google-signin.component';
import { User } from 'src/entities/user';
import { FacadeService } from './services/facade.service';
import { AppConfig } from './app-config/app.config';
import { INg2LoadingSpinnerConfig, ANIMATION_TYPES } from 'ng2-loading-spinner';
import { Subscription } from 'rxjs/Subscription';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [GoogleSigninComponent]
})
export class AppComponent   {
  title = 'app';
  showSpinner = true;
  showBigImage = true;
  loadingConfig: INg2LoadingSpinnerConfig = {
    animationType: ANIMATION_TYPES.scalingBars,
    backdropColor: 'rgba(0, 0, 0, 0.7)',
    spinnerColor: '#fff',
    spinnerPosition: 'center',
    backdropBorderRadius: '0px',
    spinnerSize: 'xl'
  };
  loadingSubscription: Subscription;
  showBigImageSubscription: Subscription;

  constructor(
    private renderer: Renderer2,
    private google: GoogleSigninComponent,
    private facade: FacadeService,
    private config: AppConfig
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
  }

  changeBg() {
    if (this.google.isUserAuthenticated()) {
      this.removeBgImage();
    } else {
      this.renderBgImage();
    }
  }

  isUserRole(roles: string[]): boolean {
    const currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser.role === '') { this.facade.userService.getRoles(); }
    if (roles[0] === 'ALL') { roles = this.config.getConfig('roles'); }
    if (roles.indexOf(currentUser.role) !== -1) { return true; } else { return false; }
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
