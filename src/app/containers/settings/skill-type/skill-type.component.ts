import { Component, OnInit, TemplateRef, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SkillType } from '@shared/models/skill-type.model';
import { FacadeService } from '@shared/services/facade.service';
import { SkillTypesSandbox } from './skill-type.sandbox';

@Component({
  selector: 'app-skill-type',
  templateUrl: './skill-type.component.html',
  styleUrls: ['./skill-type.component.scss'],
})
export class SkillTypeComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown') nameDropdown;

  filteredSkillTypes: SkillType[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchSkillTypes = [];
  listOfDisplayData = [...this.filteredSkillTypes];
  sortName = null;
  sortValue = null;

  validateForm: FormGroup;
  isDetailsVisible = false;
  isAddVisible = false;
  isAddOkLoading = false;
  emptySkillType: SkillType;
  getSkillTypes: any;
  failedResult: any;
  errorMsg: any;
  getSkillTypesFailed: any;
  getSkillTypesErrorMsg: any;

  constructor(private facade: FacadeService, private fb: FormBuilder, private skillTypesSandbox: SkillTypesSandbox) { }

  ngOnInit() {
    this.facade.appService.startLoading();
    this.facade.appService.removeBgImage();
    this.skillTypesSandbox.loadSkillTypes();
    this.getSkillTypes = this.skillTypesSandbox.skillTypes$.subscribe(skillTypes => {
      this.filteredSkillTypes = skillTypes;
      this.listOfDisplayData = skillTypes;
    });
    this.getSkillTypesFailed = this.skillTypesSandbox.skillTypesFailed$.subscribe(failed => this.failedResult = failed);
    this.getSkillTypesErrorMsg = this.skillTypesSandbox.skillTypesFailed$.subscribe(failed => this.errorMsg = failed);

    this.validateForm = this.fb.group({
      name: [null, Validators.required],
      description: [null, [Validators.required]],
    });
    this.facade.appService.stopLoading();
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    // TODO check if it is working
    const filterFunc = (item) => {
      if (item.name.toString().toUpperCase().indexOf(this.searchValue.toUpperCase()) === -1) {
        return false;
      }

      if (this.listOfSearchSkillTypes.length) {
        return this.listOfSearchSkillTypes.some(skillType => item.name.indexOf(skillType) !== -1);
      }

      return true;
    };

    const data = this.filteredSkillTypes.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => {
      if (this.sortValue === 'ascend') {
        return a[this.sortName] > b[this.sortName] ? 1 : -1;
      } else {
        return b[this.sortName] > a[this.sortName] ? 1 : -1;
      }
    });
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
      nzWrapClassName: 'recru-modal recru-modal--sm',
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
            this.facade.appService.startLoading();
            let isCompleted = true;

            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls.hasOwnProperty(i)) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if (!this.validateForm.controls[i].valid) {
                  isCompleted = false;
                }
              }
            }

            if (isCompleted) {
              const newSkillType: SkillType = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString()
              };

              this.skillTypesSandbox.addSkillType(newSkillType);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.appService.stopLoading();
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.appService.stopLoading();
                  this.facade.toastrService.success('SkillType was successfuly created!');
                  modal.destroy();
                }
              }, 500);
            }
            this.facade.appService.stopLoading();
          }
        }],
    });
  }

  showDetailsModal(skillTypeID: number): void {
    this.emptySkillType = this.filteredSkillTypes.filter(skillType => skillType.id === skillTypeID)[0];
    this.isDetailsVisible = true;
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.validateForm.reset();
    let editedSkillType: SkillType = this.filteredSkillTypes.filter(skillType => skillType.id === id)[0];
    this.validateForm.controls['name'].setValue(editedSkillType.name);
    this.validateForm.controls['description'].setValue(editedSkillType.description);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Skill type',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'recru-modal recru-modal--sm',
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
            this.facade.appService.startLoading();
            let isCompleted = true;

            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls.hasOwnProperty(i)) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if (!this.validateForm.controls[i].valid) {
                  isCompleted = false;
                }
              }
            }

            if (isCompleted) {
              editedSkillType = {
                id: editedSkillType.id,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString()
              };

              this.skillTypesSandbox.editSkillType(editedSkillType);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.appService.stopLoading();
                  this.facade.toastrService.error('The service is not available now. Try again later.');
                } else if (this.failedResult === false) {
                  this.facade.appService.stopLoading();
                  this.facade.toastrService.success('SkillType was successfully edited!');
                  modal.destroy();
                }
              }, 500);
            }
            this.facade.appService.stopLoading();
          }
        }],
    });
  }

  showDeleteConfirm(skillTypeID: number): void {
    const skillTypeDelete: SkillType = this.filteredSkillTypes.find(skillType => skillType.id === skillTypeID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to delete ' + skillTypeDelete.name + ' ?',
      nzContent: 'This action will delete all skills associated with this type',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {

        this.skillTypesSandbox.removeSkillType(skillTypeID);
        setTimeout(() => {
          if (this.failedResult === true) {
            this.facade.appService.stopLoading();
            this.facade.toastrService.error('The service is not available now. Try again later.');
            this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
          } else if (this.failedResult === false) {
            this.facade.appService.stopLoading();
            this.facade.toastrService.success('SkillType was deleted!');
          }
        }, 500);
      }
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptySkillType = { id: 0, name: '', description: '' };
  }

  ngOnDestroy() {
    this.getSkillTypes.unsubscribe();
    this.getSkillTypesFailed.unsubscribe();
    this.getSkillTypesErrorMsg.unsubscribe();
  }
}
