import { Component, Input, OnInit, SimpleChanges, TemplateRef, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Community } from '@shared/models/community.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { CommunitiesSandbox } from './communities.sandbox';

@Component({
  selector: 'app-communities',
  templateUrl: './communities.component.html',
  styleUrls: ['./communities.component.scss'],
})
export class CommunitiesComponent implements OnInit, OnDestroy {

  currentUser: User;
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string[] }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  isEdit = false;
  editingCommunityId = 0;
  candidateprofiles: CandidateProfile[] = [];
  detailedCommunity: any;
  result: any;
  errorMsg: [];
  getCommunities: any;
  getCommunitiesFailed: any;
  getCommunitiesErrorMsg: any;
  getProfiles: any;

  constructor(private facade: FacadeService, private fb: FormBuilder, public communitiesSandbox: CommunitiesSandbox) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.facade.appService.removeBgImage();
    this.resetForm();
    this.communitiesSandbox.loadCommunities();
    this.communitiesSandbox.loadCandidateProfiles();
    setTimeout(() => {
      this.getCommunities = this.communitiesSandbox.communities$.subscribe(communities => this.detailedCommunity = communities);
    });

    this.getProfiles = this.communitiesSandbox.candidateProfiles$.subscribe(profiles => {
      this.candidateprofiles = profiles;
    });

    this.getCommunitiesFailed = this.communitiesSandbox.communtiesFailed$.subscribe(failed => this.result = failed);
    this.getCommunitiesErrorMsg = this.communitiesSandbox.communtiesErrorMsg$.subscribe(msg => this.errorMsg = msg);
  }

  getCProfileNameByID(id: any) {
    const CProfile = this.candidateprofiles.find(c => c.id === parseInt(id, 10));
    return CProfile !== undefined ? CProfile.name : '';
  }

  getProfileById(id: number) {
    const CProfile = this.candidateprofiles.find(c => c.id === id);
    return CProfile;
  }

  resetForm() {
    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      description: [null, Validators.required],
      profileId: [null, Validators.required]
    });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.isEdit = false;
    this.controlArray = [];
    this.controlEditArray = [];
    this.resetForm();

    if (this.candidateprofiles.length > 0) {
      this.validateForm.controls['profileId'].setValue(this.candidateprofiles[0].id);
    }

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Add New Community',
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
              const newCommunity: Community = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                profileId: this.validateForm.controls['profileId'].value.toString(),
                profile: null,
              };
              this.communitiesSandbox.addCommunity(newCommunity);
              setTimeout(() => {
                if (this.result === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.result === false) {
                  this.controlArray = [];
                  this.facade.toastrService.success('Community was successfully created!');
                  modal.destroy();
                }
                this.communitiesSandbox.resetFailed();
              }, 500);
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.resetForm();
    this.editingCommunityId = id;
    this.isEdit = true;
    this.controlArray = [];
    this.controlEditArray = [];
    let communityToEdit: Community = this.detailedCommunity.filter(community => community.id === id)[0];

    this.fillCommunityForm(communityToEdit);

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Edit Community',
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
              let editedCommunity = {
                id,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                profileId: this.validateForm.controls['profileId'].value,
                profile: null
              };

              this.communitiesSandbox.editCommunity(editedCommunity);
              setTimeout(() => {
                if (this.result === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.result === false) {
                  this.facade.toastrService.success('Community was successfully edited!');
                  modal.destroy();
                }
                this.communitiesSandbox.resetFailed();
              }, 500);
            }
          }
        }],
    });
  }

  showDeleteConfirm(communityID: number): void {
    const communityDelete: Community = this.detailedCommunity.filter(c => c.id === communityID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + communityDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.communitiesSandbox.removeCommunity(communityID);
        setTimeout(() => {
          if (this.result === true) {
            this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
          } else if (this.result === false) {
            this.facade.toastrService.success('Community was deleted!');
          }
          this.communitiesSandbox.resetFailed();
        }, 500);
      }
    });
  }

  fillCommunityForm(community: Community) {
    this.validateForm.controls['name'].setValue(community.name);
    this.validateForm.controls['description'].setValue(community.description);
    if (this.candidateprofiles.length > 0) {
      this.validateForm.controls['profileId'].setValue(community.profileId);
    }
  }

  ngOnDestroy() {
    this.getCommunities.unsubscribe();
    this.getCommunitiesFailed.unsubscribe();
    this.getCommunitiesErrorMsg.unsubscribe();
    this.getProfiles.unsubscribe();
  }

}

