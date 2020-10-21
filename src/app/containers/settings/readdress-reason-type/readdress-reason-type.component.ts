import { Component, OnInit, TemplateRef, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
import { FacadeService } from '@shared/services/facade.service';
import { AppComponent } from '@app/app.component';
import { ReaddressReasonTypesSandbox } from './readdress-reason-type.sandbox';

@Component({
  selector: 'app-readdress-reason-type',
  templateUrl: './readdress-reason-type.component.html',
  styleUrls: ['./readdress-reason-type.component.scss'],
  providers: [AppComponent]
})
export class ReaddressReasonTypeComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown') nameDropdown;

  filteredReaddressReasonType: ReaddressReasonType[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchReaddressReasonType = [];
  listOfDisplayData = [...this.filteredReaddressReasonType];
  sortName = null;
  sortValue = null;

  validateForm: FormGroup;
  isDetailsVisible = false;
  isAddVisible = false;
  isAddOkLoading = false;
  emptyReaddressReasonType: ReaddressReasonType;
  getReaddressReasonTypes: any;
  failedResult: any;
  errorMsg: any;
  getReaddressReasonTypesFailed: any;
  getReaddressReasonTypesErrorMsg: any;

  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent, private readdressReasonTypesSandbox: ReaddressReasonTypesSandbox) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.readdressReasonTypesSandbox.loadReaddressReasonTypes();
    this.getReaddressReasonTypes = this.readdressReasonTypesSandbox.readdressReasonTypes$.subscribe(reasonTypes => {
      this.filteredReaddressReasonType = reasonTypes;
      this.listOfDisplayData = reasonTypes;
    });
    this.getReaddressReasonTypesFailed = this.readdressReasonTypesSandbox.readdressReasonTypesFailed$.subscribe(failed => this.failedResult = failed);
    this.getReaddressReasonTypesErrorMsg = this.readdressReasonTypesSandbox.readdressReasonTypesErrorMsg$.subscribe(msg => this.errorMsg = msg);

    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      description: [null, [Validators.required]],
    });
    this.app.hideLoading();
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
              const newReaddressReasonType: ReaddressReasonType = {
                name: this.validateForm.controls.name.value.toString(),
                description: this.validateForm.controls.description.value.toString(),
                id: null
              };

              this.readdressReasonTypesSandbox.addReaddressReasonType(newReaddressReasonType);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.toastrService.success('Reason category was successfuly created!');
                  modal.destroy();
                }
                this.readdressReasonTypesSandbox.resetFailed();
              }, 500);
            }
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
    const editedreaddressReasonTypeId = editedreaddressReasonType.id;
    this.validateForm.controls.description.setValue(editedreaddressReasonType.description);
    this.validateForm.controls.name.setValue(editedreaddressReasonType.name);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit reason category',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzFooter: [
        { label: 'Cancel', onClick: () => modal.destroy() },
        {
          label: 'Save', loading: false,
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
              editedreaddressReasonType = {
                name: this.validateForm.controls.name.value.toString(),
                description: this.validateForm.controls.description.value.toString(),
                id
              };

              this.readdressReasonTypesSandbox.editReaddressReasonType(editedreaddressReasonType);
              setTimeout(() => {
                if (this.failedResult === true) {
                  if (this.errorMsg[0]) {
                    this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                  } else {
                    this.facade.errorHandlerService.showErrorMessage(['The service is not available now. Try again later.']);
                  }
                } else if (this.failedResult === false) {
                  this.facade.toastrService.success('Reason category was successfuly edited!');
                  modal.destroy();
                }
                this.readdressReasonTypesSandbox.resetFailed();
              }, 500);
            }
            this.app.hideLoading();
          }
        }],
    });
  }

  showDeleteConfirm(readdressReasonTypeID: number): void {
    const readdressReasonTypeDelete: ReaddressReasonType = this.filteredReaddressReasonType.find(readdressReasonType => readdressReasonType.id === readdressReasonTypeID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + readdressReasonTypeDelete.name + '?',
      nzContent: 'This action will delete all reasons associated with this category',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.readdressReasonTypesSandbox.removeReaddressReasonType(readdressReasonTypeID);
        setTimeout(() => {
          if (this.failedResult === true) {
            if (this.errorMsg[0]) {
              this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
            } else {
              this.facade.errorHandlerService.showErrorMessage(['The service is not available now. Try again later.']);
            }
          } else if (this.failedResult === false) {
            this.facade.toastrService.success('Reason category was successfuly deleted!');
          }
          this.readdressReasonTypesSandbox.resetFailed();
        }, 500);
      }
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyReaddressReasonType = { id: 0, description: '', name: '' };
  }

  ngOnDestroy() {
    this.getReaddressReasonTypes.unsubscribe();
    this.getReaddressReasonTypesFailed.unsubscribe();
    this.getReaddressReasonTypesErrorMsg.unsubscribe();

  }
}
