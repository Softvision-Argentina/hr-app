import { Component, Input, OnChanges, OnInit, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Community } from '@shared/models/community.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { SettingsComponent } from '../settings.component';

@Component({
  selector: 'app-candidates-profile',
  templateUrl: './candidates-profile.component.html',
  styleUrls: ['./candidates-profile.component.scss']
})
export class CandidatesProfileComponent implements OnInit, OnChanges {

  @Input()
  private _detailedCandidateProfile: CandidateProfile[] = [];
  public get detailedCandidateProfile(): CandidateProfile[] {
    return this._detailedCandidateProfile;
  }
  public set detailedCandidateProfile(value: CandidateProfile[]) {
    this._detailedCandidateProfile = value;
  }

  @Input()
  private _detailedCommunity: Community[];
  public get detailedCommunity(): Community[] {
    return this._detailedCommunity;
  }
  public set detailedCommunity(value: Community[]) {
    this._detailedCommunity = value;
  }

  currentUser: User;
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  isEdit = false;
  editingCandidateProfileId = 0;
  communitys: Community[] = [];

  isDetailsVisible = false;
  isAddVisible = false;
  detailForm: FormGroup;
  emptyCandidateProfile: CandidateProfile;
  CommunitysForDetail: string[];


  constructor(private facade: FacadeService, private fb: FormBuilder, private settings: SettingsComponent) { }

  ngOnInit() {
    this.facade.appService.removeBgImage();
    this.getCommunity();
    this.resetForm();

    this.detailForm = this.fb.group({
      name: [null, [Validators.required]],
      description: [null, [Validators.required]],
      community: [null, [Validators.required]],
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._detailedCandidateProfile;
    this.getCommunity();
  }

  getCommunity() {
    this.facade.communityService.get()
      .subscribe(res => {
        this.communitys = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
    this.facade.appService.stopLoading();
  }

  getCommunityNameByID(id: number) {
    const community = this.communitys.find(s => s.id === id);
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
              this.facade.candidateProfileService.add(newCandidatesProfile)
                .subscribe(res => {
                  this.controlArray = [];
                  this.facade.toastrService.success('Candidate Profile was successfully created !');
                  this.getCommunity();
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
    this.editingCandidateProfileId = id;
    this.isEdit = true;
    this.controlArray = [];
    this.controlEditArray = [];
    let editedCandidateProfile: CandidateProfile = this._detailedCandidateProfile.filter(candidateProfile => candidateProfile.id === id)[0];
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
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                communityItems: []
              };
              this.facade.candidateProfileService.update(id, editedCandidateProfile)
                .subscribe(res => {
                  this.facade.toastrService.success('Candidate was successfully edited !');
                  this.getCommunity();
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            }
          }
        }],
    });
  }

  showDetailsModal(CandidateProfileID: number): void {
    this.emptyCandidateProfile = this._detailedCandidateProfile.find(candidateProfile => candidateProfile.id === CandidateProfileID);
    this.isDetailsVisible = true;
  }

  showDeleteConfirm(CandidateProfileID: number): void {
    const CandidateProfileDelete: CandidateProfile = this._detailedCandidateProfile.filter(c => c.id === CandidateProfileID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + CandidateProfileDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.candidateProfileService.delete(CandidateProfileID)
        .subscribe(res => {
          this.facade.toastrService.success('Candidate was deleted !');
          this.getCommunity();
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
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
}
