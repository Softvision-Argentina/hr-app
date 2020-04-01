import { Component, TemplateRef, OnInit } from '@angular/core';
import { JwtHelper } from 'angular2-jwt';
import { Router } from '@angular/router';
import { GoogleSigninComponent } from '../login/google-signin.component';
import { AppComponent } from '../app.component';
import { User } from 'src/entities/user';
import { FacadeService } from '../services/facade.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  providers: [GoogleSigninComponent]
})
export class NavMenuComponent implements OnInit {

  constructor(
    private jwtHelper: JwtHelper,
    private _appComponent: AppComponent,
    private router: Router,
    private google: GoogleSigninComponent,
    private facade: FacadeService
  ) { }
  isExpanded = false;
  currentUser: User;
  showUserSettings = false;

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  logout() {
    this.google.logout();
  }

  showPreferencesModal(modalContent: TemplateRef<{}>) {
    const modal = this.facade.modalService.create({
      nzTitle: 'Manage Preferences',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: null,
      nzWidth: '30%'
    });
  }

  onSearchChange( search: string) {
    this.facade.searchbarService.search(this.router.url, search);
  }

  changeUserSettings() {
    this.showUserSettings = !this.showUserSettings;
  }
}
