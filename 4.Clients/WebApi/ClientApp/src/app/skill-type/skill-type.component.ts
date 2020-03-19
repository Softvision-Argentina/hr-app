import { trimValidator } from './../directives/trim.validator';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { SkillType } from 'src/entities/skillType';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FacadeService } from 'src/app/services/facade.service';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-skill-type',
  templateUrl: './skill-type.component.html',
  styleUrls: ['./skill-type.component.css'],
  providers: [AppComponent]
})
export class SkillTypeComponent implements OnInit {

  @ViewChild('dropdown') nameDropdown;

  filteredSkillTypes: SkillType[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchSkillTypes = [];
  listOfDisplayData = [...this.filteredSkillTypes];
  sortName = null;
  sortValue = null;
  
  validateForm: FormGroup;
  isDetailsVisible: boolean = false;
  isAddVisible: boolean = false;
  isAddOkLoading: boolean = false;
  emptySkillType: SkillType;

  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getSkillTypes();

    this.validateForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]],
      description: [null, [Validators.required,trimValidator]],
    });
    this.app.hideLoading();
  }

  getSkillTypes(){
    this.facade.skillTypeService.get()
      .subscribe(res => {
        this.filteredSkillTypes = res;
        this.listOfDisplayData = res;
      }, err => {
        console.log(err);
      });
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchSkillTypes.length ? this.listOfSearchSkillTypes.some(skillType => item.name.indexOf(skillType) !== -1) : true) &&
        (item.name.toString().toUpperCase().indexOf(this.searchValue.toUpperCase()) !== -1);
    };
    const data = this.filteredSkillTypes.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.nameDropdown.nzVisible = false;
  }

  sort(sortName: string, value: boolean): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.validateForm.reset();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Skill type',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: [
        { label: 'Cancel', shape: 'default', onClick: () => modal.destroy() },
        { label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            this.app.showLoading();
            modal.nzFooter[1].loading = true;
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if ((!this.validateForm.controls[i].valid)) isCompleted = false;
            }
            if(isCompleted){
              let newSkillType: SkillType = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString()
              }
              this.facade.skillTypeService.add(newSkillType)
                      .subscribe(res => {
                        this.getSkillTypes();
                        this.app.hideLoading();
                        this.facade.toastrService.success("SkillType was successfuly created !");
                        modal.destroy();
                      }, err => {
                        this.app.hideLoading();
                        modal.nzFooter[1].loading = false;
                        if(err.message != undefined) this.facade.toastrService.error(err.message);
                        else this.facade.toastrService.error("The service is not available now. Try again later.");
                      })
            } 
            else modal.nzFooter[1].loading = false;
            this.app.hideLoading();
          }
        }],
    });
  }

  showDetailsModal(skillTypeID: number): void {
    this.emptySkillType = this.filteredSkillTypes.filter(skillType => skillType.id == skillTypeID)[0];
    this.isDetailsVisible = true;
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void{
    this.validateForm.reset();
    let editedSkillType: SkillType = this.filteredSkillTypes.filter(skillType => skillType.id == id)[0];
    this.validateForm.controls['name'].setValue(editedSkillType.name);
    this.validateForm.controls['description'].setValue(editedSkillType.description);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Skill type',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: [
        {  label: 'Cancel', shape: 'default', onClick: () => modal.destroy() },
        {
          label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            this.app.showLoading();
            modal.nzFooter[1].loading = true;
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if ((!this.validateForm.controls[i].valid)) isCompleted = false;
            }

            if(isCompleted){
              editedSkillType = {
                id: editedSkillType.id,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString()
              }
              this.facade.skillTypeService.update(editedSkillType.id, editedSkillType)
            .subscribe(res => {
              this.getSkillTypes();
              this.app.hideLoading();
              this.facade.toastrService.success('SkillType was successfully edited !');
              modal.destroy();
            }, err => {
              this.app.hideLoading();
              modal.nzFooter[1].loading = false;
              if(err.message != undefined) this.facade.toastrService.error(err.message);
              else this.facade.toastrService.error("The service is not available now. Try again later.");
            })
            } 
            else modal.nzFooter[1].loading = false;
            this.app.hideLoading();
          }
        }],
    });
  }

  showDeleteConfirm(skillTypeID: number): void {
    let skillTypeDelete: SkillType = this.filteredSkillTypes.find(skillType => skillType.id == skillTypeID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to delete ' + skillTypeDelete.name + ' ?',
      nzContent: 'This action will delete all skills associated with this type',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.skillTypeService.delete(skillTypeID)
        .subscribe(res => {
          this.getSkillTypes();
          this.facade.toastrService.success('SkillType was deleted !');
        }, err => {
          if(err.message != undefined) this.facade.toastrService.error(err.message);
          else this.facade.toastrService.error("The service is not available now. Try again later.");
        })
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptySkillType = { id: 0, name: '', description: '' };
  }
}