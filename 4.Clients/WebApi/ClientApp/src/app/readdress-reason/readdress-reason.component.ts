import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FacadeService } from 'src/app/services/facade.service';
import { AppComponent } from '../app.component';
import { ReaddressReason } from 'src/entities/ReaddressReason';
import { ReaddressReasonType } from 'src/entities/ReaddressReasonType';


@Component({
  selector: 'app-readdress-reason',
  templateUrl: './readdress-reason.component.html',
  styleUrls: ['./readdress-reason.component.scss'],
  providers: [AppComponent]
})
export class ReaddressReasonComponent implements OnInit {

  @ViewChild('dropdown') nameDropdown;

  selectedType: number;

  filteredreaddressReason: ReaddressReason[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchreaddressReason = [];
  listOfDisplayData = [...this.filteredreaddressReason];
  sortName = null;
  sortValue = null;
  readdressReasonTypes: ReaddressReasonType[] = [];

  validateForm: FormGroup;
  isDetailsVisible: boolean = false;
  isAddVisible: boolean = false;
  isAddOkLoading: boolean = false;
  emptyReaddressReason: ReaddressReason;

  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getreaddressReason();
    this.getReaddressReasonTypes();

    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      description: [null, [Validators.required]],
      type: [null, [Validators.required]],
    });
    this.app.hideLoading();
  }

  getReaddressReasonTypes() {
    this.facade.readdressReasonTypeService.get("")
      .subscribe(res => {
        this.readdressReasonTypes = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getreaddressReason() {
    this.facade.readdressReasonService.get("")
      .subscribe(res => {
        this.filteredreaddressReason = res;
        this.listOfDisplayData = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  filterReaddressReason(name){
    this.listOfDisplayData = this.filteredreaddressReason.filter(function(e){return e.type == name});
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.validateForm.reset();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Reason',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: [
        { label: 'Cancel', onClick: () => modal.destroy() },
        {
          label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            this.app.showLoading();
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if ((!this.validateForm.controls[i].valid)) isCompleted = false;
            }
            if (isCompleted) {
              let newReaddressReason: ReaddressReason = {
                name: this.validateForm.controls['name'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                typeId: this.validateForm.controls['type'].value.toString(),
                id: null,
                type: null
              }
              this.facade.readdressReasonService.add(newReaddressReason)
                      .subscribe(res => {
                        this.getreaddressReason();
                        this.app.hideLoading();
                        this.facade.toastrService.success("Reason was successfuly created !");
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

  showDetailsModal(id: number): void {
    this.emptyReaddressReason = this.filteredreaddressReason.find(_  => _.id == id);
    this.isDetailsVisible = true;
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.validateForm.reset();
    let editedReaddressReason: ReaddressReason = this.filteredreaddressReason.find(_ => _.id === id);
    let readdressType: ReaddressReasonType = this.readdressReasonTypes.find(_ => _.name == editedReaddressReason.type);

    this.validateForm.controls['description'].setValue(editedReaddressReason.description);
    this.validateForm.controls['name'].setValue(editedReaddressReason.name);
    this.selectedType = readdressType.id;
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit reason',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: [
        { label: 'Cancel', onClick: () => modal.destroy() },
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
              editedReaddressReason = {
                id: null,
                typeId: this.validateForm.controls['type'].value.toString(),
                description: this.validateForm.controls['description'].value.toString(),
                type: null,
                name: this.validateForm.controls['name'].value.toString(),
              }
              this.facade.readdressReasonService.update(id, editedReaddressReason)
            .subscribe(res => {
              this.getreaddressReason();
              this.app.hideLoading();
              this.facade.toastrService.success('Reason was successfully edited!');
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

  showDeleteConfirm(ReaddressReasonID: number): void {
    let ReaddressReasonDelete: ReaddressReason = this.filteredreaddressReason.find(ReaddressReason => ReaddressReason.id === ReaddressReasonID);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + ReaddressReasonDelete.name + '?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.readdressReasonService.delete(ReaddressReasonID)
        .subscribe(res => {
          this.getreaddressReason();
          this.facade.toastrService.success('Reason was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyReaddressReason = { id: 0, description: "", typeId: 0, name: "", type: null};
  }
}
