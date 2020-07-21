import { Component, OnInit } from '@angular/core';
import { ReferralsService } from '../services/referrals.service';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss']
})
export class SideMenuComponent implements OnInit {
  hideReferralsIcon = false;
  displayNavAndSideMenu: boolean;

  constructor(
    private _referralsService: ReferralsService
  ) { }

  ngOnInit() {
    this._referralsService._displayNavAndSideMenuSource.subscribe(
      instruction => this.displayNavAndSideMenu = instruction
    );


  }

}
