import { Component, OnInit, ViewChild, TemplateRef, OnDestroy } from '@angular/core';
import { Consultant } from 'src/entities/consultant';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { trimValidator } from '../directives/trim.validator';
import { FacadeService } from 'src/app/services/facade.service';
import { ConsultantDetailsComponent } from './details/consultant-details.component';
import { AppComponent } from '../app.component';
import { replaceAccent } from 'src/app/helpers/string-helpers';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-consultants',
  templateUrl: './consultants.component.html',
  styleUrls: ['./consultants.component.css'],
  providers: [ConsultantDetailsComponent, AppComponent]
})
export class ConsultantsComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown') nameDropdown;

  filteredConsultants: Consultant[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchConsultants = [];
  listOfDisplayData = [...this.filteredConsultants];
  searchSub: Subscription;
  searchConsultant = '';
  sortName = 'name';
  sortValue = 'ascend';

  // Modals
  validateForm: FormGroup;
  isDetailsVisible = false;
  isAddVisible = false;
  isAddOkLoading = false;

  emptyConsultant: Consultant;


  constructor(private facade: FacadeService, private fb: FormBuilder, private detailsModal: ConsultantDetailsComponent,
    private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getConsultants();
    this.getSearchInfo();

    this.validateForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]],
      lastName: [null, [Validators.required, trimValidator]],
      email: [null, [Validators.email, Validators.required]],
      phoneNumberPrefix: ['+54'],
      phoneNumber: [null, [Validators.required]],
      additionalInformation: [null, [trimValidator]]
    });

    this.app.hideLoading();
  }

  getConsultants() {
    this.facade.consultantService.get()
      .subscribe(res => {
        this.filteredConsultants = res;
        // tslint:disable-next-line: max-line-length
        this.listOfDisplayData = res.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getSearchInfo() {
    this.searchSub = this.facade.searchbarService.searchChanged.subscribe(data => {
      if (isNaN(Number(data))) {
        this.searchConsultant = data;
      } else {
        this.searchConsultant = '';
      }
    });
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item) => {
      // tslint:disable-next-line: max-line-length
      return (this.listOfSearchConsultants.length ? this.listOfSearchConsultants.some(consultants => item.name.indexOf(consultants) !== -1) : true) &&
        // tslint:disable-next-line: max-line-length
        (replaceAccent(item.name.toString().toUpperCase() + item.lastName.toString().toUpperCase()).indexOf(replaceAccent(this.searchValue.toUpperCase())) !== -1);
    };
    const data = this.filteredConsultants.filter(item => filterFunc(item));
    // tslint:disable-next-line: max-line-length
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.nameDropdown.nzVisible = false;
  }

  sort(sortName: string, value: string): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    // Add New Consultant Modal
    this.validateForm.reset();
    this.validateForm.controls['phoneNumberPrefix'].setValue('+54');
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Interviewer',
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
            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls[i]) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if ((!this.validateForm.controls[i].valid) &&
                  (this.validateForm.controls[i] !== this.validateForm.controls['phoneNumberPrefix'])) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              const newConsultant: Consultant = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                lastName: this.validateForm.controls['lastName'].value.toString(),
                emailAddress: this.validateForm.controls['email'].value.toString(),
                // tslint:disable-next-line: max-line-length
                phoneNumber: '(' + this.validateForm.controls['phoneNumberPrefix'].value.toString() + ')' + this.validateForm.controls['phoneNumber'].value.toString(),
                // tslint:disable-next-line: max-line-length
                additionalInformation: this.validateForm.controls['additionalInformation'].value === null ? null : this.validateForm.controls['additionalInformation'].value.toString()
              };
              this.facade.consultantService.add(newConsultant)
                .subscribe(res => {
                  this.getConsultants();
                  this.app.hideLoading();
                  this.facade.toastrService.success('Interviewer successfully created !');
                  modal.destroy();
                }, err => {
                  this.app.hideLoading();
                  modal.nzFooter[1].loading = false;
                  // tslint:disable-next-line: max-line-length
                    if (err.message !== undefined) { this.facade.toastrService.error(err.message); } else { this.facade.toastrService.error('The service is not available now. Try again later.'); }
                });
            } else { modal.nzFooter[1].loading = false; }
            this.app.hideLoading();
          }
        }],
    });
  }

  showDetailsModal(consultantID: number, modalContent: TemplateRef<{}>): void {
    this.emptyConsultant = this.filteredConsultants.filter(consultant => consultant.id === consultantID)[0];
    this.detailsModal.showModal(modalContent, this.emptyConsultant.name + ' ' + this.emptyConsultant.lastName);
  }


  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    // Edit Consultant Modal
    this.validateForm.reset();
    let editedConsultant: Consultant = this.filteredConsultants.filter(consultant => consultant.id === id)[0];
    this.validateForm.controls['name'].setValue(editedConsultant.name);
    this.validateForm.controls['lastName'].setValue(editedConsultant.lastName);
    this.validateForm.controls['email'].setValue(editedConsultant.emailAddress);
    // tslint:disable-next-line: max-line-length
    this.validateForm.controls['phoneNumberPrefix'].setValue(editedConsultant.phoneNumber.substring(1, editedConsultant.phoneNumber.indexOf(')')));
    this.validateForm.controls['phoneNumber'].setValue(editedConsultant.phoneNumber.split(')')[1]);
    this.validateForm.controls['additionalInformation'].setValue(editedConsultant.additionalInformation);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Interviewer',
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
            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls[i]) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if ((!this.validateForm.controls[i].valid) &&
                  (this.validateForm.controls[i] !== this.validateForm.controls['phoneNumberPrefix'])) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              editedConsultant = {
                id: editedConsultant.id,
                name: this.validateForm.controls['name'].value.toString(),
                lastName: this.validateForm.controls['lastName'].value.toString(),
                emailAddress: this.validateForm.controls['email'].value.toString(),
                // tslint:disable-next-line: max-line-length
                phoneNumber: '(' + this.validateForm.controls['phoneNumberPrefix'].value.toString() + ')' + this.validateForm.controls['phoneNumber'].value.toString(),
                // tslint:disable-next-line: max-line-length
                additionalInformation: this.validateForm.controls['additionalInformation'].value === null ? null : this.validateForm.controls['additionalInformation'].value.toString()
              };
              this.facade.consultantService.update(editedConsultant.id, editedConsultant)
            .subscribe(res => {
              this.getConsultants();
              this.app.hideLoading();
              this.facade.toastrService.success('Interviewer successfully edited.');
              modal.destroy();
            }, err => {
              this.app.hideLoading();
              modal.nzFooter[1].loading = false;
              this.facade.errorHandlerService.showErrorMessage(err);
            });
            } else { modal.nzFooter[1].loading = false; }
            this.app.hideLoading();
          }
        }],
    });
  }

  showDeleteConfirm(consultantID: number): void {
    const consultantDelete: Consultant = this.filteredConsultants.filter(consultant => consultant.id === consultantID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + consultantDelete.lastName + ', ' + consultantDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.consultantService.delete(consultantID)
        .subscribe(res => {
          this.getConsultants();
          this.facade.toastrService.success('Interviewer was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyConsultant = {
      id: 0,
      name: '',
      lastName: '',
      additionalInformation: '',
      emailAddress: '',
      phoneNumber: ''
    };
  }

  ngOnDestroy() {
    this.searchSub.unsubscribe();
  }
}
