import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Consultant } from 'src/entities/consultant';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { trimValidator } from '../directives/trim.validator';
import { FacadeService } from 'src/app/services/facade.service';
import { AppComponent } from '../app.component';
import { Employee } from 'src/entities/employee';
import { User } from 'src/entities/user';
import { EmployeeDetailsComponent } from './details/employee-details.component';
import { Role } from 'src/entities/role';
import { EmployeeService } from '../services/employee.service';
import { Input } from '@angular/compiler/src/core';
import { EmployeeAux } from 'src/entities/employeeAux';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers: [AppComponent, EmployeeDetailsComponent]
})
export class EmployeesComponent implements OnInit {
  @ViewChild('dropdown') nameDropdown;
  editMode: boolean;
  employeeForm: FormGroup;
  newReviewerForm: FormGroup;
  employees: Employee[] = [];
  listOfDisplayData: Employee[] = [];
  searchValue = '';
  currentUser: User;
  consultants: Consultant[] = [];
  employee: Employee;
  editForm: FormGroup;
  activeRoles: Role[] = [];
  reviewers: Employee[] = [];
  detailedEmployee;
  editEmployee: Employee;
  statusList;
  filteredReviewersNames: string[] = [];
  filteredEditReviewersNames: any = [];
  reviewersFullNameAndId: any[] = [];
  employeesWithSelectedReviewer: Employee[] = [];
  showInputReviewerMessage: boolean;
  showReviewerNotFoundMessage: boolean;

  constructor(private facade: FacadeService, private fb: FormBuilder,
    private app: AppComponent, private detailsModal: EmployeeDetailsComponent) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.getEmployees();
    this.getConsultants();
    this.getRoles();
    this.editMode = false;
    this.statusList = [{ id: 0, name: 'Not Hired' }, { id: 1, name: 'Hired' }];
    this.showReviewerNotFoundMessage = false;

