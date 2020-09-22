import { Component, OnInit, TemplateRef, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Community } from '@shared/models/community.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { CandidateProfilesSandbox } from './candidates-profile.sandbox';

@Component({
  selector: 'app-candidates-profile',
  templateUrl: './candidates-profile.component.html',
  styleUrls: ['./candidates-profile.component.scss']
})
export class CandidatesProfileComponent implements OnInit, OnDestroy {

  currentUser: User;
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  isEdit = false;
  editingCandidateProfileId = 0;
  communities: Community[];

  isDetailsVisible = false;
  isAddVisible = false;
  detailForm: FormGroup;
  emptyCandidateProfile: CandidateProfile;
  communitiesForDetail: string[];
  detailedCandidateProfile: CandidateProfile[];
  getCandidateProfiles: any;
  getCommunities: any;
  getCandidateProfilesFailed: any;
  getCandidateProfilesErrorMsg: any;
  result: any;
  errorMsg: [];

  constructor(private facade: FacadeService, private fb: FormBuilder, private candidateProfilesSandbox: CandidateProfilesSandbox) { }

  ngOnInit() {
    this.facade.appService.removeBgImage();
    this.candidateProfilesSandbox.loadCandidateProfiles();
    this.candidateProfilesSandbox.loadCommunities();
    this.getCandidateProfiles = this.candidateProfilesSandbox.candidateProfiles$.subscribe(candidateProfiles => this.detailedCandidateProfile = candidateProfiles);
    this.getCommunities = this.candidateProfilesSandbox.communities$.subscribe(communities => this.communities = communities);
    this.getCandidateProfilesFailed = this.candidateProfilesSandbox.candidateProfilesFailed$.subscribe(failed => this.result = failed);
    this.getCandidateProfilesErrorMsg = this.candidateProfilesSandbox.candidateProfilesErrorMsg$.subscribe(msg => this.errorMsg = msg);
    this.resetForm();

    this.detailForm = this.fb.group({
      name: [null, [Validators.required]],
      description: [null, [Validators.required]],
      community: [null, [Validators.required]],
    });
  }

  getCommunityNameByID(id: number) {
    const community = this.communities.find(s => s.id === id);
    return community !== undefined ? community.name : '';
  }

  resetForm() {
    this.validateForm = this.fb.group({
      name: [null, Validators.required],
      description: [null, Validators.required]
    });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.isEdit = false;
    this.controlArray = [];
    this.controlEditArray = [];
    this.resetForm();

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Add New Candidate Profile',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        {
          label: 'Cancel',

          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            let isCompleted = true;
            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls[i]) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if ((!this.validateForm.controls[i].valid)) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              const newCandidatesProfile: CandidateProfile = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                communityItems: []
              };
              this.candidateProfilesSandbox.addCandidateProfile(newCandidatesProfile);
              setTimeout(() => {
                if (this.result === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.result === false) {
                  this.controlArray = [];
                  this.facade.toastrService.success('Candidate Profile was successfully created !');
                  modal.destroy();
                }
                this.candidateProfilesSandbox.resetFailed();
              }, 500);
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.resetForm();
    this.editingCandidateProfileId = id;
    this.isEdit = true;
    this.controlArray = [];
    this.controlEditArray = [];
    let editedCandidateProfile: CandidateProfile = this.detailedCandidateProfile.filter(candidateProfile => candidateProfile.id === id)[0];
    this.fillCandidateProfileForm(editedCandidateProfile);

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Edit Candidate Profile',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        {
          label: 'Cancel',

          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            let isCompleted = true;
            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls[i]) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if (!this.validateForm.controls[i].valid) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              editedCandidateProfile = {
                id,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                communityItems: editedCandidateProfile.communityItems
              };
              this.candidateProfilesSandbox.editCandidateProfile(editedCandidateProfile);
              setTimeout(() => {
                if (this.result === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.result === false) {
                  this.facade.toastrService.success('Candidate Profile was successfully edited!');
                  modal.destroy();
                }
                this.candidateProfilesSandbox.resetFailed();
              }, 500);
            }
          }
        }],
    });
  }

  showDetailsModal(CandidateProfileID: number): void {
    this.emptyCandidateProfile = this.detailedCandidateProfile.find(candidateProfile => candidateProfile.id === CandidateProfileID);
    this.isDetailsVisible = true;
  }

  showDeleteConfirm(CandidateProfileID: number): void {
    const CandidateProfileDelete: CandidateProfile = this.detailedCandidateProfile.filter(c => c.id === CandidateProfileID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + CandidateProfileDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {

        this.candidateProfilesSandbox.removeCandidateProfile(CandidateProfileID);
        setTimeout(() => {
          if (this.result === true) {
            this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
          } else if (this.result === false) {
            this.facade.toastrService.success('Candidate Profile was deleted!');
          }
          this.candidateProfilesSandbox.resetFailed();
        }, 500);
      }
    });
  }

  fillCandidateProfileForm(candidateProfile: CandidateProfile) {
    this.validateForm.controls['name'].setValue(candidateProfile.name);
    this.validateForm.controls['description'].setValue(candidateProfile.description);
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyCandidateProfile = {
      id: 0,
      name: '',
      description: '',
      communityItems: []
    };
  }

  getColor(candidateCommunity: Community[], community: Community): string {
    const colors: string[] = ['red', 'volcano', 'orange', 'gold', 'lime', 'green', 'cyan', 'blue', 'geekblue', 'purple'];
    let index: number = candidateCommunity.indexOf(community);
    if (index > colors.length) {
      index = parseInt((index / colors.length).toString().split(',')[0], 10);
    }
    return colors[index];
  }

  ngOnDestroy() {
    this.getCandidateProfiles.unsubscribe();
    this.getCommunities.unsubscribe();
    this.getCandidateProfilesFailed.unsubscribe();
    this.getCandidateProfilesErrorMsg.unsubscribe();
  }
}
