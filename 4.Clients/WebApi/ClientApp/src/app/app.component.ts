import { Component, Renderer2 } from '@angular/core';
import { GoogleSigninComponent } from './login/google-signin.component';
import { User } from 'src/entities/user';
import { FacadeService } from './services/facade.service';
import { AppConfig } from './app-config/app.config';
import { INg2LoadingSpinnerConfig, ANIMATION_TYPES } from 'ng2-loading-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [GoogleSigninComponent]
})
export class AppComponent   {
  title = 'app';
  showSpinner = true;
  loadingConfig: INg2LoadingSpinnerConfig = {
    animationType: ANIMATION_TYPES.scalingBars,
    backdropColor: 'rgba(0, 0, 0, 0.7)',
    spinnerColor: '#fff',
    spinnerPosition: 'center',
    backdropBorderRadius: '0px',
    spinnerSize: 'xl'
  };

  constructor(
    private renderer: Renderer2,
    private google: GoogleSigninComponent,
    private facade: FacadeService,
    private config: AppConfig) {
    setTimeout(() => {
      this.showSpinner = false;
    }, 1500);
  }


  isUserRole(roles: string[]): boolean {
    const currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser.role === '') { this.facade.userService.getRoles(); }
    if (roles[0] === 'ALL') { roles = this.config.getConfig('roles'); }
    if (roles.indexOf(currentUser.role) !== -1) { return true; } else { return false; }
  }

  renderBgImage() {
    this.renderer.setStyle(document.querySelector('app-root').firstElementChild, 'background-size', '100% 100%');
    this.renderer.setStyle(document.querySelector('app-root').firstElementChild, 'background-position', 'top center');
    this.renderer.setStyle(document.querySelector('app-root').firstElementChild, 'background-repeat', 'no-repeat');
    this.renderer.setStyle(document.querySelector('app-root').firstElementChild, 'background-size', 'cover');
    this.renderer.setStyle(document.querySelector('app-root').firstElementChild, 'width', '100%');
    this.renderer.setStyle(document.querySelector('app-root').firstElementChild, 'height', '100%');
  }

  removeBgImage() {
    this.renderer.setStyle(document.querySelector('app-root').firstElementChild, 'background-image', 'none');
    this.renderer.setStyle(document.querySelector('app-root').firstElementChild, 'margin-top', '0%');
  }

  showLoading() {
    this.showSpinner = true;
  }

  hideLoading() {
    this.showSpinner = false;
  }

  isAuthenticated() {
    return localStorage.getItem('currentUser') !== null;
  }
}
