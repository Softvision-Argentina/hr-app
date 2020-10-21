import { Component, OnInit, TemplateRef, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SkillType } from '@shared/models/skill-type.model';
import { Skill } from '@shared/models/skill.model';
import { FacadeService } from '@shared/services/facade.service';
import { SkillsSandbox } from './skills.sandbox';

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss'],
})
export class SkillsComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown') nameDropdown;

  listOfTagOptions = [];
  skillTypes: SkillType[];
  filteredSkills: Skill[];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchSkills = [];
  listOfDisplayData;
  sortName = null;
  sortValue = null;

  skillForm: FormGroup;
  isDetailsVisible = false;
  isAddVisible = false;
  isAddOkLoading = false;
  emptySkill: Skill;
  skillTypeForDetail: string;
  getSkills: any;
  failedResult: any;
  errorMsg: any;
  getSkillsFailed: any;
  getSkillsErrorMsg: any;
  getSkillTypes: any;

  constructor(private facade: FacadeService, private formBuilder: FormBuilder, private skillsSandbox: SkillsSandbox) { }

  ngOnInit() {
    this.facade.appService.startLoading();
    this.facade.appService.removeBgImage();

    this.skillsSandbox.loadSkills();
    this.getSkills = this.skillsSandbox.skills$.subscribe(skills => { this.filteredSkills = skills; this.listOfDisplayData = skills; });
    this.getSkillsFailed = this.skillsSandbox.skillsFailed$.subscribe(failed => this.failedResult = failed);
    this.getSkillsErrorMsg = this.skillsSandbox.skillsErrorMsg$.subscribe(msg => this.errorMsg = msg);

    this.skillsSandbox.loadSkillTypes();
    this.getSkillTypes = this.skillsSandbox.skillTypes$.subscribe(skillTypes => this.skillTypes = skillTypes);

    this.skillForm = this.formBuilder.group({
      name: [null, Validators.required],
      description: [null, Validators.required],
      type: [null, [Validators.required]],
    });
    this.facade.appService.stopLoading();
  }

  getSkillTypeNameByID(id: number) {
    const skillType = this.skillTypes.find(s => s.id === id);
    return skillType ? skillType.name : '';
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }
  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchSkills.length ? this.listOfSearchSkills.some(skills => item.name.indexOf(skills) !== -1) : true) &&
        (item.name.toString().toUpperCase().indexOf(this.searchValue.toUpperCase()) !== -1);
    };
    const data = this.filteredSkills.filter(item => filterFunc(item));
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
    // Add New Skill Modal
    this.skillForm.reset();
    if (this.skillTypes.length > 0) {
      this.skillForm.controls['type'].setValue(this.skillTypes[0].id);
    }

    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Skill',
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
            for (const i in this.skillForm.controls) {
              if (this.skillForm.controls.hasOwnProperty(i)) {
                this.skillForm.controls[i].markAsDirty();
                this.skillForm.controls[i].updateValueAndValidity();
                if (!this.skillForm.controls[i].valid) {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {
              const newSkill: Skill = {
                id: 0,
                name: this.skillForm.controls['name'].value.toString(),
                description: this.skillForm.controls['description'].value.toString(),
                type: this.skillForm.controls['type'].value,
                candidateSkills: []
              };

              this.skillsSandbox.addSkill(newSkill);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.appService.stopLoading();
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.appService.stopLoading();
                  this.facade.toastrService.success('Skill was successfully created!');
                  modal.destroy();
                }
              }, 500);
            }
            this.facade.appService.stopLoading();
          }
        }],
    });
  }

  showDetailsModal(skillID: number): void {
    this.emptySkill = this.filteredSkills.find(skill => skill.id === skillID);
    this.skillTypeForDetail = this.getSkillTypeNameByID(this.emptySkill.type);
    this.isDetailsVisible = true;
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.skillForm.reset();
    let editedSkill: Skill = this.filteredSkills.filter(skill => skill.id === id)[0];
    this.skillForm.controls['name'].setValue(editedSkill.name);
    this.skillForm.controls['description'].setValue(editedSkill.description);
    this.skillForm.controls['type'].setValue(editedSkill.type);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Skill',
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
            for (const i in this.skillForm.controls) {
              if (this.skillForm.controls.hasOwnProperty(i)) {
                this.skillForm.controls[i].markAsDirty();
                this.skillForm.controls[i].updateValueAndValidity();
                if (!this.skillForm.controls[i].valid) {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {

              editedSkill = {
                id: editedSkill.id,
                name: this.skillForm.controls['name'].value.toString(),
                description: this.skillForm.controls['description'].value.toString(),
                type: this.skillForm.controls['type'].value,
                candidateSkills: []
              };

              this.skillsSandbox.editSkill(editedSkill);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.appService.stopLoading();
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.appService.stopLoading();
                  this.facade.toastrService.success('Skill was successfully edited!');
                  modal.destroy();
                }
              }, 500);
            }
            this.facade.appService.stopLoading();
          }
        }],
    });
  }

  showDeleteConfirm(skillID: number): void {
    const skillDelete: Skill = this.filteredSkills.find(skill => skill.id === skillID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + skillDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {

        this.skillsSandbox.removeSkill(skillID);
        setTimeout(() => {
          if (this.failedResult === true) {
            this.facade.appService.stopLoading();
            this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
          } else if (this.failedResult === false) {
            this.facade.toastrService.success('Skill was successfully deleted!');
          }
        }, 500);
      }
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptySkill = {
      id: 0,
      name: '',
      description: '',
      type: 0,
      candidateSkills: []
    };
  }

  ngOnDestroy() {
    this.getSkills.unsubscribe();
    this.getSkillsFailed.unsubscribe();
    this.getSkillsErrorMsg.unsubscribe();
    this.getSkillTypes.unsubscribe();
  }
}

