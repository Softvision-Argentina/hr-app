import { Component, OnInit } from '@angular/core';
import { Community } from 'src/entities/community';
import { CandidateProfile } from 'src/entities/Candidate-Profile';
import { ActivatedRoute, Params } from '@angular/router';
import { FacadeService } from '../services/facade.service';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-profiles.component',
  templateUrl: './profiles.component.html',
  styleUrls: ['./profiles.component.css']
})
export class ProfilesComponent implements OnInit {
  tab: string;
  emptyCandidateProfile: CandidateProfile[] = [];
  listOfDisplayData = [...this.emptyCandidateProfile];
  
  emptyCommunity: Community[] = [];
  listOfDisplayDataCommunity = [...this.emptyCommunity];

  constructor(private route: ActivatedRoute, private facade: FacadeService, private app: AppComponent) {}

  ngOnInit() {
    this.getCandidatesProfile();
    this.getCommunities();
    this.tab=this.route.snapshot.params['tab'];
    this.route.params.subscribe((params: Params)=>{
      this.tab=this.route.snapshot.params['tab'];
    });
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
}
