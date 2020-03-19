import { Component, OnInit, TemplateRef, HostListener } from '@angular/core';
import { AppComponent } from '../app.component';
import { CompanyCalendar} from 'src/entities/Company-Calendar';
import { FacadeService } from '../services/facade.service';
import { trimValidator } from '../directives/trim.validator';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import { SettingsComponent } from '../settings/settings.component';
import * as  differenceInCalendarDays from 'date-fns/difference_in_calendar_days';

@Component({
  selector: 'app-company-calendar',
  templateUrl: './company-calendar.component.html',
  styleUrls: ['./company-calendar.component.css']
})
export class CompanyCalendarComponent implements OnInit {
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  listOfCompanyCalendar: CompanyCalendar[] = [];
  today = new Date();
  showCalendarSelected : boolean = false;

  constructor(private facade: FacadeService, 
              private fb: FormBuilder,
              private app: AppComponent,
              private settings: SettingsComponent) { }

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
    if (event.target == modal) {
      modal.style.display = 'none';
    }
  }

  getCompanyCalendar(){
    this.facade.companyCalendarService.get()
      .subscribe(res => {
        this.listOfCompanyCalendar = res.sort( (a,b) => {
          let d1 = new Date(a.date);
          let d2 = new Date(b.date);
          return d1>d2 ? -1 : d1<d2 ? 1 : 0;
        }  );
      }, err => {
        console.log(err);
      });
  }

  resetForm() {
    this.validateForm = this.fb.group({
        type: [null, [Validators.required]],
        date: [new Date(), [Validators.required]],
        comments: [null, [Validators.required, trimValidator]],
    });
  }

  hideCalendar(){
    this.showCalendarSelected = false;
  }

  disabledDate = (current: Date): boolean => {
    // Can not select days before today and today
    return differenceInCalendarDays(current, this.today) < 0;
  };


  showAddModal(modalContent: TemplateRef<{}>): void {
    this.controlArray = [];
    this.controlEditArray = [];
    this.resetForm();
  
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New festivity/reminder day',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
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
            modal.nzFooter[1].loading = true; 
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if ((!this.validateForm.controls[i].valid)) { isCompleted = false; }
            }
            if (isCompleted) {
              let newCompanyCalendar: CompanyCalendar = {
                id: 0,
                type: this.validateForm.controls['type'].value.toString(),
                date: this.validateForm.controls['date'].value.toISOString(),
                comments : this.validateForm.controls['comments'].value.toString()
              }
              this.facade.companyCalendarService.add(newCompanyCalendar)
                .subscribe(res => {
                  this.getCompanyCalendar();
                  this.controlArray = [];
                  this.facade.toastrService.success('festivity/reminder day was successfully created !');
                  this.getCompanyCalendar();
                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  if (err.message !== undefined) {
                    this.facade.toastrService.error(err.message);
                  } else {
                    this.facade.toastrService.error('The service is not available now. Try again later.');
                  }
                });
            }
            else { modal.nzFooter[1].loading = false; }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {    
    this.resetForm();
    this.controlArray = [];
    this.controlEditArray = [];    
    let editedCompanyCalendar: CompanyCalendar = this.listOfCompanyCalendar.filter(CompanyCalendar => CompanyCalendar.id == id)[0];
    this.fillCompanyCalendarForm(editedCompanyCalendar);

    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Company Calendar',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
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
            modal.nzFooter[1].loading = true;
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if (!this.validateForm.controls[i].valid) { isCompleted = false; }
            }
            let newDate;
            newDate = editedCompanyCalendar.date == this.validateForm.controls['date'].value ?
                      this.validateForm.controls['date'].value :
                      this.validateForm.controls['date'].value.toISOString();

            if (isCompleted) {
              editedCompanyCalendar = {
                id: 0,
                type: this.validateForm.controls['type'].value.toString(),
                date: newDate,
                comments : this.validateForm.controls['comments'].value.toString()
              }
              this.facade.companyCalendarService.update(id, editedCompanyCalendar)
                .subscribe(res => {
                  this.getCompanyCalendar();
                  this.facade.toastrService.success('festivity/reminder day was successfully edited !');
                  modal.destroy();
                  this.getCompanyCalendar();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  if (err.message !== undefined) {
                    this.facade.toastrService.error(err.message);
                  } else {
                    this.facade.toastrService.error('The service is not available now. Try again later.');
                  }
                })
            }
            else { modal.nzFooter[1].loading = false; }
          }
        }],
    });
  }

  showDeleteConfirm(CompanyCalendarID: number): void {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.companyCalendarService.delete(CompanyCalendarID)
        .subscribe(res => {
          this.getCompanyCalendar();
          this.facade.toastrService.success('festivity/reminder day was deleted !');
        }, err => {
          if (err.message !== undefined) {
            this.facade.toastrService.error(err.message);
          } else {
            this.facade.toastrService.error('The service is not available now. Try again later.');
          }
        })
    });
  }
  
  fillCompanyCalendarForm(CompanyCalendar: CompanyCalendar) {
    this.validateForm.controls['type'].setValue(CompanyCalendar.type);
    this.validateForm.controls['date'].setValue(CompanyCalendar.date);
    this.validateForm.controls['comments'].setValue(CompanyCalendar.comments);
  }
}