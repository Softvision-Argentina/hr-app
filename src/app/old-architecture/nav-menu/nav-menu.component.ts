import { Component, OnInit, TemplateRef } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Candidate } from '@shared/models/candidate.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { ReferralsService } from '@shared/services/referrals.service';
import { GoogleSigninComponent } from '../login/google-signin.component';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
  providers: [GoogleSigninComponent]
})
export class NavMenuComponent implements OnInit {

  constructor(
    private router: Router,
    private google: GoogleSigninComponent,
    private facade: FacadeService,
    private _referralsService: ReferralsService
  ) { }
  isExpanded: boolean = false;
  currentUser: User = null;
  showUserSettings: boolean = false;
  url: string = '';
  search: string = '';
  searchbarPlaceholder = 'Search...';
  displayNavAndSideMenu: boolean;
  candidateInfo: Candidate;

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.router.events.subscribe(data => {
      this.search = '';
    });
    this.getPlaceholder();

    this._referralsService._displayNavAndSideMenuSource.subscribe(instruction => this.displayNavAndSideMenu = instruction);

    if (this.currentUser.role !== 'Employee') {
      this._referralsService.displayNavAndSideMenu(true);
    }

    this._referralsService._candidateInfoSource.subscribe(info => this.candidateInfo = info);
  }

  logout() {
    let emptyCandidate: Candidate;
    this._referralsService.sendCandidateInfo(emptyCandidate);
    this._referralsService.startReferralsModal(false);
    this.google.logout();
  }

  showPreferencesModal(modalContent: TemplateRef<{}>) {
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Manage Preferences',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: null,
      nzWidth: '30%'
    });
  }

  onSearchChange(search: string) {
    this.facade.searchbarService.search(this.router.url, search);
  }

  changeUserSettings() {
    this.showUserSettings = !this.showUserSettings;
  }

  private getPlaceholder() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        switch (event.url) {
          case '/dashboard':
            this.searchbarPlaceholder = 'Search for a dashboard';
            break;
          case '/':
            this.searchbarPlaceholder = 'Search for a Process using candidate\'s name';
            break;
          case '/people':
            this.searchbarPlaceholder = 'Search for a candidate';
            break;
          case '/tasks':
            this.searchbarPlaceholder = 'Search for a Task';
            break;
          case '/daysOff':
            this.searchbarPlaceholder = 'Search for a candidate\'s daysoff using his/her Dni';
            break;
          case '/referrals':
            this.searchbarPlaceholder = 'Search for your own referrals';
            break;
          case '/reports':
            this.searchbarPlaceholder = 'Search for a report';
            break;
          case '/settings':
            this.searchbarPlaceholder = 'Search for a setting';
            break;
          default:
            this.searchbarPlaceholder = 'Search...';
            break;
        }
      }
    });
  }
}