import { Component, OnInit, TemplateRef, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DeclineReason } from '@shared/models/decline-reason.model';
import { FacadeService } from '@shared/services/facade.service';
import { AppComponent } from '@app/app.component';
import { DeclineReasonSandbox } from './decline-reasons.sandbox';

@Component({
  selector: 'app-decline-reasons',
  templateUrl: './decline-reasons.component.html',
  styleUrls: ['./decline-reasons.component.scss'],
  providers: [AppComponent]
})
export class DeclineReasonComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown') nameDropdown;

  filteredDeclineReasons: DeclineReason[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchDeclineReasons = [];
  listOfDisplayData = [...this.filteredDeclineReasons];
  sortName = null;
  sortValue = null;

  validateForm: FormGroup;
  isDetailsVisible = false;
  isAddVisible = false;
  isAddOkLoading = false;
  emptyDeclineReason: DeclineReason;
  getDeclineReasons: any;
  failedResult: boolean;
  errorMsg: any;
  getDeclineReasonFailed: any;
  getDeclineReasonErrorMsg: any;

  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent, private declineReasonSandbox: DeclineReasonSandbox) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.declineReasonSandbox.loadDeclineReasons();
    this.getDeclineReasons = this.declineReasonSandbox.declineReasons$.subscribe(reasons => {
      this.filteredDeclineReasons = reasons;
      this.listOfDisplayData = reasons;
    });
    this.getDeclineReasonFailed = this.declineReasonSandbox.declineReasonFailed$.subscribe(failed => this.failedResult = failed);
    this.getDeclineReasonErrorMsg = this.declineReasonSandbox.declineReasonErrorMsg$.subscribe(msg => this.errorMsg = msg);

    this.validateForm = this.fb.group({
      name: [null, Validators.required],
      description: [null, Validators.required],
    });
    this.app.hideLoading();
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
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzFooter: [
        { label: 'Cancel', onClick: () => modal.destroy() },
        {
          label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            this.app.showLoading();
            let isCompleted = true;
            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls[i]) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if ((!this.validateForm.controls[i].valid)) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              const newDeclineReason: DeclineReason = {
                id: 0,
                name: this.validateForm.controls.name.value.toString(),
                description: this.validateForm.controls.description.value.toString()
              };

              this.declineReasonSandbox.addDeclineReason(newDeclineReason);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.toastrService.success('DeclineReason was successfuly created!');
                  modal.destroy();
                }
                this.declineReasonSandbox.resetFailed();
              }, 500);
            }
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
    this.validateForm.controls.name.setValue(editedDeclineReason.name);
    this.validateForm.controls.description.setValue(editedDeclineReason.description);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Decline reason',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzFooter: [
        { label: 'Cancel', onClick: () => modal.destroy() },
        {
          label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            this.app.showLoading();
            let isCompleted = true;
            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls[i]) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if ((!this.validateForm.controls[i].valid)) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              editedDeclineReason = {
                id: editedDeclineReason.id,
                name: this.validateForm.controls.name.value.toString(),
                description: this.validateForm.controls.description.value.toString()
              };

              this.declineReasonSandbox.editDeclineReason(editedDeclineReason);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.toastrService.success('Decline reason was successfully edited!');
                  modal.destroy();
                }
                this.declineReasonSandbox.resetFailed();
              }, 500);
            }
            this.app.hideLoading();
          }
        }],
    });
  }

  showDeleteConfirm(declineReasonID: number): void {
    const declineReasonDelete: DeclineReason = this.filteredDeclineReasons.find(declineReason => declineReason.id === declineReasonID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + declineReasonDelete.name + '?',
      nzContent: 'This action will delete all skills associated with this type',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.declineReasonSandbox.removeDeclineReason(declineReasonID);
        setTimeout(() => {
          if (this.failedResult === true) {
            this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
          } else if (this.failedResult === false) {
            this.facade.toastrService.success('Decline reason was successfully deleted!');
          }
          this.declineReasonSandbox.resetFailed();
        }, 500);
      }
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyDeclineReason = { id: 0, name: '', description: '' };
  }

  ngOnDestroy() {
    this.getDeclineReasons.unsubscribe();
    this.getDeclineReasonFailed.unsubscribe();
    this.getDeclineReasonErrorMsg.unsubscribe();
  }
}

