import { Component, HostListener, OnInit, TemplateRef, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CompanyCalendar } from '@shared/models/company-calendar.model';
import { FacadeService } from '@shared/services/facade.service';
import differenceInCalendarDays from 'date-fns/differenceInCalendarDays';
import { CompanyCalendarSandbox } from './company-calendar.sandbox';

@Component({
  selector: 'app-company-calendar',
  templateUrl: './company-calendar.component.html',
  styleUrls: ['./company-calendar.component.scss']
})
export class CompanyCalendarComponent implements OnInit, OnDestroy {
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  listOfCompanyCalendar: CompanyCalendar[] = [];
  today = new Date();
  showCalendarSelected = false;
  getCompanyCalendarEvents: any;
  failedResult: boolean;
  errorMsg: any;
  getCompanyCalendarFailed: any;
  getCompanyCalendarErrorMsg: any;

  constructor(private facade: FacadeService, private fb: FormBuilder, private companyCalendarSandbox: CompanyCalendarSandbox) { }

  ngOnInit() {
    this.companyCalendarSandbox.loadCompanyCalendarEvents();
    this.getCompanyCalendarEvents = this.companyCalendarSandbox.companyCalendarEvents$.subscribe(events => this.listOfCompanyCalendar = events);
    this.getCompanyCalendarFailed = this.companyCalendarSandbox.companyCalendarFailed$.subscribe(failed => this.failedResult = failed);
    this.getCompanyCalendarErrorMsg = this.companyCalendarSandbox.companyCalendarErrorMsg$.subscribe(msg => this.errorMsg = msg);
    this.resetForm();
    this.validateForm = this.fb.group({
      type: [null, [Validators.required]],
      date: [new Date(), [Validators.required]],
      comments: [null, [Validators.required]],
    });
  }

  showCalendarModal() {
    const modal = document.getElementById('myModalCalendar');
    modal.style.display = 'block';
  }

  closeCalendarModal() {
    const modal = document.getElementById('myModalCalendar');
    modal.style.display = 'none';
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const modal = document.getElementById('myModalCalendar');
    if (event.target === modal) {
      modal.style.display = 'none';
    }
  }

  resetForm() {
    this.validateForm = this.fb.group({
      type: [null, [Validators.required]],
      date: [new Date(), [Validators.required]],
      comments: [null, Validators.required],
    });
  }

  hideCalendar() {
    this.showCalendarSelected = false;
  }

  disabledDate = (current: Date): boolean => {
    // Can not select days before today and today
    return differenceInCalendarDays(current, this.today) < 0;
  }


  showAddModal(modalContent: TemplateRef<{}>): void {
    this.controlArray = [];
    this.controlEditArray = [];
    this.resetForm();

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Add New festivity/reminder day',
      nzContent: modalContent,
      nzClosable: true,
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
            let isCompleted = true;
            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls.hasOwnProperty(i)) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if ((!this.validateForm.controls[i].valid)) {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {
              const newCompanyCalendar: CompanyCalendar = {
                id: 0,
                type: this.validateForm.controls.type.value.toString(),
                date: this.validateForm.controls.date.value.toISOString(),
                comments: this.validateForm.controls.comments.value.toString()
              };
              this.companyCalendarSandbox.addCompanyCalendarEvent(newCompanyCalendar);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.controlArray = [];
                  this.facade.toastrService.success('festivity/reminder day was successfully created!');
                  modal.destroy();
                }
                this.companyCalendarSandbox.resetFailed();
              }, 500);
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.resetForm();
    this.controlArray = [];
    this.controlEditArray = [];
    let editedCompanyCalendar: CompanyCalendar = this.listOfCompanyCalendar.filter(companyCalendar => companyCalendar.id === id)[0];
    this.fillCompanyCalendarForm(editedCompanyCalendar);

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Edit Company Calendar',
      nzContent: modalContent,
      nzClosable: true,
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
            let newDate: string;
            if (editedCompanyCalendar.date === this.validateForm.controls.date.value) {
              newDate = this.validateForm.controls.date.value;
            } else {
              newDate = this.validateForm.controls.date.value.toISOString();
            }

            if (isCompleted) {
              editedCompanyCalendar = {
                id,
                type: this.validateForm.controls.type.value.toString(),
                date: new Date(newDate),
                comments: this.validateForm.controls.comments.value.toString()
              };

              this.companyCalendarSandbox.editCompanyCalendarEvent(editedCompanyCalendar);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.controlArray = [];
                  this.facade.toastrService.success('festivity/reminder day was successfully edited!');
                  modal.destroy();
                }
                this.companyCalendarSandbox.resetFailed();
              }, 500);
            }
          }
        }],
    });
  }

  showDeleteConfirm(CompanyCalendarID: number): void {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {

        this.companyCalendarSandbox.removeCompanyCalendarEvent(CompanyCalendarID);
        setTimeout(() => {
          if (this.failedResult === true) {
            this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
          } else if (this.failedResult === false) {
            this.controlArray = [];
            this.facade.toastrService.success('festivity/reminder day was successfully deleted!');
          }
          this.companyCalendarSandbox.resetFailed();
        }, 500);
      }
    });
  }

  fillCompanyCalendarForm(companyCalendar: CompanyCalendar) {
    this.validateForm.controls.type.setValue(companyCalendar.type);
    this.validateForm.controls.date.setValue(companyCalendar.date);
    this.validateForm.controls.comments.setValue(companyCalendar.comments);
  }

  ngOnDestroy() {
    this.getCompanyCalendarEvents.unsubscribe();
    this.getCompanyCalendarFailed.unsubscribe();
    this.getCompanyCalendarErrorMsg.unsubscribe();
  }
}

