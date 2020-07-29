import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from '@shared/models/employee.model';
import { Role } from '@shared/models/role.model';
import { FacadeService } from '@shared/services/facade.service';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent implements OnInit {
  roles: Role[] = [];
  roleForm: FormGroup;
  employeesWithDeleteRole: Employee[] = [];

  constructor(private fb: FormBuilder, private facade: FacadeService) { }

  ngOnInit() {
    this.getRoles();

    this.roleForm = this.fb.group({
      name: [null, [Validators.required]],
      isActive: [null, [Validators.required]]
    });
  }

  getRoles() {
    this.facade.RoleService.get()
      .subscribe(res => {
        this.roles = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
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
          this.facade.toastrService.error("The are some employees with this associated role.");
        }
        else {
          this.facade.RoleService.delete(role.id).subscribe(res => {
            this.getRoles();
            this.facade.toastrService.success('Role was deleted !');
          }, err => {
            this.facade.errorHandlerService.showErrorMessage(err);
          })
        }
      }
    });
  }

  getEmployeesWithDeleteRole(role: Role) {
    this.facade.employeeService.get("GetAll")
      .subscribe(res => {
        this.employeesWithDeleteRole = res.filter(e => e.role.id === role.id);
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  showAddModal(modalContent: TemplateRef<{}>) {
    this.roleForm.reset();
    this.roleForm.controls["isActive"].setValue("true");
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Add New Role',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '50%',
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
            let isCompleted: boolean = true;
            for (let role in this.roleForm.controls) {
              this.roleForm.controls[role].markAsDirty();
              this.roleForm.controls[role].updateValueAndValidity();
              if (!this.roleForm.controls[role].valid) isCompleted = false;
            }
            if (isCompleted) {
              let addRole: Role = {
                id: 0,
                name: this.roleForm.controls["name"].value,
                isActive: this.roleForm.controls["isActive"].value,
              }
              this.facade.RoleService.add(addRole).subscribe(res => {
                this.getRoles();
                this.facade.toastrService.success('Role was successfully created !');
                modal.destroy();
              }, err => {
                this.facade.errorHandlerService.showErrorMessage(err);
              })
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
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Edit Role',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '60%',
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
            let isCompleted: boolean = true;
            for (const i in this.roleForm.controls) {
              this.roleForm.controls[i].markAsDirty();
              this.roleForm.controls[i].updateValueAndValidity();
              if (!this.roleForm.controls[i].valid) isCompleted = false;
            }
            if (isCompleted) {
              let editRole: Role = role;
              editRole.name = this.roleForm.controls["name"].value;
              editRole.isActive = this.roleForm.controls["isActive"].value;
              this.facade.RoleService.update(role.id, editRole).subscribe(res => {
                this.getRoles();
                this.facade.toastrService.success('Role was successfully edited !');
                modal.destroy();
              }, err => {
                this.facade.errorHandlerService.showErrorMessage(err);
              })
            }
          }
        }]
    });
  }

  fillRoleForm(role: Role) {
    this.roleForm.controls["name"].setValue(role.name);
    this.roleForm.controls["isActive"].setValue(role.isActive.toString());
  }

}