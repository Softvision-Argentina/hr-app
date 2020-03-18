import { Component, OnInit, TemplateRef, OnDestroy } from '@angular/core';
import { FacadeService } from '../services/facade.service';
import { FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { Consultant } from '../../entities/consultant';
import { trimValidator } from '../directives/trim.validator';
import { Task } from 'src/entities/task';
import { TaskItem } from 'src/entities/taskItem';
import { AppConfig } from '../app-config/app.config';
import { dateValidator } from '../directives/date.validator';
import { AppComponent } from '../app.component';
import { User } from 'src/entities/user';
import { SearchbarService } from '../services/searchbar.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'tasks',
  templateUrl: 'tasks.component.html',
  styleUrls: ['tasks.component.css'],
  providers: [AppComponent]
})
export class TasksComponent implements OnInit, OnDestroy {

  showCloseIcon: boolean = false;
  searchTitle: string = "";
  consultants: Consultant[] = [];
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  loading: boolean = true;
  toDoList: Task[] = [];
  searchSub: Subscription;
  orderBy: string = "Order by";
  toDoListDisplay: any = [...this.toDoList];
  dummyTask: Task;
  showAllTasks: boolean = true;
  currentConsultant: Consultant;
  user: User;

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getConsultants();
    this.getTasks();
    this.resetForm();
    this.loading = false;
    this.app.hideLoading();
     this.searchSub = this.search.searchChanged.subscribe(data => {
      this.searchTitle = data;
    });
  }

  constructor(
    private search: SearchbarService,
    private facade: FacadeService,
    private fb: FormBuilder,
    private config: AppConfig,
    private app: AppComponent
  ) {
    this.user = JSON.parse(localStorage.getItem('currentUser'));
  }

  getConsultants() {
    this.facade.consultantService.get()
      .subscribe(res => {
        this.consultants = res;
        this.currentConsultant = res.filter(c => c.emailAddress.toLowerCase() == this.user.Email.toLowerCase())[0];
      }, err => {
        console.log(err);
      });
  }

  getTasks() {
    if (this.app.isUserRole(["HRManagement", "Admin"])) {
      console.log()
      this.facade.taskService.get()
        .subscribe(res => {
          this.toDoList = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));;
          this.toDoListDisplay = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));;
        }, err => {
          console.log(err);
        });
    }
    else{
      this.facade.taskService.getByConsultant(this.user.Email)
        .subscribe(res => {
          this.toDoList = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));;
          this.toDoListDisplay = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));;
        }, err => {
          console.log(err);
        });
    }
  }

  checkAll(id: number) {
    let updateTask = this.toDoList.find(this.findTaskIndex, id);
    let index = this.toDoList.indexOf(updateTask);
    let displayIndex = this.toDoListDisplay.indexOf(updateTask);

    this.app.showLoading();
    this.facade.taskService.approve(id).subscribe(res => {
      updateTask.taskItems.forEach(item => {
        item.checked = true;
      });
      updateTask.isApprove = true;
      updateTask.isNew = false;

      this.toDoList[index] = updateTask;
      this.toDoListDisplay[displayIndex] = updateTask;
      this.app.hideLoading();
    }, err => {
      this.app.hideLoading();
      this.facade.toastrService.error('An error has ocurred. Please try again later');
    });
  }

  deleteTask(id: number) {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete this task?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        let updateTask = this.toDoList.find(this.findTaskIndex, id);
        let index: number = this.toDoList.indexOf(updateTask);
        let displayIndex: number = this.toDoListDisplay.indexOf(updateTask);

        this.facade.taskService.delete(id)
          .subscribe(res => {
            if (index !== -1 && displayIndex !== -1) {
              this.toDoList.splice(index, 1);
            }
          }, err => {
            if (err.message != undefined) this.facade.toastrService.error(err.message);
            else this.facade.toastrService.error("The service is not available now. Try again later.");
          });
      }
    });
  }

  findTaskIndex(newItem) {
    return newItem.id === this;
  }

  getDeadlineDays() {
    return this.config.getConfig('taskDeadline');
  }

  changeStatus(id: number, item: TaskItem) {
    let isEmpty: boolean = true;
    let task = this.toDoList.find(this.findTaskIndex, id);
    let index = this.toDoList.indexOf(task);
    let taskItem = task.taskItems.filter(it => it.id == item.id)[0];
    let itemIndex = task.taskItems.indexOf(item);
    taskItem.checked = !taskItem.checked;
    this.facade.taskService.update(task.id, task)
      .subscribe(res => {
        this.toDoList[index].taskItems[itemIndex] = taskItem;
        this.toDoList[index].isNew = false;

        this.toDoList[index].taskItems.forEach(it => {
          if (!it.checked) isEmpty = false;
        });
        if (isEmpty) this.toDoList[index].isApprove = true;
        else this.toDoList[index].isApprove = false;
      }, err => {
        taskItem.checked = !taskItem.checked;
        if (err.message != undefined) this.facade.toastrService.error(err.message);
        else this.facade.toastrService.error("The service is not available now. Try again later.");
      });
  }

  addItem(id: number) {
    const input = <HTMLInputElement>document.querySelector('#newItem_' + id);
    if (input.value.trim() != '') {
      let updateTask = this.toDoList.find(this.findTaskIndex, id);
      let newId: number = updateTask.taskItems.length > 0 ? updateTask.taskItems[updateTask.taskItems.length - 1].id + 1 : 0;
      let newItem: TaskItem = {
        id: newId,
        text: input.value,
        checked: false,
        taskId: id,
        task: this.dummyTask
      }
      updateTask.taskItems.push(newItem);
      if (updateTask.isApprove) updateTask.isApprove = false;
      this.facade.taskService.update(updateTask.id, updateTask)
        .subscribe(res => {
          this.toDoList[this.toDoList.indexOf(updateTask)] = updateTask;
          this.toDoList[this.toDoList.indexOf(updateTask)].isNew = false;
          input.value = '';
        }, err => {
          if (err && err.errorCode == 900) {
            this.facade.toastrService.error(err.message);
          }
          else {
            this.facade.toastrService.error('An error has ocurred. Please try again later');
          }
          input.value = '';
          let itemIndex: number = updateTask.taskItems.indexOf(newItem);
          updateTask.taskItems.splice(itemIndex, 1);
        });
    }
    else {
      this.facade.toastrService.error('You must enter a valid text.');
    }
  }

  removeItem(id: number, item: TaskItem) {
    let updateTask: Task = this.toDoList.find(this.findTaskIndex, id);
    let taskIndex: number = this.toDoList.indexOf(updateTask);
    let itemIndex: number = updateTask.taskItems.indexOf(item);

    updateTask.taskItems.splice(itemIndex, 1);
    this.facade.taskService.update(updateTask.id, updateTask)
      .subscribe(res => {
        this.toDoList[taskIndex].isNew = false;

        this.toDoList[taskIndex] = updateTask;
        //If all items are checked, task is apprvoed
        if (this.toDoList[taskIndex].taskItems.every(it => it.checked))
          this.toDoList[taskIndex].isApprove = true;
      }, err => {
        if (err.message != undefined) this.facade.toastrService.error(err.message);
        else this.facade.toastrService.error("The service is not available now. Try again later.");
        updateTask.taskItems.splice(itemIndex, 0, item);
      });
  }

  showToDoList(option: string) {
    switch (option) {
      case 'ALL':
        if(this.showAllTasks) this.toDoListDisplay = this.toDoList;
        else this.toDoListDisplay = this.toDoList.filter(task => task.consultant.emailAddress.toLowerCase() === this.currentConsultant.emailAddress.toLowerCase());
        break;
      case 'OPEN':
        if(this.showAllTasks) this.toDoListDisplay = this.toDoList.filter(task => !task.isApprove);
        else this.toDoListDisplay = this.toDoList.filter(task => !task.isApprove && task.consultant.emailAddress.toLowerCase() === this.currentConsultant.emailAddress.toLowerCase());
        break;
      case 'CLOSED':
        if(this.showAllTasks) this.toDoListDisplay = this.toDoList.filter(task => task.isApprove);
        else this.toDoListDisplay = this.toDoList.filter(task => task.isApprove && task.consultant.emailAddress.toLowerCase() === this.currentConsultant.emailAddress.toLowerCase());
        break;
    }
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    //Add New Candidate Modal
    this.controlArray = [];
    this.resetForm();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Task',
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
            modal.nzFooter[1].loading = true;
            let isCompleted: boolean = true;
            let items: any[] = [];
            if (!this.app.isUserRole(["HRManagement", "Admin"]))
              this.validateForm.controls['consultant'].setValue(this.currentConsultant.id.toString());
            for (const i in this.validateForm.controls) {
              this.validateForm.controls[i].markAsDirty();
              this.validateForm.controls[i].updateValueAndValidity();
              console.log(this.validateForm.controls[i].value);
              console.log(this.validateForm.controls[i]);
              if (this.validateForm.controls[i].valid==false)
                isCompleted = false;
                console.log(isCompleted);
              if (i.includes('item'))
                items.push(this.validateForm.controls[i].value);
            }
            if (isCompleted) {
              let newId: number = this.toDoList.length > 0 ? this.toDoList[this.toDoList.length - 1].id + 1 : 0;
              let taskItems: TaskItem[] = [];
              let consultantID: number = this.validateForm.controls['consultant'].value;
              let newTask: Task = {
                id: newId,
                title: this.validateForm.controls['title'].value,
                isApprove: false,
                creationDate: new Date(),
                endDate: this.validateForm.controls['endDate'].value.toISOString(),
                consultantId: consultantID,
                consultant: this.consultants.filter(consultant => consultant.id == consultantID)[0],
                isNew: true,
                taskItems: taskItems
              }
              if (items.length > 0) {
                let i = 0;
                items.forEach(item => {
                  let newItem: TaskItem = {
                    id: i,
                    text: item,
                    checked: false,
                    taskId: newId,
                    task: this.dummyTask
                  };
                  newTask.taskItems.push(newItem);
                  i++;
                });
              }
              this.facade.taskService.add(newTask)
                .subscribe(res => {
                  console.log(res);
                  newTask.id = res.id;
                  this.toDoList.push(newTask);
                  console.log(newTask);
                  this.facade.toastrService.success('Task was successfully created !');
                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  if (err.message != undefined) this.facade.toastrService.error(err.message);
                  else this.facade.toastrService.error("The service is not available now. Try again later.");
                })
            }
            else modal.nzFooter[1].loading = false;
          }
        }]
    });
  }

  resetForm() {
    this.validateForm = this.fb.group({
      title: [null, [Validators.required, trimValidator]],
      endDate: [null, [Validators.required, dateValidator]],
      consultant: [null, [Validators.required, trimValidator]]
    });
  }

  addField(e?: MouseEvent): void {
    if (e) {
      e.preventDefault();
    }
    const id = this.controlArray.length > 0 ? this.controlArray[this.controlArray.length - 1].id + 1 : 0;

    const control = {
      id,
      controlInstance: `item${id}`
    };
    const index = this.controlArray.push(control);
    this.validateForm.addControl(
      this.controlArray[index - 1].controlInstance,
      new FormControl(null, Validators.required)
    );
  }

  shouldShowNewIcon(task: Task): boolean {
    if (task && task.isNew) {
      return true;
    }
    return false;
  }

  shouldShowDeadlineIcon(task: Task): boolean {
    if (task == undefined || task == null || task.endDate == undefined || task.endDate == null)
      return false;

    var currentDate = new Date();
    var deadLineDate = new Date();
    deadLineDate.setDate(currentDate.getDate() + this.getDeadlineDays());
    var formatted = new Date(task.endDate.toString());

    if (deadLineDate.toISOString() > formatted.toISOString()) {
      return true;
    }
    if (formatted.toISOString() < currentDate.toISOString()) {
      return true;
    }
    return false;
  }

  removeField(i: { id: number; controlInstance: string }, e: MouseEvent): void {
    e.preventDefault();
    if (this.controlArray.length > 0) {
      const index = this.controlArray.indexOf(i);
      this.controlArray.splice(index, 1);
      this.validateForm.removeControl(i.controlInstance);
    }
  }

  getFormControl(name: string): AbstractControl {
    return this.validateForm.controls[name];
  }

  mouseHovering(ev): void {
    var id = 'icon_' + ev.target.id.toString();
    document.getElementById(id).style.display = "block";
  }

  mouseLeft(ev): void {
    var id = 'icon_' + ev.target.id.toString();
    document.getElementById(id).style.display = "none";
  }

  canAssign(): boolean {
    if (this.currentConsultant && this.app.isUserRole(["HRManagement", "Admin"])) return true;
    else return false;
  }

  assignToMe() {
    if (this.currentConsultant) {
      this.validateForm.controls["consultant"].setValue(this.currentConsultant.id.toString());
    }
  }

  orderTasksBy(order: string) {
    switch (order) {
      case "urgent":
        this.orderBy = "Urgent";
        this.toDoListDisplay.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
        break;
      case "new":
        this.orderBy = "Newest";
        this.toDoListDisplay.sort((a, b) => (a.id > b.id ? 1 : -1));
        break;
      case "old":
        this.orderBy = "Oldest";
        this.toDoListDisplay.sort((a, b) => (a.id < b.id ? 1 : -1));
        break;
    }
  }

  getDaysLeft(endDate: Date) {
    let today: Date = new Date;
    let dueDate: Date = new Date(endDate);
    let dateDiff = (dueDate.getDate() - today.getDate());
    return dateDiff;
  }

  isCurrentUser(task: Task): boolean {
    if (task.consultant.emailAddress.toLowerCase() == this.currentConsultant.emailAddress.toLowerCase()) return true;
    else return false;
  }

  filterTasks() {
    if(!this.showAllTasks){
      this.toDoListDisplay = this.toDoListDisplay.filter(todo => todo.consultant.emailAddress.toLowerCase() === this.currentConsultant.emailAddress.toLowerCase());
    }
    else{
      this.toDoListDisplay = this.toDoList;
    }

  }

  ngOnDestroy() {
    this.searchSub.unsubscribe();
  }
}
