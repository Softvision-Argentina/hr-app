import { Component, OnInit, ViewChild, TemplateRef, OnDestroy } from '@angular/core';
import { User } from 'src/entities/user';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { trimValidator } from '../directives/trim.validator';
import { FacadeService } from 'src/app/services/facade.service';
import { UserDetailsComponent } from './details/user-details.component';
import { AppComponent } from '../app.component';
import { replaceAccent } from 'src/app/helpers/string-helpers';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
  providers: [UserDetailsComponent, AppComponent]
})
export class UsersComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown') nameDropdown;

  filteredUsers: User[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchUsers = [];
  listOfDisplayData = [...this.filteredUsers];
  searchSub: Subscription;
  searchUser = '';
  sortName = 'name';
  sortValue = 'ascend';

  //Modals
  validateForm: FormGroup;
  isDetailsVisible: boolean = false;
  isAddVisible: boolean = false;
  isAddOkLoading: boolean = false;
  emptyUser: User;


  constructor(private facade: FacadeService, private fb: FormBuilder, private detailsModal: UserDetailsComponent,
    private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getUsers();
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

  getUsers(){
    this.facade.userService.get()
      .subscribe(res => {
        this.filteredUsers = res;
        this.listOfDisplayData = res.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getSearchInfo() {
    this.searchSub = this.facade.searchbarService.searchChanged.subscribe(data => {
      if (isNaN(Number(data))) {
        this.searchUser = data;
      } else {
        this.searchUser = '';
      }
    });
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchUsers.length ? this.listOfSearchUsers.some(users => item.name.indexOf(users) !== -1) : true) &&
        (replaceAccent(item.name.toString().toUpperCase() + item.lastName.toString().toUpperCase()).indexOf(replaceAccent(this.searchValue.toUpperCase())) !== -1);
    };
    const data = this.filteredUsers.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.nameDropdown.nzVisible = false;
  }

  sort(sortName: string, value: string): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    //Add New User Modal
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
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if ((!this.validateForm.controls[i].valid) &&
                (this.validateForm.controls[i] != this.validateForm.controls['phoneNumberPrefix'])) isCompleted = false;
            }
            if(isCompleted){
/*               let newUser: User = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                lastName: this.validateForm.controls['lastName'].value.toString(),

                emailAddress: this.validateForm.controls['email'].value.toString(),
                phoneNumber: '(' + this.validateForm.controls['phoneNumberPrefix'].value.toString() + ')' + this.validateForm.controls['phoneNumber'].value.toString(), 
                additionalInformation: this.validateForm.controls['additionalInformation'].value === null ? null : this.validateForm.controls['additionalInformation'].value.toString()
              } */
              let newUser: User = new User(0,this.validateForm.controls['name'].value.toString(),this.validateForm.controls['email'].value.toString())
              this.facade.userService.add(newUser)
            .subscribe(res => {
              this.getUsers();
              this.app.hideLoading();
              this.facade.toastrService.success("Interviewer successfully created !");
              modal.destroy();
            }, err => {
              this.app.hideLoading();
              modal.nzFooter[1].loading = false;
              this.facade.errorHandlerService.showErrorMessage(err);
            });
            } else {
                modal.nzFooter[1].loading = false;
            }

            this.app.hideLoading();
          }
        }],
    });
  }

  showDetailsModal(userID: number, modalContent: TemplateRef<{}>): void {
    this.emptyUser = this.filteredUsers.filter(user => user.id == userID)[0];
    this.detailsModal.showModal(modalContent, this.emptyUser.name);
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void{
    //Edit User Modal
    this.validateForm.reset();
    let editedUser: User = this.filteredUsers.filter(user => user.id == id)[0];
    this.validateForm.controls['name'].setValue(editedUser.name);
    //this.validateForm.controls['lastName'].setValue(editedUser.lastName);
    this.validateForm.controls['email'].setValue(editedUser.email);
    //this.validateForm.controls['phoneNumberPrefix'].setValue(editedUser.phoneNumber.substring(1, editedUser.phoneNumber.indexOf(')'))); 
    //this.validateForm.controls['phoneNumber'].setValue(editedUser.phoneNumber.split(')')[1]);
    //this.validateForm.controls['additionalInformation'].setValue(editedUser.additionalInformation);
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
            let isCompleted: boolean = true;
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              if ((!this.validateForm.controls[i].valid) &&
                (this.validateForm.controls[i] != this.validateForm.controls['phoneNumberPrefix'])) isCompleted = false;
            }
            if(isCompleted){
//              editedUser = {
//                id: editedUser.id,
//                name: this.validateForm.controls['name'].value.toString(),
//                lastName: this.validateForm.controls['lastName'].value.toString(),
//                emailAddress: this.validateForm.controls['email'].value.toString(),
//                phoneNumber: '(' + this.validateForm.controls['phoneNumberPrefix'].value.toString() + ')' + this.validateForm.controls['phoneNumber'].value.toString(), 
//                additionalInformation: this.validateForm.controls['additionalInformation'].value === null ? null : this.validateForm.controls['additionalInformation'].value.toString()
//              }
                editedUser = new User(editedUser.id,this.validateForm.controls['name'].value.toString(),this.validateForm.controls['name'].value.toString())
              this.facade.userService.update(editedUser.id, editedUser)
            .subscribe(res => {
              this.getUsers();
              this.app.hideLoading();
              this.facade.toastrService.success("Interviewer successfully edited.");
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

  showDeleteConfirm(userID: number): void {
    let userDelete: User = this.filteredUsers.filter(user => user.id == userID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + ', ' + userDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.userService.delete(userID)
        .subscribe(res => {
          this.getUsers();
          this.facade.toastrService.success('Interviewer was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyUser = new User(0,'','');
/*     this.emptyUser = {
      id: 0,
      name: '',
      lastName: '',
      additionalInformation: '',
      emailAddress: '',
      phoneNumber: ''
    };*/
  } 

  ngOnDestroy() {
    this.searchSub.unsubscribe();
  }
}