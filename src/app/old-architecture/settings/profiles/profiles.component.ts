import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Community } from '@shared/models/community.model';
import { FacadeService } from '@shared/services/facade.service';
@Component({
  selector: 'app-profiles.component',
  templateUrl: './profiles.component.html',
  styleUrls: ['./profiles.component.scss']
})
export class ProfilesComponent implements OnInit {
  tab: string;
  emptyCandidateProfile: CandidateProfile[] = [];
  listOfDisplayData = [...this.emptyCandidateProfile];

  emptyCommunity: Community[] = [];
  listOfDisplayDataCommunity = [...this.emptyCommunity];

  constructor(private route: ActivatedRoute, private facade: FacadeService) { }

  ngOnInit() {
    this.getCandidatesProfile();
    this.getCommunities();
    this.tab = this.route.snapshot.params['tab'];
    this.route.params.subscribe(() => {
      this.tab = this.route.snapshot.params['tab'];
    });
  }

  getCandidatesProfile() {
    this.facade.candidateProfileService.getData()
      .subscribe(res => {
        this.emptyCandidateProfile = res;
        this.listOfDisplayData = res;
      }, err => {
        console.log(err);
      });
  }

  getCommunities() {
    this.facade.communityService.getData()
      .subscribe(res => {
        this.emptyCommunity = res;
        this.listOfDisplayDataCommunity = res;
      }, err => {
        console.log(err);
      });
  }
}

