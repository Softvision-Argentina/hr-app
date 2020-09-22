import { Component, OnInit, TemplateRef, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
import { ReaddressReason } from '@shared/models/readdress-reason.model';
import { FacadeService } from '@shared/services/facade.service';
import { AppComponent } from '@app/app.component';
import { ReaddressReasonSandbox } from './readdress-reason.sandbox';

@Component({
  selector: 'app-readdress-reason',
  templateUrl: './readdress-reason.component.html',
  styleUrls: ['./readdress-reason.component.scss'],
  providers: [AppComponent]
})
export class ReaddressReasonComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown') nameDropdown;

  selectedType: number;

  filteredreaddressReason: any;
  isLoadingResults = false;
  searchValue = '';
  listOfSearchreaddressReason = [];
  listOfDisplayData: any;
  sortName = null;
  sortValue = null;
  readdressReasonTypes: ReaddressReasonType[] = [];

  validateForm: FormGroup;
  isDetailsVisible = false;
  isAddVisible = false;
  isAddOkLoading = false;
  emptyReaddressReason: ReaddressReason;
  getReaddressReasons: any;
  failedResult: any;
  errorMsg: any;
  getReaddressReasonsFailed: any;
  getReaddressReasonsErrorMsg: any;
  getReaddressReasonTypes: any;

  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent, private readdressReasonSandbox: ReaddressReasonSandbox) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.readdressReasonSandbox.loadReaddressReasons();
    this.getReaddressReasons = this.readdressReasonSandbox.readdressReasons$.subscribe(readdressReasons => {
      this.filteredreaddressReason = readdressReasons;
      this.listOfDisplayData = readdressReasons;
    });
    this.getReaddressReasonsFailed = this.readdressReasonSandbox.readdressReasonsFailed$.subscribe(failed => this.failedResult = failed);
    this.getReaddressReasonsErrorMsg = this.readdressReasonSandbox.readdressReasonsErrorMsg$.subscribe(msg => this.errorMsg = msg);

    this.readdressReasonSandbox.loadReaddressReasonTypes();
    this.getReaddressReasonTypes = this.readdressReasonSandbox.readdressReasonTypes$.subscribe(reasonTypes => this.readdressReasonTypes = reasonTypes);

    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      description: [null, [Validators.required]],
      type: [null, [Validators.required]],
    });
    this.app.hideLoading();
  }

  filterReaddressReason(name) {
    if (name === 'ALL') {
      this.listOfDisplayData = this.filteredreaddressReason;
    } else {
      this.listOfDisplayData = this.filteredreaddressReason.filter((reason) => reason.type === name);
    }
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.validateForm.reset();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Reason',
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
              const readdressType = this.readdressReasonTypes.filter(type => type.id === this.validateForm.controls.type.value)[0];
              const newReaddressReason: any = {
                name: this.validateForm.controls.name.value.toString(),
                description: this.validateForm.controls.description.value.toString(),
                typeId: this.validateForm.controls.type.value.toString(),
                id: null,
                type: readdressType.name
              };

              this.readdressReasonSandbox.addReaddressReason(newReaddressReason);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.toastrService.success('Reason was successfuly created!');
                  modal.destroy();
                }
                this.readdressReasonSandbox.resetFailed();
              }, 500);
            }
            this.app.hideLoading();
          }
        }],
    });
  }

  showDetailsModal(id: number): void {
    this.emptyReaddressReason = this.filteredreaddressReason.find(_ => _.id === id);
    this.isDetailsVisible = true;
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.validateForm.reset();
    let editedReaddressReason: ReaddressReason = this.filteredreaddressReason.find(_ => _.id === id);
    const readdressType: ReaddressReasonType = this.readdressReasonTypes.find(_ => _.name === editedReaddressReason.type);
    this.validateForm.controls.description.setValue(editedReaddressReason.description);
    this.validateForm.controls.name.setValue(editedReaddressReason.name);
    this.selectedType = readdressType.id;
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit reason',
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
              editedReaddressReason = {
                id,
                typeId: this.validateForm.controls.type.value.toString(),
                description: this.validateForm.controls.description.value.toString(),
                type: editedReaddressReason.type,
                name: this.validateForm.controls.name.value.toString(),
              };

              this.readdressReasonSandbox.editReaddressReason(editedReaddressReason);
              setTimeout(() => {
                if (this.failedResult === true) {
                  if (this.errorMsg[0]) {
                    this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                  } else {
                    this.facade.errorHandlerService.showErrorMessage(['The service is not available now. Try again later.']);
                  }
                } else if (this.failedResult === false) {
                  this.facade.toastrService.success('Reason was successfuly edited!');
                  modal.destroy();
                }
                this.readdressReasonSandbox.resetFailed();
              }, 500);
            }
            this.app.hideLoading();
          }
        }],
    });
  }

  showDeleteConfirm(ReaddressReasonID: number): void {
    const ReaddressReasonDelete: ReaddressReason = this.filteredreaddressReason.find(readdressReason => readdressReason.id === ReaddressReasonID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + ReaddressReasonDelete.name + '?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.readdressReasonSandbox.removeReaddressReason(ReaddressReasonID);
        setTimeout(() => {
          if (this.failedResult === true) {
            if (this.errorMsg[0]) {
              this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
            } else {
              this.facade.errorHandlerService.showErrorMessage(['The service is not available now. Try again later.']);
            }
          } else if (this.failedResult === false) {
            this.facade.toastrService.success('Reason was successfuly deleted!');
          }
          this.readdressReasonSandbox.resetFailed();
        }, 500);
      }
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyReaddressReason = { id: 0, description: '', typeId: 0, name: '', type: null };
  }

  ngOnDestroy() {
    this.getReaddressReasons.unsubscribe();
    this.getReaddressReasonsFailed.unsubscribe();
    this.getReaddressReasonsErrorMsg.unsubscribe();
    this.getReaddressReasonTypes.unsubscribe();
  }
}

