import { Component, OnInit, TemplateRef, HostListener } from '@angular/core';
import { CompanyCalendar} from 'src/entities/Company-Calendar';
import { FacadeService } from '../services/facade.service';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import differenceInCalendarDays from 'date-fns/differenceInCalendarDays';

@Component({
  selector: 'app-company-calendar',
  templateUrl: './company-calendar.component.html',
  styleUrls: ['./company-calendar.component.scss']
})
export class CompanyCalendarComponent implements OnInit {
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  listOfCompanyCalendar: CompanyCalendar[] = [];
  today = new Date();
  showCalendarSelected = false;

  constructor(private facade: FacadeService, private fb: FormBuilder) { }

  ngOnInit() {
    this.getCompanyCalendar();
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

  getCompanyCalendar() {
    this.facade.companyCalendarService.get()
      .subscribe(res => {
        this.listOfCompanyCalendar = res.sort((a, b) => {
          let d1 = new Date(a.date);
          let d2 = new Date(b.date);
          return d1 > d2 ? -1 : d1 < d2 ? 1 : 0;
        });
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
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
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Add New festivity/reminder day',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
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
                type: this.validateForm.controls['type'].value.toString(),
                date: this.validateForm.controls['date'].value.toISOString(),
                comments: this.validateForm.controls['comments'].value.toString()
              }
              this.facade.companyCalendarService.add(newCompanyCalendar)
                .subscribe(() => {
                  this.getCompanyCalendar();
                  this.controlArray = [];
                  this.facade.toastrService.success('festivity/reminder day was successfully created !');
                  this.getCompanyCalendar();
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.resetForm();
    this.controlArray = [];
    this.controlEditArray = [];
    let editedCompanyCalendar: CompanyCalendar = this.listOfCompanyCalendar.filter(CompanyCalendar => CompanyCalendar.id === id)[0];
    this.fillCompanyCalendarForm(editedCompanyCalendar);

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Edit Company Calendar',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
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
            if (editedCompanyCalendar.date === this.validateForm.controls['date'].value) {
              newDate = this.validateForm.controls['date'].value;
            } else {
              newDate = this.validateForm.controls['date'].value.toISOString();
            }

            if (isCompleted) {
              editedCompanyCalendar = {
                id: 0,
                type: this.validateForm.controls['type'].value.toString(),
                date: new Date(newDate),
                comments: this.validateForm.controls['comments'].value.toString()
              };

              this.facade.companyCalendarService.update(id, editedCompanyCalendar)
                .subscribe(res => {
                  this.getCompanyCalendar();
                  this.facade.toastrService.success('festivity/reminder day was successfully edited !');
                  modal.destroy();
                  this.getCompanyCalendar();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            } else {
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
      nzOnOk: () => this.facade.companyCalendarService.delete(CompanyCalendarID)
        .subscribe(res => {
          this.getCompanyCalendar();
          this.facade.toastrService.success('festivity/reminder day was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  fillCompanyCalendarForm(CompanyCalendar: CompanyCalendar) {
    this.validateForm.controls['type'].setValue(CompanyCalendar.type);
    this.validateForm.controls['date'].setValue(CompanyCalendar.date);
    this.validateForm.controls['comments'].setValue(CompanyCalendar.comments);
  }
}
