import { Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DaysOffStatusEnum } from '@shared/enums/daysoff-status.enum';
import { DaysOff } from '@shared/models/days-off.model';
import { User } from '@shared/models/user.model';
import { DaysOffService } from '@shared/services/days-off.service';
import { EmployeeService } from '@shared/services/employee.service';
import { FacadeService } from '@shared/services/facade.service';
import { dniValidator } from '@shared/utils/dni.validator';
import { Globals } from '@shared/utils/globals';
import differenceInCalendarDays from "date-fns/differenceInCalendarDays";
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-days-off',
  templateUrl: './days-off.component.html',
  styleUrls: ['./days-off.component.scss']
})
export class DaysOffComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown') nameDropdown;

  validateForm: FormGroup = null;
  listOfDaysOff: DaysOff[] = [];
  employee: any = null;
  searchValue = '';
  searchValueType = '';
  searchValueStatus = '';
  listOfSearch: any[] = [];
  listOfDisplayData: DaysOff[] = [...this.listOfDaysOff];
  sortDni: any = null;
  sortValue: string = null;
  sortName: any = null;
  reasons: any[] = null;
  showCalendarSelected = false;
  isHr: boolean = null;
  today: Date = new Date();
  currentUser: User = null;
  statusList: any[] = [];
  searchSub: Subscription = null;
  searchDni = '';

  constructor(private facade: FacadeService,
    private fb: FormBuilder,
    private daysOffService: DaysOffService,
    private employeeService: EmployeeService,
    private globals: Globals) {
    this.reasons = globals.daysOffTypeList.sort((a, b) => (a.name).localeCompare(b.name));
    this.statusList = globals.daysOffStatusList;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.isHr = this.currentUser.role === 'Admin' || this.currentUser.role === 'Recruiter';
    this.employeeService.GetByEmail(this.currentUser.username)
      .subscribe(res => {
        this.employee = res.body;
        this.getDaysOff();
        this.resetForm();
      });
    this.getSearchInfo();
  }

  getDaysOff() {
    if (this.isHr) {
      this.facade.daysOffService.get()
        .subscribe(res => {
          this.listOfDaysOff = res;
          this.listOfDisplayData = res;
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        });
    } else {
      this.daysOffService.getByDNI(this.employee.dni)
        .subscribe(res => {
          this.listOfDaysOff = res.body;
          this.listOfDisplayData = res.body;
        });
    }
    this.facade.appService.stopLoading();
  }
  getSearchInfo() {
    this.searchSub = this.facade.searchbarService.searchChanged.subscribe(data => {
      this.searchDni = data;
    });
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
  }

  disabledDateTime = (): object => {
    return {
      nzDisabledHours: () => this.range(0, 24).splice(4, 20),
      nzDisabledMinutes: () => this.range(30, 60),
      nzDisabledSeconds: () => [55, 56]
    };
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.resetForm();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add new day off',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzFooter: [
        {  label: 'Cancel', onClick: () => modal.destroy() },
        {
          label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            if (this.compareTwoDates()) {
              this.facade.appService.startLoading();
              if (this.validateForm.controls.DNI.valid === false) {
                this.facade.toastrService.error('Please input a valid DNI.');
                this.facade.appService.stopLoading();
              } else {
                const dni: number = this.validateForm.controls.DNI.value === null || this.validateForm.controls.DNI.value === undefined ? 0
                  : this.validateForm.controls.DNI.value;
                this.employeeService.GetByDNI(dni)
                  .subscribe(res => {
                    this.facade.appService.stopLoading();
                    this.employee = res.body;
                    if (!this.employee || this.employee === null) {
                      this.facade.toastrService.error('There is no employee with that DNI.');
                    } else {
                      let isCompleted = true;
                      for (const i in this.validateForm.controls) {
                        if (this.validateForm.controls[i]) {
                          this.validateForm.controls[i].markAsDirty();
                          this.validateForm.controls[i].updateValueAndValidity();
                          if ((this.validateForm.controls[i].status !== 'DISABLED' && !this.validateForm.controls[i].valid)) { isCompleted = false; }
                        }
                      }
                      const newStatus = this.isHr ? this.validateForm.controls['status'].value : DaysOffStatusEnum.InReview;
                      if (isCompleted) {
                        const newDayOff: DaysOff = {
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
                            this.facade.appService.stopLoading();
                            this.getDaysOff();
                            this.facade.toastrService.success('Day off was successfuly created !');
                            modal.destroy();
                          }, err => {
                            this.facade.appService.stopLoading();
                            this.facade.errorHandlerService.showErrorMessage(err);
                          });
                      }
                    }
                  });
              }
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    // Edit User Modal
    this.resetForm();
    let editedDayOff: DaysOff = this.listOfDaysOff.filter(_ => _.id === id)[0];
    this.fillForm(editedDayOff);
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Edit day off',
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
            this.facade.appService.startLoading();
            this.employeeService.GetByDNI(this.validateForm.controls.DNI.value)
              .subscribe(res => {
                this.employee = res.body;
                this.facade.appService.stopLoading();
                if (!this.employee || this.employee == null) {
                  this.facade.toastrService.error('There is no employee with that DNI.');
                }
              });
            if (this.employee) {
              let isCompleted = true;
              for (const i in this.validateForm.controls) {
                if (this.validateForm.controls[i]) {
                  this.validateForm.controls[i].markAsDirty();
                  this.validateForm.controls[i].updateValueAndValidity();
                  if ((this.validateForm.controls[i].status !== 'DISABLED' && !this.validateForm.controls[i].valid)) { isCompleted = false; }
                }
              }

              let newDate; let newEndDate;
              newDate = editedDayOff.date === this.validateForm.controls['date'].value ? this.validateForm.controls['date'].value : new Date(this.validateForm.controls['date'].value).toISOString();
              newEndDate = editedDayOff.endDate === this.validateForm.controls.endDate.value ? this.validateForm.controls['endDate'].value : new Date(this.validateForm.controls['endDate'].value).toISOString();
              const newStatus = this.isHr ? this.validateForm.controls['status'].value : DaysOffStatusEnum.InReview;
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
                    this.facade.toastrService.success('Day off was successfully edited !');
                    modal.destroy();
                  }, err => {
                    this.facade.errorHandlerService.showErrorMessage(err);
                  });
              }
            }
          }
        }]
    });
  }

  showDeleteConfirm(dayOffId: number): void {
    const dayOff: DaysOff = this.listOfDaysOff.find(_ => _.id === dayOffId);
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
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  resetForm() {
    const dni = this.isHr ? null : this.employee.dni;

    this.validateForm = this.fb.group({
      DNI: [dni, [Validators.required, dniValidator]],
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
        this.facade.toastrService.success('Petition was succesfully accepted !');
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
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
        (item.type === this.searchValueType);
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
        (item.status === this.searchValueStatus);
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

  getStatus(daysOff: any): string {
    const statusFilter = this.statusList.filter(st => st.id === daysOff.status);
    if (statusFilter.length !== 0) {
      return statusFilter[0].name;
    }
  }

  getType(daysOff: any): string {
    const typeFilter = this.reasons.filter(st => st.id === daysOff.type);
    if (typeFilter.length !== 0) {
      return typeFilter[0].name;
    }
  }

  showAcceptButton(status: DaysOffStatusEnum) {
    return this.isHr && status === DaysOffStatusEnum.InReview;
  }

  ngOnDestroy() {
    this.searchSub.unsubscribe();
  }
}
