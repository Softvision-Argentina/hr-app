import { Component, OnInit, TemplateRef, Input, ContentChild } from "@angular/core";
import { FacadeService } from "src/app/services/facade.service";
import { Candidate } from "src/entities/candidate";
import { CandidateSkill } from "src/entities/candidateSkill";
import { Consultant } from "src/entities/consultant";
import { Globals } from '../../app-globals/globals';
import { Community } from "src/entities/community";
import { CandidateProfile } from "src/entities/Candidate-Profile";


@Component({
    selector: 'candidate-details',
    templateUrl: './candidate-details.component.html',
    styleUrls: ['./candidate-details.component.css']
  })
  
  
  export class CandidateDetailsComponent implements OnInit {
    
    @Input()
    private _detailedCandidate: Candidate;
    public get detailedCandidate(): Candidate {
        return this._detailedCandidate;
    }
    public set detailedCandidate(value: Candidate) {
        this._detailedCandidate = value;
    }

    recruiterName: string = '';
    profileName: string = '';
    communityName: string = '';
    englishLevelList: any[] = [];
    statusList: any[] = [];

    constructor(private facade: FacadeService, private globals: Globals) {
      this.englishLevelList = globals.englishLevelList;
      this.statusList = globals.candidateStatusList;
     }

    ngOnInit(){
        this.getRecruiterName();
        this.getProfileName();
        this.getCommunityName();
    }

    getRecruiterName(){
        this.facade.consultantService.get()
        .subscribe(res => {
          this.recruiterName = res.filter(x => x.id === this._detailedCandidate.recruiter.id)[0].name + " " +
                                    res.filter(x => x.id === this._detailedCandidate.recruiter.id)[0].lastName;
        }, err => {
          console.log(err);
        });
      }

      getCommunityName(){
        this.facade.communityService.get()
        .subscribe(res => {
          this.communityName = res.filter(x => x.id === this._detailedCandidate.community.id)[0].name;
        }, err => {
          console.log(err);
        });
      }

      getProfileName(){
        this.facade.candidateProfileService.get()
        .subscribe(res => {
          this.profileName = res.filter(x => x.id === this._detailedCandidate.profile.id)[0].name;
        }, err => {
          console.log(err);
        });
      }

    showModal(modalContent: TemplateRef <{}>, fullName: string){
        fullName = fullName + "'s details";
        this.facade.modalService.create({
            nzTitle: fullName,
            nzContent: modalContent,
            nzClosable: true,
            nzWrapClassName: 'vertical-center-modal',
            nzFooter: null
        })
    }

    // getColor(candidateSkills: CandidateSkill[], skill: CandidateSkill): string {
    //     let colors: string[] = ['red', 'volcano', 'orange', 'gold', 'lime', 'green', 'cyan', 'blue', 'geekblue', 'purple'];
    //     let index: number = candidateSkills.indexOf(skill);
    //     if (index > colors.length) index = parseInt((index / colors.length).toString().split(',')[0]);
    //     return colors[index];
    //   }

    // getEnglishLevel(): string {
    //   return this.englishLevelList.filter(x => x.id === this._detailedCandidate.englishLevel)[0].name;
    // }

    getCandidateStatus(): string {
      return this.statusList.filter(x => x.id === this._detailedCandidate.status)[0].name;
    }
}