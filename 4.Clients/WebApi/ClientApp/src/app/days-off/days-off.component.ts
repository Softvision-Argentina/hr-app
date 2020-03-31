import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { DaysOff } from 'src/entities/days-off';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { trimValidator } from '../directives/trim.validator';
import { dniValidator } from '../directives/dni.validator';
import { AppComponent } from '../app.component';
import { EmployeeService } from 'src/app/services/employee.service'
import { DaysOffService } from '../services/days-off.service';
import * as  differenceInCalendarDays from 'date-fns/difference_in_calendar_days';
import { User } from 'src/entities/user';
import { Globals } from '../app-globals/globals';
import { DaysOffStatusEnum } from '../../entities/enums/daysoff-status.enum';

@Component({
  selector: 'app-days-off',
  templateUrl: './days-off.component.html',
  styleUrls: ['./days-off.component.css']
})
export class DaysOffComponent implements OnInit {

  @ViewChild('dropdown') nameDropdown;

  validateForm: FormGroup;
  listOfDaysOff: DaysOff[] = [];
  employee;
  searchValue = '';
  searchValueType = '';
  searchValueStatus = '';
  listOfSearch = [];
  listOfDisplayData = [...this.listOfDaysOff];
  sortDni = null;
  sortValue = null;
  sortName = null;
  reasons: any[];
  showCalendarSelected: boolean = false;
  isHr: boolean;
  today = new Date();
  currentUser: User;
  statusList: any[];

  constructor(private facade: FacadeService,
    private fb: FormBuilder,
    private app: AppComponent,
    private daysOffService: DaysOffService,
    private employeeService: EmployeeService,
    private globals: Globals) {
    this.reasons = globals.daysOffTypeList;
    this.statusList = globals.daysOffStatusList;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.isHr = this.currentUser.role === 'Admin';
    this.employeeService.GetByEmail(this.currentUser.email)
      .subscribe(res => {
        this.employee = res.body;
        this.getDaysOff();
        this.resetForm();
      });
  }

  getDaysOff() {
    if (this.isHr) {
      this.facade.daysOffService.get()
        .subscribe(res => {
          this.listOfDaysOff = res;
          this.listOfDisplayData = res;
        }, err => {
          console.log(err);
        });
    } else {
      this.daysOffService.getByDNI(this.employee.dni)
        .subscribe(res => {
          this.listOfDaysOff = res.body;
          this.listOfDisplayData = res.body;
        });
    }
  }

  hideCalendar() {
    this.showCalendarSelected = false;
  }

  compareTwoDates(): boolean {
    if (new Date(this.validateForm.controls['endDate'].value) < new Date(this.validateForm.controls['date'].value)) {
      this.facade.toastrService.error('End Date must be before start date');
      return false;
    }
    return true;
  }

  range(start: number, end: number): number[] {
    const result: number[] = [];
    for (let i = start; i < end; i++) {
      result.push(i);
    }
    return result;
  }

  disabledDate = (current: Date): boolean => {
    // Can not select days before today and today
    return differenceInCalendarDays(current, this.today) < 0;
  };

  disabledDateTime = (): object => {
    return {
      nzDisabledHours: () => this.range(0, 24).splice(4, 20),
      nzDisabledMinutes: () => this.range(30, 60),
      nzDisabledSeconds: () => [55, 56]
    };
  };

  canAssign(): boolean {
    // if (this.currentConsultant && this.app.isUserRole(['HRManagement', 'Admin'])) return true;
    // else return false;
    return true;
  }

