import { Component, OnDestroy, OnInit } from '@angular/core';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Community } from '@shared/models/community.model';
import { Office } from '@shared/models/office.model';
import { Room } from '@shared/models/room.model';
import { FacadeService } from '@shared/services/facade.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit, OnDestroy {

  constructor(private facade: FacadeService) { }

  emptyCandidateProfile: CandidateProfile[] = [];
  listOfDisplayData = [...this.emptyCandidateProfile];
  emptyCommunity: Community[] = [];
  listOfDisplayDataCommunity = [...this.emptyCommunity];
  emptyRoom: Room[] = [];
  listOfDisplayDataRoom = [...this.emptyRoom];
  emptyOffice: Office[] = [];
  listOfDisplayDataOffice = [...this.emptyOffice];
  settingsSubscription: Subscription = new Subscription();

  ngOnInit() {
    this.facade.appService.startLoading();
    this.facade.appService.removeBgImage();
    this.facade.appService.stopLoading();
    this.getOffices();
    this.getRooms();
    this.getCandidatesProfile();
    this.getCommunities();
  }


  getCandidatesProfile() {
    const candidateProfilesSubscription = this.facade.candidateProfileService.getData().subscribe(res => {
      if (!!res) {
        this.emptyCandidateProfile = res;
        this.listOfDisplayData = res;
      }
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.settingsSubscription.add(candidateProfilesSubscription);
  }

  getCommunities() {
    const communitySubscription = this.facade.communityService.getData().subscribe(res => {
      if (!!res) {
        this.emptyCommunity = res;
        this.listOfDisplayDataCommunity = res;
      }
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.settingsSubscription.add(communitySubscription);
  }

  getRooms() {
    this.facade.RoomService.get()
      .subscribe(res => {
        this.emptyRoom = res;
        this.listOfDisplayDataRoom = res;
      }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
  }

  getOffices() {
    const officesSubscription = this.facade.OfficeService.getData().subscribe(res => {
      if (!!res) {
        this.emptyOffice = res;
        this.listOfDisplayDataOffice = res;
      }
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.settingsSubscription.add(officesSubscription);
  }

  refresh(): void {
    this.getCommunities();
    this.getCandidatesProfile();
  }

  ngOnDestroy() {
    this.settingsSubscription.unsubscribe();
  }
}
