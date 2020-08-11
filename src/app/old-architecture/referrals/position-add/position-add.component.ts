import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Community } from '@shared/models/community.model';
import { OpenPosition } from '@shared/models/open-position.model';
import { FacadeService } from '@shared/services/facade.service';
import { Globals } from '@shared/utils/globals';
import { formFieldHasRequiredValidator } from '@shared/utils/utils.functions';
import { NzModalService } from 'ng-zorro-antd';

@Component({
  selector: 'app-position-add',
  templateUrl: './position-add.component.html',
  styleUrls: ['./position-add.component.scss']
})
export class PositionAddComponent implements OnInit {
  
  @Input() private _communities: Community[];
  public get communities(): Community[] {
    return this._communities;
  }
  public set communities(value: Community[]) {
    this._communities = value;
  }

  @Input() isEditPosition = false;
  @Input() positionToEdit: OpenPosition;

  seniorityList: any[];
  jobDescriptionContent: string = "";
  studios = [{name: 'Buenos Aires'}];
  positionForm: FormGroup = this.fb.group({
    title: [null, [Validators.required, Validators.maxLength(50)]],
    community: [null, [Validators.required]],
    seniority: [null, [Validators.required]],
    studio: ['',[Validators.required, Validators.maxLength(40)]],
    priority: [false],
    jobDescription: [null, [Validators.required, Validators.maxLength(1000)]]
  });

  constructor(private fb: FormBuilder, private globals: Globals, private facade: FacadeService,  private modalService: NzModalService) {
    this.seniorityList = globals.seniorityListForAddPosition;
  }

  ngOnInit() {
    if (this.communities) {
      this.communities = this.communities.sort((a, b) => (a.name.localeCompare(b.name)));
    }
    this.fillPositionForm(this.positionToEdit);
  }

  fillPositionForm(openPositionToEdit : OpenPosition){
    if(this.isEditPosition){
      this.positionForm.controls['title'].setValue(openPositionToEdit.title);
      this.positionForm.controls['studio'].setValue(openPositionToEdit.studio);
      this.positionForm.controls['seniority'].setValue(openPositionToEdit.seniority);
      this.positionForm.controls['community'].setValue(openPositionToEdit.community.id);
      this.jobDescriptionContent = openPositionToEdit.jobDescription;
      this.positionForm.controls['jobDescription'].setValue(openPositionToEdit.jobDescription);
      this.positionForm.controls['priority'].setValue(openPositionToEdit.priority);
    }
  }

  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.positionForm)
  }

  createNewPosition(action : string) {
    this.facade.appService.startLoading();
    let isCompleted = true;

    for (const i in this.positionForm.controls) {
      if (this.positionForm.controls[i]) {
        this.positionForm.controls[i].markAsDirty();
        this.positionForm.controls[i].updateValueAndValidity();
        if (!this.positionForm.controls[i].valid) { isCompleted = false; }
      }
    }

    if (isCompleted) {
      const newPosition: OpenPosition = {
        id: 0,
        title: this.positionForm.controls['title'].value.toString(),
        studio: this.positionForm.controls['studio'].value.toString(),
        community: new Community(this.positionForm.controls['community'].value),
        seniority: this.positionForm.controls['seniority'].value,
        jobDescription: this.positionForm.controls['jobDescription'].value,
        priority: this.positionForm.controls['priority'].value
      };

      if(action='new'){
        this.facade.openPositionService.add(newPosition)
        .subscribe(res => {
          this.facade.toastrService.success('Position was successfully created !');          
          this.modalService.closeAll();
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
          this.facade.appService.stopLoading();
        });
      }else{
        this.facade.openPositionService.update(this.positionToEdit.id, newPosition)
        .subscribe(res => {
          this.facade.toastrService.success('Position was successfully edited !');
          this.modalService.closeAll();
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
          this.facade.appService.stopLoading();
        });
      }
    }
    this.facade.appService.stopLoading();
  }

  clearDataAndCloseModal() {
    this.facade.modalService.openModals[0].destroy();
  }

  getJobDescriptionContent(content: string): void {
    this.positionForm.controls['jobDescription'].setValue(content);
  }
}