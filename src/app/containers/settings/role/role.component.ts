import { Component, OnInit, TemplateRef, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from '@shared/models/employee.model';
import { Role } from '@shared/models/role.model';
import { FacadeService } from '@shared/services/facade.service';
import { RoleSandbox } from './role.sandbox';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent implements OnInit, OnDestroy {
  roles: Role[] = [];
  roleForm: FormGroup;
  employeesWithDeleteRole: Employee[] = [];
  getRoles: any;
  failedResult: boolean;
  errorMsg: any;
  getRoleFailed: any;
  getRoleErrorMsg: any;

  constructor(private fb: FormBuilder, private facade: FacadeService, private roleSandbox: RoleSandbox) { }

  ngOnInit() {
    this.roleSandbox.loadRoles();
    this.getRoles = this.roleSandbox.roles$.subscribe(roles => this.roles = roles);
    this.getRoleFailed = this.roleSandbox.roleFailed$.subscribe(failed => this.failedResult = failed);
    this.getRoleErrorMsg = this.roleSandbox.roleErrorMsg$.subscribe(msg => this.errorMsg = msg);

    this.roleForm = this.fb.group({
      name: [null, [Validators.required]],
      isActive: [null, [Validators.required]]
    });
  }

  showDeleteConfirm(role: Role) {
    this.getEmployeesWithDeleteRole(role);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete  ' + role.name + ' role?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        if (this.employeesWithDeleteRole.length > 0) {
          this.facade.toastrService.error('The are some employees with this associated role.');
        }
        else {
          this.roleSandbox.removeRole(role.id);
          setTimeout(() => {
            if (this.failedResult === true) {
              this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
            } else if (this.failedResult === false) {
              this.facade.toastrService.success('Role was successfully deleted!');
            }
            this.roleSandbox.resetFailed();
          }, 500);
        }
      }
    });
  }

  getEmployeesWithDeleteRole(role: Role) {
    this.facade.employeeService.get('GetAll')
      .subscribe(res => {
        this.employeesWithDeleteRole = res.filter(e => e.role.id === role.id);
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  showAddModal(modalContent: TemplateRef<{}>) {
    this.roleForm.reset();
    this.roleForm.controls.isActive.setValue('true');
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Add New Role',
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
            for (const role in this.roleForm.controls) {
              if (this.roleForm.controls[role]) {
                this.roleForm.controls[role].markAsDirty();
                this.roleForm.controls[role].updateValueAndValidity();
                if (!this.roleForm.controls[role].valid) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              const addRole: Role = {
                id: 0,
                name: this.roleForm.controls.name.value,
                isActive: this.roleForm.controls.isActive.value,
              };

              this.roleSandbox.addRole(addRole);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.toastrService.success('Role was successfully created!');
                  modal.destroy();
                }
                this.roleSandbox.resetFailed();
              }, 500);
            }
          }
        }

      ]
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, role: Role) {
    this.roleForm.reset();
    this.fillRoleForm(role);
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Edit Role',
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
            for (const i in this.roleForm.controls) {
              if (this.roleForm.controls[i]) {
                this.roleForm.controls[i].markAsDirty();
                this.roleForm.controls[i].updateValueAndValidity();
                if (!this.roleForm.controls[i].valid) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              const editRole = {
                id: role.id,
                name: this.roleForm.controls.name.value,
                isActive: this.roleForm.controls.isActive.value
              };

              this.roleSandbox.editRole(editRole);
              setTimeout(() => {
                if (this.failedResult === true) {
                  this.facade.errorHandlerService.showErrorMessage(this.errorMsg);
                } else if (this.failedResult === false) {
                  this.facade.toastrService.success('Role was successfully edited!');
                  modal.destroy();
                }
                this.roleSandbox.resetFailed();
              }, 500);
            }
          }
        }]
    });
  }

  fillRoleForm(role: Role) {
    this.roleForm.controls.name.setValue(role.name);
    this.roleForm.controls.isActive.setValue(role.isActive.toString());
  }

  ngOnDestroy() {
    this.getRoles.unsubscribe();
    this.getRoleFailed.unsubscribe();
    this.getRoleErrorMsg.unsubscribe();
  }
}

