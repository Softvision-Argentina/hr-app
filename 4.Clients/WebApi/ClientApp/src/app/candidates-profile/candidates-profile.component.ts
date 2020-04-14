import { Component, OnInit, TemplateRef, Input, SimpleChanges, OnChanges } from '@angular/core';
import { AppComponent } from '../app.component';
import { CandidateProfile } from 'src/entities/Candidate-Profile';
import { FacadeService } from '../services/facade.service';
import { trimValidator } from '../directives/trim.validator';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from 'src/entities/user';
import { Community } from 'src/entities/community';
import { SettingsComponent } from '../settings/settings.component';

@Component({
  selector: 'app-candidates-profile',
  templateUrl: './candidates-profile.component.html',
  styleUrls: ['./candidates-profile.component.css']
})
export class CandidatesProfileComponent implements OnInit, OnChanges {

  @Input()
  private _detailedCandidateProfile: CandidateProfile[];
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

  currentConsultant: User;
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


  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent, private settings: SettingsComponent) { }

  ngOnInit() {
    this.app.removeBgImage();
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
  }

  getCommunityNameByID(id: number) {
    const community = this.communitys.find(s => s.id === id);
    return community !== undefined ? community.name : '';
  }

  resetForm() {
    this.validateForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]],
      description: [null, [Validators.required, trimValidator]]
    });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.isEdit = false;
    this.controlArray = [];
    this.controlEditArray = [];
    this.resetForm();

    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Candidate Profile',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            modal.nzFooter[1].loading = true; // el boton de guardar cambios cambia a true
            let isCompleted = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if ((!this.validateForm.controls[i].valid)) { isCompleted = false; }
            }
            if (isCompleted) {
              const newCandidatesProfile: CandidateProfile = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                communityItems: []
              }
              this.facade.candidateProfileService.add(newCandidatesProfile)
                .subscribe(res => {
                  this.settings.getCandidatesProfile();
                  this.controlArray = [];
                  this.facade.toastrService.success('Candidate Profile was successfully created !');
                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  this.facade.errorHandlerService.showErrorMessage(err);
                })
            } else { modal.nzFooter[1].loading = false; }
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
    let editedCandidateProfile: CandidateProfile = this._detailedCandidateProfile.filter(CandidateProfile => CandidateProfile.id === id)[0];
    this.fillCandidateProfileForm(editedCandidateProfile);

    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Candidate Profile',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            modal.nzFooter[1].loading = true;
            let isCompleted = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if (!this.validateForm.controls[i].valid) { isCompleted = false; }
            }
            if (isCompleted) {
              editedCandidateProfile = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                communityItems: []
              }
              this.facade.candidateProfileService.update(id, editedCandidateProfile)
                .subscribe(res => {
                  this.settings.getCandidatesProfile();
                  this.facade.toastrService.success('Candidate was successfully edited !');
                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  this.facade.errorHandlerService.showErrorMessage(err);
                })
            } else { modal.nzFooter[1].loading = false; }
          }
        }],
    });
  }

  showDetailsModal(CandidateProfileID: number): void {
    this.emptyCandidateProfile = this._detailedCandidateProfile.find(CandidateProfile => CandidateProfile.id === CandidateProfileID);
    this.isDetailsVisible = true;
  }

  showDeleteConfirm(CandidateProfileID: number): void {
    const CandidateProfileDelete: CandidateProfile = this._detailedCandidateProfile.filter(c => c.id === CandidateProfileID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete ' + CandidateProfileDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.candidateProfileService.delete(CandidateProfileID)
        .subscribe(res => {
          this.settings.getCandidatesProfile();
          this.facade.toastrService.success('Candidate was deleted !');
        }, err => {
          this,this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  fillCandidateProfileForm(CandidateProfile: CandidateProfile) {
    this.validateForm.controls['name'].setValue(CandidateProfile.name);
    this.validateForm.controls['description'].setValue(CandidateProfile.description);
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
    if (index > colors.length) { index = parseInt((index / colors.length).toString().split(',')[0]); }
    return colors[index];
  }
}