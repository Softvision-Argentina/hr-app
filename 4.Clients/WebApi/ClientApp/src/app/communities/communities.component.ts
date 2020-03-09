import { Component, OnInit, ViewChild, TemplateRef, Input,SimpleChanges } from '@angular/core';
import { Community } from 'src/entities/community';
import { FacadeService } from 'src/app/services/facade.service';
import { trimValidator } from '../directives/trim.validator';
import { FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { AppComponent } from '../app.component';
import { Consultant } from 'src/entities/consultant';
import { NzStatisticNumberComponent } from 'ng-zorro-antd/statistic/nz-statistic-number.component';
import { User } from 'src/entities/user';
import { CandidateProfile } from 'src/entities/Candidate-Profile';
import { SettingsComponent } from '../settings/settings.component';

@Component({
  selector: 'app-communities',
  templateUrl: './communities.component.html',
  styleUrls: ['./communities.component.css'],
})
export class CommunitiesComponent implements OnInit {

  @Input()
  private _detailedCommunity: Community[];
  public get detailedCommunity(): Community[]{
      return this._detailedCommunity;
  }
  public set detailedCommunity(value: Community[]) {
      this._detailedCommunity = value;
  }

  @Input()
  private _detailedCandidateProfile: CandidateProfile[];
  public get detailedCandidateProfile(): CandidateProfile[] {
      return this._detailedCandidateProfile;
  }
  public set detailedCandidateProfile(value: CandidateProfile[]) {
      this._detailedCandidateProfile = value;
  }

  currentConsultant: User;
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string[] }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  isEdit: boolean = false;
  editingCommunityId: number = 0;
  candidateprofiles: CandidateProfile[] = [];


  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent, private settings: SettingsComponent) {
      this.currentConsultant = JSON.parse(localStorage.getItem("currentUser"));
  }

  ngOnInit() {
    this.app.removeBgImage();
    this.resetForm();
    this.getCandidateProfiles();
  }
  
  ngOnChanges(changes: SimpleChanges){
    changes._detailedCandidateProfile;
    this.getCandidateProfiles();
  }

  
  getCProfileNameByID(id: number) {
    let CProfile = this.candidateprofiles.find(c => c.id === id);
    return CProfile != undefined ? CProfile.name : '';
  }

  getProfileById(id:number){
    let CProfile= this.candidateprofiles.find(c => c.id === id);
    return CProfile;
  }

  getCandidateProfiles(){
    this.facade.candidateProfileService.get()
    .subscribe(res => {
      this.candidateprofiles = res;
    }, err => {
      console.log(err);
    });
  }

  resetForm() { //crea el validateform(cuerpo de un Communitiesform)
    this.validateForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]], //name: new FormControl(value, validator or array of validators)
      description: [null, [Validators.required, trimValidator]],
      profileId: [null, [Validators.required, trimValidator]] //NO OLVIDAR ESTO
    });
  }

  
  showAddModal(modalContent: TemplateRef<{}>): void {
    //Add New Community Modal
    this.isEdit = false;
    this.controlArray = [];
    this.controlEditArray = [];
    this.resetForm();

    if(this.candidateprofiles.length > 0)
    this.validateForm.controls['profileId'].setValue(this.candidateprofiles[0].id);    
    
  
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Community', //Boton de agregar
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
      nzFooter: [
        {
          label: 'Cancel', //boton de cancelar
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save', //boton de guardar cambios
          type: 'primary',
          loading: false,
          onClick: () => {
            modal.nzFooter[1].loading = true; //el boton de guardar cambios cambia a true
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if ((!this.validateForm.controls[i].valid)) isCompleted = false;
            }
            if (isCompleted) {
              let newCommunity: Community = {
                id:0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                profileId: this.validateForm.controls['profileId'].value.toString(),
                profile: null
              }
              this.facade.communityService.add(newCommunity)
                .subscribe(res => {          
                  
                  //this.settings.getCommunities();        
                  this.settings.refresh();
                  this.controlArray = [];
                  this.facade.toastrService.success('Community was successfully created !');
                  
                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  if (err.message != undefined) this.facade.toastrService.error(err.message);
                  else this.facade.toastrService.error("The service is not available now. Try again later.");
                })
            }
            else modal.nzFooter[1].loading = false;
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    //Edit Consultant Modal
    this.resetForm();
    this.editingCommunityId = id; 
    this.isEdit = true;
    this.controlArray = [];
    this.controlEditArray = [];
    let editedCommunity: Community = this._detailedCommunity.filter(community => community.id == id)[0];
    
    this.fillCommunityForm(editedCommunity);

    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Community',
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
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if (!this.validateForm.controls[i].valid) isCompleted = false;
            }
            if (isCompleted) {
              editedCommunity = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                profileId: this.validateForm.controls['profileId'].value.toString(),
                profile: null 
              }
              this.facade.communityService.update(id, editedCommunity)
                .subscribe(res => {
                  this.settings.getCommunities();
                  this.facade.toastrService.success('Community was successfully edited !');
                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  if (err.message != undefined) this.facade.toastrService.error(err.message);
                  else this.facade.toastrService.error("The service is not available now. Try again later.");
                })
            }
            else modal.nzFooter[1].loading = false;
          }
        }],
    });
  }
  
  showDeleteConfirm(communityID: number): void {
  let communityDelete: Community = this._detailedCommunity.filter(c => c.id == communityID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete ' + communityDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.communityService.delete(communityID)
        .subscribe(res => {
          this.settings.getCommunities();
          this.facade.toastrService.success('Community was deleted !');
        }, err => {
          if (err.message != undefined) this.facade.toastrService.error(err.message);
          else this.facade.toastrService.error("The service is not available now. Try again later.");
        })
    });
  }
  
  fillCommunityForm(community: Community) {
    this.validateForm.controls['name'].setValue(community.name);
    this.validateForm.controls['description'].setValue(community.description);  
    if(this.candidateprofiles.length > 0)
    this.validateForm.controls['profileId'].setValue(this.candidateprofiles[0].id);    
  }

}
