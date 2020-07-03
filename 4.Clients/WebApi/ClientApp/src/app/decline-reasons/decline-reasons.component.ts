import { trimValidator } from './../directives/trim.validator';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { DeclineReason } from 'src/entities/declineReason';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FacadeService } from 'src/app/services/facade.service';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-decline-reasons',
  templateUrl: './decline-reasons.component.html',
  styleUrls: ['./decline-reasons.component.scss'],
  providers: [AppComponent]
})
export class DeclineReasonComponent implements OnInit {

  @ViewChild('dropdown') nameDropdown;

  filteredDeclineReasons: DeclineReason[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchDeclineReasons = [];
  listOfDisplayData = [...this.filteredDeclineReasons];
  sortName = null;
  sortValue = null;

  validateForm: FormGroup;
  isDetailsVisible: boolean = false;
  isAddVisible: boolean = false;
  isAddOkLoading: boolean = false;
  emptyDeclineReason: DeclineReason;

  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getDeclineReasons();

    this.validateForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]],
      description: [null, [Validators.required, trimValidator]],
    });
    this.app.hideLoading();
  }

  getDeclineReasons() {
    this.facade.declineReasonService.get("Named")
      .subscribe(res => {
        this.filteredDeclineReasons = res;
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
      return (this.listOfSearchDeclineReasons.length ? this.listOfSearchDeclineReasons.some(declineReason => item.name.indexOf(declineReason) !== -1) : true) &&
        (item.name.toString().toUpperCase().indexOf(this.searchValue.toUpperCase()) !== -1);
    };
    const data = this.filteredDeclineReasons.filter(item => filterFunc(item));
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
      nzTitle: 'Add New Decline reason',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal modal-custom',
      nzFooter: [
        {  label: 'Cancel', onClick: () => modal.destroy() },
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
              let newDeclineReason: DeclineReason = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString()
              }
              this.facade.declineReasonService.add(newDeclineReason)
                      .subscribe(res => {
                        this.getDeclineReasons();
                        this.app.hideLoading();
                        this.facade.toastrService.success("DeclineReason was successfuly created !");
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

  showDetailsModal(declineReasonID: number): void {
    this.emptyDeclineReason = this.filteredDeclineReasons.filter(declineReason => declineReason.id === declineReasonID)[0];
    this.isDetailsVisible = true;
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.validateForm.reset();
    let editedDeclineReason: DeclineReason = this.filteredDeclineReasons.filter(declineReason => declineReason.id === id)[0];
    this.validateForm.controls['name'].setValue(editedDeclineReason.name);
    this.validateForm.controls['description'].setValue(editedDeclineReason.description);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Decline reason',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal modal-custom',
      nzFooter: [
        {  label: 'Cancel', onClick: () => modal.destroy() },
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
              editedDeclineReason = {
                id: editedDeclineReason.id,
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString()
              }
              this.facade.declineReasonService.update(editedDeclineReason.id, editedDeclineReason)
            .subscribe(res => {
              this.getDeclineReasons();
              this.app.hideLoading();
              this.facade.toastrService.success('Decline reason was successfully edited!');
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

  showDeleteConfirm(declineReasonID: number): void {
    let declineReasonDelete: DeclineReason = this.filteredDeclineReasons.find(declineReason => declineReason.id === declineReasonID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + declineReasonDelete.name + '?',
      nzContent: 'This action will delete all skills associated with this type',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.declineReasonService.delete(declineReasonID)
        .subscribe(res => {
          this.getDeclineReasons();
          this.facade.toastrService.success('DeclineReason was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyDeclineReason = { id: 0, name: '', description: '' };
  }
}