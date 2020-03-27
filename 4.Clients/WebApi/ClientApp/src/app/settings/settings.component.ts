import { Component, OnInit } from '@angular/core';
import { AppComponent } from '../app.component';
import { CandidateProfile} from 'src/entities/Candidate-Profile';
import { Community } from 'src/entities/community';
import { FacadeService } from '../services/facade.service';
import { Office } from 'src/entities/office';
import { Room } from 'src/entities/room';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  constructor(private facade: FacadeService, private app: AppComponent) { }

  emptyCandidateProfile: CandidateProfile[] = [];
  listOfDisplayData = [...this.emptyCandidateProfile];

  emptyCommunity: Community[] = [];
  listOfDisplayDataCommunity = [...this.emptyCommunity];

  emptyRoom: Room[] = [];
  listOfDisplayDataRoom = [...this.emptyRoom];

  emptyOffice: Office[] = [];
  listOfDisplayDataOffice = [...this.emptyOffice];

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.app.hideLoading();
    this.getOffices();
    this.getRooms();
    this.getCandidatesProfile();
    this.getCommunities();
  }


  getCandidatesProfile() {
    this.facade.candidateProfileService.get()
      .subscribe(res => {
       this.emptyCandidateProfile = res;
       this.listOfDisplayData = res;
      }, err => {
        console.log(err);
      });
  }

  getCommunities() {
    this.facade.communityService.get()
      .subscribe(res => {
        this.emptyCommunity = res;
        this.listOfDisplayDataCommunity = res;
      }, err => {
        console.log(err);
      });
  }

  getRooms() {
    this.facade.RoomService.get()
    .subscribe(res => {
      this.emptyRoom = res;
      this.listOfDisplayDataRoom = res;
      }, err => {
      console.log(err);
    });
  }

  getOffices() {
    this.facade.OfficeService.get().subscribe(res => {
      this.emptyOffice = res;
      this.listOfDisplayDataOffice = res;
    }, err => {
      console.log(err);
    });
  }

  refresh(): void {
    this.getCommunities();
    this.getCandidatesProfile();
  }
}
