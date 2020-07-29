import { Component, Input, OnInit, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Community } from '@shared/models/community.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { SettingsComponent } from '../settings/settings.component';

@Component({
  selector: 'app-communities',
  templateUrl: './communities.component.html',
  styleUrls: ['./communities.component.scss'],
})
export class CommunitiesComponent implements OnInit {

  @Input()
  private _detailedCommunity: Community[] = [];
  public get detailedCommunity(): Community[] {
    return this._detailedCommunity;
  }
  public set detailedCommunity(value: Community[]) {
    this._detailedCommunity = value;
  }

  @Input()
  private _detailedCandidateProfile: CandidateProfile[] = [];
  public get detailedCandidateProfile(): CandidateProfile[] {
    return this._detailedCandidateProfile;
  }
  public set detailedCandidateProfile(value: CandidateProfile[]) {
    this._detailedCandidateProfile = value;
  }

  currentUser: User;
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string[] }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  isEdit = false;
  editingCommunityId = 0;
  candidateprofiles: CandidateProfile[] = [];


  constructor(private facade: FacadeService, private fb: FormBuilder, private settings: SettingsComponent) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.facade.appService.removeBgImage();
    this.resetForm();
    this.getCandidateProfiles();
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._detailedCandidateProfile;
    this.getCandidateProfiles();
  }

  getCProfileNameByID(id: number) {
    const CProfile = this.candidateprofiles.find(c => c.id === id);
    return CProfile !== undefined ? CProfile.name : '';
  }

  getProfileById(id: number) {
    const CProfile = this.candidateprofiles.find(c => c.id === id);
    return CProfile;
  }

  getCandidateProfiles() {
    this.facade.candidateProfileService.get()
      .subscribe(res => {
        this.candidateprofiles = res.sort((a, b) => (a.name).localeCompare(b.name));
        this.facade.appService.stopLoading();
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
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
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Add New Community',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
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
                profile: null
              };
              this.facade.communityService.add(newCommunity)
                .subscribe(res => {
                  this.controlArray = [];
                  this.facade.toastrService.success('Community was successfully created !');
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
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
    let editedCommunity: Community = this._detailedCommunity.filter(community => community.id === id)[0];

    this.fillCommunityForm(editedCommunity);

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Edit Community',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
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
              editedCommunity = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                profileId: this.validateForm.controls['profileId'].value.toString(),
                profile: null
              };
              this.facade.communityService.update(id, editedCommunity)
                .subscribe(res => {
                  this.facade.toastrService.success('Community was successfully edited !');
                  this.getCandidateProfiles();
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            }
          }
        }],
    });
  }

  showDeleteConfirm(communityID: number): void {
    const communityDelete: Community = this._detailedCommunity.filter(c => c.id === communityID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + communityDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.communityService.delete(communityID)
        .subscribe(res => {
          this.facade.toastrService.success('Community was deleted !');
          this.getCandidateProfiles();
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  fillCommunityForm(community: Community) {
    this.validateForm.controls['name'].setValue(community.name);
    this.validateForm.controls['description'].setValue(community.description);
    if (this.candidateprofiles.length > 0) {
      this.validateForm.controls['profileId'].setValue(community.profileId);
    }
  }
}
