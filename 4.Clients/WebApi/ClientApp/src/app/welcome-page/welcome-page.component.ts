import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { FacadeService } from '../services/facade.service';
import { ReferralsService } from '../services/referrals.service';
import { User } from '../../entities/user';

@Component({
  selector: 'app-welcome-page',
  templateUrl: './welcome-page.component.html',
  styleUrls: ['./welcome-page.component.scss']
})
export class WelcomePageComponent implements OnInit {
  currentUser: User = null;
  modalStart: boolean;
  displayNavAndSideMenu: boolean;

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this._referralsService.displayNavAndSideMenu(false);
    this.facade.appService.stopLoading();
    this._referralsService._startReferralsModalSource.subscribe(instruction => this.modalStart = instruction);
    this._referralsService._displayNavAndSideMenuSource.subscribe(instruction => this.displayNavAndSideMenu = instruction);
  }

  constructor(
    private facade: FacadeService,
    private router: Router,
    private _referralsService: ReferralsService
  ) { }

  startReferralsModal() {
    this._referralsService.startReferralsModal(true);
    this._referralsService.displayNavAndSideMenu(true);
    this.router.navigateByUrl('/referrals');
  }

  goToReferralsComponent() {
    this._referralsService.displayNavAndSideMenu(true);
    this.router.navigateByUrl('/referrals/openpositions');
  }

}