    this.employeeForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]],
      lastName: [null, [Validators.required, trimValidator]],
      dni: [null, Validators.required],
      phoneNumberPrefix: ['+54'],
      phoneNumber: [null, Validators.required],
      emailAddress: [null, [Validators.email, Validators.required]],
      linkedInProfile: [null, [trimValidator]],
      additionalInformation: [null, [trimValidator]],
      recruiterId: [null, [Validators.required]],
      status: [null],
      role: [null],
      roleId: [null, [Validators.required]],
      isReviewer: [null],
      reviewer: [null],
      reviewerName: [null],
    });

    this.newReviewerForm = this.fb.group({
      reviewerId: [null, Validators.required]
    });
  }

  getEmployees() {
    this.facade.employeeService.get('GetAll')
      .subscribe(res => {
        this.listOfDisplayData = res.filter(e => e.id !== 1);
        this.employees = res;
        this.reviewers = res.filter(e => e.isReviewer);
        this.filteredReviewersNames = this.reviewers.map(r => r.name + ' ' + r.lastName);
        this.reviewersFullNameAndId = [];
        for (const reviewer of this.reviewers) {
          const r: EmployeeAux = {
            id: reviewer.id,
            fullName: reviewer.name + ' ' + reviewer.lastName,
            reviewerId: null
          };
          if (reviewer.reviewer !== null) {
            r.reviewerId = reviewer.reviewer.id;
          }
          this.reviewersFullNameAndId.push(r);
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getConsultants() {
    this.facade.consultantService.get()
      .subscribe(res => {
        this.consultants = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getRoles() {
    this.facade.RoleService.get()
      .subscribe(res => {
        this.activeRoles = res.filter(role => role.isActive);
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  showDeleteConfirm(employeeID: number): void {
    const employeeDelete: Employee = this.employees.filter(employee => employee.id === employeeID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + employeeDelete.lastName + ', ' + employeeDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.employeeService.delete(employeeID)
        .subscribe(res => {
          this.getEmployees();
          this.facade.toastrService.success('Employee was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.editMode = false;
    this.employeeForm.reset();
    this.fillEmployeeForm();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Employee',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '70%',
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
            let isCompleted: boolean;
            isCompleted = this.validateEmployeeFields();
            if (isCompleted) {
              const newEmployee: Employee = {
                id: 0,
                dni: this.employeeForm.controls['dni'].value,
                name: this.employeeForm.controls['name'].value,
                lastName: this.employeeForm.controls['lastName'].value,
                emailAddress: this.employeeForm.controls['emailAddress'].value,

                phoneNumber: '(' + this.employeeForm.controls['phoneNumberPrefix'].value + ')' + this.employeeForm.controls['phoneNumber'].value,
                linkedInProfile: this.employeeForm.controls['linkedInProfile'].value,
                additionalInformation: this.employeeForm.controls['additionalInformation'].value,
                status: this.employeeForm.controls['status'].value,
                recruiterId: this.employeeForm.controls['recruiterId'].value,
                role: null,
                roleId: this.employeeForm.controls['roleId'].value,

                isReviewer: this.employeeForm.controls['isReviewer'].value === null ? false : this.employeeForm.controls['isReviewer'].value,
                reviewer: null,
                reviewerId: null
              };
              this.employeeForm.controls['reviewerName'].value === '' || this.employeeForm.controls['reviewerName'].value === null ?
                newEmployee.reviewerId = 1 :
                // tslint:disable-next-line: no-unused-expression
                this.reviewersFullNameAndId.find(r => r.fullName === this.employeeForm.controls['reviewerName'].value).id;
              this.facade.employeeService.add(newEmployee)
                .subscribe(res => {
                  this.getEmployees();
                  this.app.hideLoading();
                  this.facade.toastrService.success('Employee successfully created!');
                  modal.destroy();
                }, err => {
                  this.app.hideLoading();
                  modal.nzFooter[1].loading = false;
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            } else {
              modal.nzFooter[1].loading = false;
              this.app.hideLoading();
            }
          }
        }],
    });
  }

  showEditModal(editEmployee: Employee, modalContent: TemplateRef<{}>) {
    this.editMode = true;
    this.editEmployee = editEmployee;
    this.fillEditEmployeeForm(editEmployee);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit ' + editEmployee.name + ' ' + editEmployee.lastName,
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
            let isCompleted: boolean;
            isCompleted = this.validateEmployeeFields();
            if (isCompleted) {
              editEmployee.name = this.employeeForm.controls['name'].value;
              editEmployee.lastName = this.employeeForm.controls['lastName'].value;
              editEmployee.dni = this.employeeForm.controls['dni'].value;
              editEmployee.emailAddress = this.employeeForm.controls['emailAddress'].value;

              editEmployee.phoneNumber = '(' + this.employeeForm.controls['phoneNumberPrefix'].value + ')' + this.employeeForm.controls['phoneNumber'].value;
              editEmployee.linkedInProfile = this.employeeForm.controls['linkedInProfile'].value;
              editEmployee.status = this.employeeForm.controls['status'].value;
              editEmployee.roleId = this.employeeForm.controls['roleId'].value;
              editEmployee.recruiterId = this.employeeForm.controls['recruiterId'].value;
              editEmployee.isReviewer = this.employeeForm.controls['isReviewer'].value;

              this.employeeForm.controls['reviewerName'].value === '' ? editEmployee.reviewerId = 1 : editEmployee.reviewerId = this.reviewersFullNameAndId.find(r => r.fullName === this.employeeForm.controls['reviewerName'].value).id;
              editEmployee.additionalInformation = this.employeeForm.controls['additionalInformation'].value;

              this.facade.employeeService.Update(editEmployee)
                .subscribe(res => {
                  this.getEmployees();
                  modal.destroy();
                  this.facade.toastrService.success('Employee succesfully updated.');
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                  modal.nzFooter[1].loading = false;
                  this.app.hideLoading();
                });
            } else { modal.nzFooter[1].loading = false; }
            this.app.hideLoading();
          }
        }
      ]
    });
    this.app.hideLoading();
  }

  showDetailsModal(employeeId: number, modalContent: TemplateRef<{}>): void {
    this.facade.employeeService.get('GetById/' + employeeId)
      .subscribe(res => {
        this.detailedEmployee = res;
        this.detailsModal.showModal(modalContent, this.detailedEmployee.name + ' ' + this.detailedEmployee.lastName);
      });
  }

  fillEmployeeForm() {
    this.showInputReviewerMessage = false;
    this.showReviewerNotFoundMessage = false;
    this.employeeForm.controls['phoneNumberPrefix'].setValue('+54');
    this.employeeForm.controls['status'].setValue(1);
    const recruiterId = this.consultants.filter(c => c.emailAddress === this.currentUser.email)[0].id;
    this.employeeForm.controls['recruiterId'].setValue(recruiterId);
    if (this.activeRoles.length > 0) {
      this.employeeForm.controls['roleId'].setValue(this.activeRoles[0].id);
    }
  }

  fillEditEmployeeForm(employee: Employee) {
    this.showInputReviewerMessage = false;
    this.showReviewerNotFoundMessage = false;
    this.employeeForm.controls['name'].setValue(employee.name);
    this.employeeForm.controls['lastName'].setValue(employee.lastName);
    this.employeeForm.controls['dni'].setValue(employee.dni);
    this.employeeForm.controls['phoneNumberPrefix'].setValue(employee.phoneNumber.substring(1, employee.phoneNumber.indexOf(')')));
    this.employeeForm.controls['phoneNumber'].setValue(employee.phoneNumber.split(')')[1]);
    this.employeeForm.controls['emailAddress'].setValue(employee.emailAddress);
    this.employeeForm.controls['linkedInProfile'].setValue(employee.linkedInProfile);
    this.employeeForm.controls['additionalInformation'].setValue(employee.additionalInformation);
    this.employeeForm.controls['status'].setValue(employee.status);
    this.employeeForm.controls['recruiterId'].setValue(employee.recruiterId);
    this.employeeForm.controls['roleId'].setValue(employee.role.id);
    this.employeeForm.controls['recruiterId'].setValue(employee.recruiterId);
    this.employeeForm.controls['isReviewer'].setValue(employee.isReviewer);
    if (employee.reviewer.id === 1) {
      this.employeeForm.controls['reviewerName'].setValue('');
    } else {
      this.employeeForm.controls['reviewerName'].setValue(employee.reviewer.name + ' ' + employee.reviewer.lastName);
    }

    this.filteredEditReviewersNames = this.reviewersFullNameAndId.filter(r => r.id !== employee.id && r.reviewerId !== employee.id).map(r => r.fullName);
  }

  search(): void {
    const sValue = this.searchValue;
    function employeeFilter(employee: Employee) {
      return employee.name.toString().toUpperCase().indexOf(sValue.toUpperCase()) !== -1 ||
        employee.lastName.toString().toUpperCase().indexOf(sValue.toUpperCase()) !== -1;
    }
    const data = this.employees.filter(employeeFilter);
    this.listOfDisplayData = data.filter(e => e.id !== 1);
    this.searchValue = '';
    this.nameDropdown.nzVisible = false;
  }

  reset() {
    this.searchValue = '';
    this.search();
  }

  filterReviewers(event) {
    const filteredReviewer = event.target.value;

    this.filteredReviewersNames = this.reviewersFullNameAndId.filter(r => r.fullName.toLowerCase().indexOf(filteredReviewer.toLowerCase()) !== -1).map(r => r.fullName);
    this.showReviewerNotFoundMessage = false;
  }

  filterEditReviewers(event) {
    const filteredReviewer = event.target.value;

    this.filteredEditReviewersNames = this.reviewersFullNameAndId.filter(r => r.id !== this.editEmployee.id && r.reviewerId !== this.editEmployee.id && r.fullName.toLowerCase().indexOf(filteredReviewer.toLowerCase()) !== -1).map(r => r.fullName);
    this.showReviewerNotFoundMessage = false;
  }

  validateEmployeeFields(): boolean {
    let isCompleted = true;
    for (const i in this.employeeForm.controls) {
      if (this.employeeForm.controls[i]) {
        this.employeeForm.controls[i].markAsDirty();
        this.employeeForm.controls[i].updateValueAndValidity();
        if (!this.employeeForm.controls[i].valid) { isCompleted = false; }
      }
    }
    if (this.employeeForm.controls['reviewerName'].value === '' || this.employeeForm.controls['reviewerName'].value === undefined) {
      if (this.employeeForm.controls['isReviewer'].value === false || this.employeeForm.controls['isReviewer'].value === undefined) {
        isCompleted = false;
        this.showInputReviewerMessage = true;
        this.showReviewerNotFoundMessage = false;
      }
    } else {
      let existReviewer;
      if (this.editMode) {
        existReviewer = this.filteredEditReviewersNames.find(r => r === this.employeeForm.controls['reviewerName'].value);
      } else {
        existReviewer = this.filteredReviewersNames.find(r => r === this.employeeForm.controls['reviewerName'].value);
      }
      if (existReviewer === undefined) {
        isCompleted = false;
        this.showInputReviewerMessage = false;
        this.showReviewerNotFoundMessage = true;
      } else {
        isCompleted = true;
        this.showInputReviewerMessage = false;
        this.showReviewerNotFoundMessage = false;
      }
    }
    return isCompleted;
  }

  onIsReviewerChange(event, modalContent: TemplateRef<{}>) {
    const value: boolean = event.target.checked;
    this.showInputReviewerMessage = false;
    this.showReviewerNotFoundMessage = false;
    if (!value) {
      this.employeesWithSelectedReviewer = this.listOfDisplayData.filter(e => e.reviewer.id === this.editEmployee.id);
      if (this.employeesWithSelectedReviewer.length > 0) {
        const availableReviewers = this.reviewers.filter(r => r.id !== this.editEmployee.id);
        if (availableReviewers.length > 0) {
          this.showNewReviewerModal(modalContent);
        } else {
          this.showNoReviewersAvailablesModal();
        }
      }
    }
  }

  showNewReviewerModal(modalContent: TemplateRef<{}>) {
    const modal = this.facade.modalService.create({
      nzTitle: 'New Reviewer',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => {
            modal.destroy();
            this.employeeForm.controls['isReviewer'].setValue(true);
          }
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            let isCompleted = true;
            for (const i in this.newReviewerForm.controls) {
              if (this.newReviewerForm.controls[i]) {
                this.newReviewerForm.controls[i].markAsDirty();
                this.newReviewerForm.controls[i].updateValueAndValidity();
                if ((!this.newReviewerForm.controls[i].valid)) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              for (const editEmployee of this.employeesWithSelectedReviewer) {

                editEmployee.isReviewer ? editEmployee.reviewerId = 1 : editEmployee.reviewerId = this.newReviewerForm.controls['reviewerId'].value;
                editEmployee.roleId = editEmployee.role.id;
                this.facade.employeeService.Update(editEmployee)
                  .subscribe(res => {
                    modal.destroy();
                    this.facade.toastrService.success('Employee succesfully updated.');
                  }, err => {
                    this.facade.errorHandlerService.showErrorMessage(err);
                  });
              }
            }
          }
        }
      ]
    });
  }

  showNoReviewersAvailablesModal() {
    const modal = this.facade.modalService.create({
      nzTitle: 'New Reviewer',

      nzContent: 'There are no availables reviewers to reeplace ' + this.editEmployee.name + ' ' + this.editEmployee.lastName + ' as reviewer.',
      nzClosable: true,
      nzFooter: [
        {
          label: 'Ok',
          shape: 'default',
          onClick: () => {
            modal.destroy();
            this.employeeForm.controls['isReviewer'].setValue(true);
          }
        }
      ]
    });
  }
}
