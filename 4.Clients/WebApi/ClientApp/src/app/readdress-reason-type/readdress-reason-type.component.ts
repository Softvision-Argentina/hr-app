import { trimValidator } from './../directives/trim.validator';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FacadeService } from 'src/app/services/facade.service';
import { AppComponent } from '../app.component';

import { ReaddressReasonType } from 'src/entities/ReaddressReasonType';


@Component({
  selector: 'app-readdress-reason-type',
  templateUrl: './readdress-reason-type.component.html',
  styleUrls: ['./readdress-reason-type.component.css'],
  providers: [AppComponent]
})
export class ReaddressReasonTypeComponent implements OnInit {

  @ViewChild('dropdown') nameDropdown;

  filteredReaddressReasonType: ReaddressReasonType[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchReaddressReasonType = [];
  listOfDisplayData = [...this.filteredReaddressReasonType];
  sortName = null;
  sortValue = null;

  validateForm: FormGroup;
  isDetailsVisible: boolean = false;
  isAddVisible: boolean = false;
  isAddOkLoading: boolean = false;
  emptyReaddressReasonType: ReaddressReasonType;

  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getReaddressReasonTypes();

    this.validateForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]],
      description: [null, [Validators.required, trimValidator]],
    });
    this.app.hideLoading();
  }

  getReaddressReasonTypes() {
    this.facade.readdressReasonTypeService.get()
      .subscribe(res => {
        this.filteredReaddressReasonType = res;
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
      return (this.listOfSearchReaddressReasonType.length ? this.listOfSearchReaddressReasonType.some(readdressReasonType => item.name.indexOf(readdressReasonType) !== -1) : true) &&
        (item.name.toString().toUpperCase().indexOf(this.searchValue.toUpperCase()) !== -1);
    };
    const data = this.filteredReaddressReasonType.filter(item => filterFunc(item));
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
      nzTitle: 'Add New Reason Category',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: [
        { label: 'Cancel', shape: 'default', onClick: () => modal.destroy() },
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
            if (isCompleted) {
              let newReaddressReasonType: ReaddressReasonType = {
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                id: null
              }
              this.facade.readdressReasonTypeService.add(newReaddressReasonType)
                      .subscribe(res => {
                        this.getReaddressReasonTypes();
                        this.app.hideLoading();
                        this.facade.toastrService.success("Reason category was successfuly created !");
                        modal.destroy();
                      }, err => {
                        this.app.hideLoading();
                        modal.nzFooter[1].loading = false;
                        this.facade.errorHandlerService.showErrorMessage(err);
                      })
            } 
            else modal.nzFooter[1].loading = false;
            this.app.hideLoading();
          }
        }],
    });
  }

  showDetailsModal(readdressReasonTypeID: number): void {
    this.emptyReaddressReasonType = this.filteredReaddressReasonType.filter(readdressReasonType => readdressReasonType.id === readdressReasonTypeID)[0];
    this.isDetailsVisible = true;
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.validateForm.reset();
    let editedreaddressReasonType: ReaddressReasonType = this.filteredReaddressReasonType.filter(readdressReasonType => readdressReasonType.id === id)[0];
    let editedreaddressReasonTypeId = editedreaddressReasonType.id; 
    this.validateForm.controls['description'].setValue(editedreaddressReasonType.description);
    this.validateForm.controls['name'].setValue(editedreaddressReasonType.name);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit reason category',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: [
        { label: 'Cancel', shape: 'default', onClick: () => modal.destroy() },
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
            if (isCompleted) {
              editedreaddressReasonType = {
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                id:  null
              }
              this.facade.readdressReasonTypeService.update(id, editedreaddressReasonType)
            .subscribe(res => {
              this.getReaddressReasonTypes();
              this.app.hideLoading();
              this.facade.toastrService.success('Category was successfully edited!');
              modal.destroy();
            }, err => {
              this.app.hideLoading();
              modal.nzFooter[1].loading = false;
              this.facade.errorHandlerService.showErrorMessage(err);
            })
            } 
            else modal.nzFooter[1].loading = false;
            this.app.hideLoading();
          }
        }],
    });
  }

  showDeleteConfirm(readdressReasonTypeID: number): void {
    let readdressReasonTypeDelete: ReaddressReasonType = this.filteredReaddressReasonType.find(readdressReasonType => readdressReasonType.id === readdressReasonTypeID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + readdressReasonTypeDelete.name + '?',
      nzContent: 'This action will delete all reasons associated with this category',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.readdressReasonTypeService.delete(readdressReasonTypeID)
        .subscribe(res => {
          this.getReaddressReasonTypes();
          this.facade.toastrService.success('Category was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyReaddressReasonType = { id: 0, description: '', name: '' };
  }
}
