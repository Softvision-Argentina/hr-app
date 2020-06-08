import { Component, OnInit, TemplateRef, Input, ContentChild } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { Candidate } from 'src/entities/candidate';
import { Globals } from '../../app-globals/globals';

@Component({
  selector: 'candidate-details',
  templateUrl: './candidate-details.component.html',
  styleUrls: ['./candidate-details.component.scss']
})


export class CandidateDetailsComponent implements OnInit {

  @Input() _detailedCandidate: Candidate;
  public get detailedCandidate(): Candidate {
    return this._detailedCandidate;
  }
  public set detailedCandidate(value: Candidate) {
    this._detailedCandidate = value;
  }

  userName: string = '';
  profileName: string = '';
  communityName: string = '';
  englishLevelList: any[] = [];
  statusList: any[] = [];

  constructor(private facade: FacadeService, private globals: Globals) {
    this.englishLevelList = globals.englishLevelList;
    this.statusList = globals.candidateStatusList;
  }

  ngOnInit() {
    this.getRecruiterName();
    this.getProfileName();
    this.getCommunityName();
  }

  getRecruiterName() {
    this.facade.userService.get()
      .subscribe(res => {
        if (this._detailedCandidate) {
          this.userName = res.filter(candidate => candidate.id === this._detailedCandidate.user.id)[0].firstName + ' ' +
            res.filter(candidate => candidate.id === this._detailedCandidate.user.id)[0].lastName;
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getCommunityName() {
    this.facade.communityService.get()
      .subscribe(res => {
        if (this._detailedCandidate) {
          this.communityName = res.filter(community => community.id === this._detailedCandidate.community.id)[0].name;
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getProfileName() {
    this.facade.candidateProfileService.get()
      .subscribe(res => {
        if (this._detailedCandidate) {
          this.profileName = res.filter(profile => profile.id === this._detailedCandidate.profile.id)[0].name;
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  showModal(modalContent: TemplateRef<{}>, fullName: string) {
    const nameAndLastName = `${fullName}'s details`;
    this.facade.modalService.create({
      nzTitle: nameAndLastName,
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: null
    });
  }

  getCandidateStatus(): string {
    return this.statusList.filter(status => status.id === this._detailedCandidate.status)[0].name;
  }
}
