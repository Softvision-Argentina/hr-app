import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppComponent } from '../app.component';
import { CandidateProfile} from 'src/entities/Candidate-Profile';
import { Community } from 'src/entities/community';
import { FacadeService } from '../services/facade.service';
import { Office } from 'src/entities/office';
import { Room } from 'src/entities/room';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit, OnDestroy {

  constructor(private facade: FacadeService, private app: AppComponent) { }

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
    this.app.removeBgImage();
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
