import { trimValidator } from './../directives/trim.validator';
import { Component, OnInit, ViewChild, TemplateRef} from '@angular/core';
import { Skill } from 'src/entities/skill';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SkillType } from 'src/entities/skillType';
import { FacadeService } from 'src/app/services/facade.service';
import { AppComponent } from '../app.component';
@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.css'],
  providers: [AppComponent]
})
export class SkillsComponent implements OnInit {

  @ViewChild('dropdown') nameDropdown;

  listOfTagOptions = [];
  skillTypes: SkillType[] = [];
  filteredSkills: Skill[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchSkills = [];
  listOfDisplayData = [...this.filteredSkills];
  sortName = null;
  sortValue = null;

  skillForm: FormGroup;
  isDetailsVisible = false;
  isAddVisible = false;
  isAddOkLoading = false;
  emptySkill: Skill;
  skillTypeForDetail: string;

  constructor(private facade: FacadeService, private formBuilder: FormBuilder, private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getSkills();
    this.getSkillTypes();

    this.skillForm = this.formBuilder.group({
      name: [null, [Validators.required, trimValidator]],
      description: [null, [Validators.required, trimValidator]],
      type: [null, [Validators.required]],
    });
    this.app.hideLoading();
  }

  getSkillTypeNameByID(id: number) {
      const skillType = this.skillTypes.find(s => s.id === id);
      return skillType ? skillType.name : '';
  }

  getSkillTypes() {
    this.facade.skillTypeService.get()
      .subscribe(res => {
        this.skillTypes = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getSkills() {
    this.facade.skillService.get()
      .subscribe(res => {
        this.filteredSkills = res;
        this.listOfDisplayData = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
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
    this.getSkillTypes();
    if (this.skillTypes.length > 0) {
      this.skillForm.controls['type'].setValue(this.skillTypes[0].id);
    }

    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Skill',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
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
            this.app.showLoading();
            modal.nzFooter[1].loading = true;
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
                type: this.skillForm.controls['type'].value.toString(),
                candidateSkills: []
              };

              this.facade.skillService.add(newSkill)
                .subscribe(() => {
                  this.getSkills();
                  this.app.hideLoading();
                  this.facade.toastrService.success('Skill was successfully created !');
                  modal.destroy();
                }, err => {
                  this.app.hideLoading();
                  modal.nzFooter[1].loading = false;
                  if (err.message) {
                    this.facade.toastrService.error(err.message);
                  } else {
                    this.facade.toastrService.error('The service is not available now. Try again later.');
                  }
                });
            } else {
              modal.nzFooter[1].loading = false;
              this.app.hideLoading();
            }
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
      nzWrapClassName: 'vertical-center-modal',
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
            this.app.showLoading();
            modal.nzFooter[1].loading = true;
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
                type: this.skillForm.controls['type'].value.toString(),
                candidateSkills: []
              };

              this.facade.skillService.update(id, editedSkill)
                .subscribe(() => {
                  this.getSkills();
                  this.app.hideLoading();
                  this.facade.toastrService.success('Skill was successfully created !');
                  modal.destroy();
                }, err => {
                  this.app.hideLoading();
                  modal.nzFooter[1].loading = false;
                  if (err.message) {
                    this.facade.toastrService.error(err.message);
                  } else {
                    this.facade.toastrService.error('The service is not available now. Try again later.');
                  }
                });
            } else {
              modal.nzFooter[1].loading = false;
            }

            this.app.hideLoading();
          }
        }],
    });
  }

  showDeleteConfirm(skillID: number): void {
    const skillDelete: Skill = this.filteredSkills.find(skill => skill.id === skillID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete ' + skillDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.skillService.delete(skillID)
        .subscribe(() => {
          this.getSkills();
          this.facade.toastrService.success('Skill was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
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
}