  filterTasks() {
    // if(!this.showAllTasks){
    //   this.toDoListDisplay = this.toDoListDisplay.filter(todo => todo.consultant.emailAddress.toLowerCase() === this.currentConsultant.emailAddress.toLowerCase());
    // }
    // else{
    //   this.toDoListDisplay = this.toDoList;
    // }

  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.resetForm();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add new day off',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        { label: 'Cancel', shape: 'default', onClick: () => modal.destroy() },
        {
          label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            if (this.compareTwoDates()) {
              this.app.showLoading();
              if (this.validateForm.controls.DNI.valid === false) {
                this.facade.toastrService.error('Please input a valid DNI.');
                this.app.hideLoading();
              }
              else {
                const dni: number = this.validateForm.controls.DNI.value === null || this.validateForm.controls.DNI.value === undefined ? 0
                  : this.validateForm.controls.DNI.value;
                this.employeeService.GetByDNI(dni)
                  .subscribe(res => {
                    this.app.hideLoading();
                    this.employee = res.body;
                    if (!this.employee || this.employee === null) {
                      this.facade.toastrService.error('There is no employee with that DNI.');
                    } else {
                      let isCompleted: boolean = true;
                      for (const i in this.validateForm.controls) {
                        this.validateForm.controls[i].markAsDirty();
                        this.validateForm.controls[i].updateValueAndValidity();
                        if ((this.validateForm.controls[i].status !== 'DISABLED' && !this.validateForm.controls[i].valid)) isCompleted = false;
                      }
                      let newStatus = this.isHr ? this.validateForm.controls['status'].value : DaysOffStatusEnum.InReview
                      if (isCompleted) {
                        let newDayOff: DaysOff = {
                          id: 0,
                          date: this.validateForm.controls['date'].value.toISOString(),
                          endDate: this.validateForm.controls['endDate'].value.toISOString(),
                          type: this.validateForm.controls['type'].value.toString(),
                          status: newStatus,
                          employeeId: this.employee.id,
                          employee: this.employee
                        };
                        this.facade.daysOffService.add(newDayOff)
                          .subscribe(res => {
                            this.app.hideLoading()
                            this.getDaysOff();
                            this.facade.toastrService.success('Day off was successfuly created !');
                            modal.destroy();
                          }, err => {
                            this.app.hideLoading();
                            // modal.nzFooter[1].loading = false;
                            if (err.message !== undefined) this.facade.toastrService.error(err.message);
                            else this.facade.toastrService.error('The service is not available now. Try again later.');
                          })
                      }
                      // else modal.nzFooter[1].loading = false;
                      // this.app.hideLoading();
                    }
                  })
              };
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    //Edit Consultant Modal
    this.resetForm();
    let editedDayOff: DaysOff = this.listOfDaysOff.filter(_ => _.id === id)[0];

    this.fillForm(editedDayOff);

    const modal = this.facade.modalService.create({
      nzTitle: 'Edit day off',
      nzContent: modalContent,
      nzClosable: true,
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
            // modal.nzFooter[1].loading = true;
            this.app.showLoading();
            this.employeeService.GetByDNI(this.validateForm.controls.DNI.value)
              .subscribe(res => {
                this.employee = res.body;
                this.app.hideLoading();
                if (!this.employee || this.employee === null) {
                  this.facade.toastrService.error('There is no employee with that DNI.');
                }
              })
            if (this.employee) {
              let isCompleted: boolean = true;
              for (const i in this.validateForm.controls) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();
                if ((this.validateForm.controls[i].status !== 'DISABLED' && !this.validateForm.controls[i].valid)) isCompleted = false;
              }

              let newDate; let newEndDate;
              newDate = editedDayOff.date === this.validateForm.controls['date'].value ? this.validateForm.controls['date'].value : new Date(this.validateForm.controls['date'].value).toISOString();
              newEndDate = editedDayOff.endDate === this.validateForm.controls.endDate.value ? this.validateForm.controls['endDate'].value : new Date(this.validateForm.controls['endDate'].value).toISOString();

              let newStatus = this.isHr ? this.validateForm.controls['status'].value : DaysOffStatusEnum.InReview;

              if (isCompleted) {
                editedDayOff = {
                  id: 0,
                  date: newDate,
                  endDate: newEndDate,
                  type: this.validateForm.controls['type'].value,
                  status: newStatus,
                  employeeId: this.employee.id,
                  employee: this.employee
                };
                this.facade.daysOffService.update(id, editedDayOff)
                  .subscribe(res => {
                    this.getDaysOff();
                    // this.app.hideLoading();
                    this.facade.toastrService.success('Day off was successfully edited !');
                    modal.destroy();
                  }, err => {
                    // this.app.hideLoading();
                    // modal.nzFooter[1].loading = false;
                    if (err.message !== undefined) { this.facade.toastrService.error(err.message); }
                    else { this.facade.toastrService.error('The service is not available now. Try again later.'); }
                  })
              }
              // else modal.nzFooter[1].loading = false;
              // this.app.hideLoading();
            }
          }
        }]
    });
  }

  showDeleteConfirm(dayOffId: number): void {
    let dayOff: DaysOff = this.listOfDaysOff.find(_ => _.id === dayOffId);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to delete ?',
      nzContent: 'This action will delete the day off',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.daysOffService.delete(dayOff.id)
        .subscribe(res => {
          this.getDaysOff();
          this.facade.toastrService.success('Day off was deleted !');
        }, err => {
          if (err.message !== undefined) this.facade.toastrService.error(err.message);
          else this.facade.toastrService.error('The service is not available now. Try again later.');
        })
    });
  }

  resetForm() {
    let dni = this.isHr ? null : this.employee.dni;

    this.validateForm = this.fb.group({
      DNI: [dni, [Validators.required, trimValidator, dniValidator]],
      type: [null, [Validators.required]],
      date: [new Date(), [Validators.required]],
      endDate: [new Date(), [Validators.required]],
      status: [DaysOffStatusEnum.InReview]
    });

    if (!this.isHr) {
      this.validateForm.controls['DNI'].disable();
    } else {
      this.validateForm.controls['DNI'].enable();
    }
  }

  acceptPetition(daysOff: DaysOff) {
    daysOff.status = DaysOffStatusEnum.Accepted;
    this.facade.daysOffService.update(daysOff.id, daysOff)
      .subscribe(res => {
        this.getDaysOff();
        // this.app.hideLoading();
        this.facade.toastrService.success('Petition was succesfully accepted !');
      }, err => {
        // this.app.hideLoading();
        // modal.nzFooter[1].loading = false;
        if (err.message !== undefined) this.facade.toastrService.error(err.message);
        else this.facade.toastrService.error('The service is not available now. Try again later.');
      })
  }

  fillForm(daysOff: DaysOff) {
    this.validateForm.controls['DNI'].setValue(daysOff.employee.dni);
    this.validateForm.controls['type'].setValue(daysOff.type);
    this.validateForm.controls['date'].setValue(daysOff.date);
    this.validateForm.controls['endDate'].setValue(daysOff.date);
    this.validateForm.controls['status'].setValue(daysOff.status);
  }


  reset(): void {
    this.searchValue = '';
    this.getDaysOff();
    this.search();
  }

  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearch.length ? this.listOfSearch.some(daysOff => item.employee.dni.indexOf(daysOff) !== -1) : true) &&
        (item.employee.dni.toString().indexOf(this.searchValue.trim()) !== -1); // trimvalidator
    };
    const data = this.listOfDaysOff.filter(item => filterFunc(item));
    this.listOfDaysOff = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortDni] > b[this.sortDni] ? 1 : -1) : (b[this.sortDni] > a[this.sortDni] ? 1 : -1));
    this.nameDropdown.nzVisible = false;
  }

  searchType(): void {
    const filterFunc = (item) => {
      return (this.listOfSearch.length ? this.listOfSearch.some(p => item.type === p) : true) &&
        (item.type === this.searchValueType)
    };
    const data = this.listOfDaysOff.filter(item => filterFunc(item));
    this.listOfDaysOff = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.searchValueType = '';
    this.nameDropdown.nzVisible = false;
  }

  resetType(): void {
    this.searchValueType = '';
    this.getDaysOff();
    this.searchType();
  }

  searchStatus(): void {
    const filterFunc = (item) => {
      return (this.listOfSearch.length ? this.listOfSearch.some(p => item.status === p) : true) &&
        (item.status === this.searchValueStatus)
    };
    const data = this.listOfDaysOff.filter(item => filterFunc(item));
    this.listOfDaysOff = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.searchValueStatus = '';
    this.nameDropdown.nzVisible = false;
  }

  resetStatus(): void {
    this.searchValueStatus = '';
    this.getDaysOff();
    this.searchStatus();
  }

  getStatus(status: number): string {
    return this.statusList.filter(st => st.id === status)[0].name;
  }

  getType(type: number): string {
    return this.reasons.filter(st => st.id === type)[0].name;
  }

  showAcceptButton(status: DaysOffStatusEnum) {
    return this.isHr && status === DaysOffStatusEnum.InReview;
  }
}
