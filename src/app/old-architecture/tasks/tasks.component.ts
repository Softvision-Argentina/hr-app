import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { TaskItem } from '@shared/models/task-item.model';
import { Task } from '@shared/models/task.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { SearchbarService } from '@shared/services/searchbar.service';
import { AppConfig } from '@shared/utils/app.config';
import { dateValidator } from '@shared/utils/date.validator';
import { Subscription } from 'rxjs';

@Component({
  selector: 'tasks',
  templateUrl: 'tasks.component.html',
  styleUrls: ['tasks.component.scss'],
})
export class TasksComponent implements OnInit, OnDestroy {

  showCloseIcon = false;
  searchTitle = '';
  users: User[] = [];
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  loading = true;
  toDoList: Task[] = [];
  searchSub: Subscription;
  orderBy = 'Order by';
  toDoListDisplay: any = [...this.toDoList];
  dummyTask: Task;
  showAllTasks = true;
  currentUser: User;
  user: User;
  tasksSubscription: Subscription = new Subscription();

  ngOnInit() {
    this.facade.appService.startLoading();
    this.facade.appService.removeBgImage();
    this.getUsers();
    this.getTasks();
    this.resetForm();
    this.loading = false;
    this.facade.appService.stopLoading();
    this.searchSub = this.search.searchChanged.subscribe(data => {
      this.searchTitle = data;
    });
    this.tasksSubscription.add(this.searchSub);
  }

  constructor(
    private search: SearchbarService,
    private facade: FacadeService,
    private fb: FormBuilder,
    private config: AppConfig
  ) {
    this.user = JSON.parse(localStorage.getItem('currentUser'));
  }

  getUsers() {
    this.facade.userService.get()
      .subscribe(res => {
        this.users = res.sort((a, b) => ((a.firstName + ' ' + a.lastName).localeCompare(b.firstName + ' ' + b.lastName)));
        this.currentUser = res.filter(c => this.isSameTextInLowerCase(c.username, this.user.username))[0];
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getTasks() {
    if (this.facade.appService.isUserRole(['HRManagement', 'Admin', 'Recruiter'])) {
      this.facade.taskService.get()
        .subscribe(res => {
          this.toDoList = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
          this.toDoListDisplay = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        });
    } else {
      this.facade.taskService.getByUser(this.user.username)
        .subscribe(res => {
          this.toDoList = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
          this.toDoListDisplay = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        });
    }
  }

  checkAll(id: number) {
    const updateTask = this.toDoList.find(this.findTaskIndex, id);
    const index = this.toDoList.indexOf(updateTask);
    const displayIndex = this.toDoListDisplay.indexOf(updateTask);

    this.facade.appService.startLoading();
    this.facade.taskService.approve(id).subscribe(res => {
      updateTask.taskItems.forEach(item => {
        item.checked = true;
      });
      updateTask.isApprove = true;
      updateTask.isNew = false;

      this.toDoList[index] = updateTask;
      this.toDoListDisplay[displayIndex] = updateTask;
      this.facade.appService.stopLoading();
    }, err => {
      this.facade.appService.stopLoading();
      this.facade.toastrService.error('An error has ocurred. Please try again later');
    });
  }

  deleteTask(id: number) {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete this task?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        const updateTask = this.toDoList.find(this.findTaskIndex, id);
        const index: number = this.toDoList.indexOf(updateTask);
        const displayIndex: number = this.toDoListDisplay.indexOf(updateTask);

        this.facade.taskService.delete(id)
          .subscribe(() => {
            if (index !== -1 && displayIndex !== -1) {
              this.toDoList.splice(index, 1);
            }
          }, err => {
            this.facade.errorHandlerService.showErrorMessage(err);
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
    let isEmpty = true;
    const task = this.toDoList.find(this.findTaskIndex, id);
    const index = this.toDoList.indexOf(task);
    const taskItem = task.taskItems.filter(it => it.id === item.id)[0];
    const itemIndex = task.taskItems.indexOf(item);
    taskItem.checked = !taskItem.checked;
    this.facade.taskService.update(task.id, task)
      .subscribe(() => {
        this.toDoList[index].taskItems[itemIndex] = taskItem;
        this.toDoList[index].isNew = false;

        this.toDoList[index].taskItems.forEach(it => {
          if (!it.checked) {
            isEmpty = false;
          }
        });

        if (isEmpty) {
          this.toDoList[index].isApprove = true;
        } else {
          this.toDoList[index].isApprove = false;
        }
      }, err => {
        taskItem.checked = !taskItem.checked;
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  addItem(id: number) {
    const input = <HTMLInputElement>document.querySelector('#newItem_' + id);
    if (input.value.trim() !== '') {
      const updateTask = this.toDoList.find(this.findTaskIndex, id);
      const newId: number = updateTask.taskItems.length > 0 ? updateTask.taskItems[updateTask.taskItems.length - 1].id + 1 : 0;
      const newItem: TaskItem = {
        id: newId,
        text: input.value,
        checked: false,
        taskId: id,
        task: this.dummyTask
      };

      updateTask.taskItems.push(newItem);

      if (updateTask.isApprove) {
        updateTask.isApprove = false;
      }

      this.facade.taskService.update(updateTask.id, updateTask)
        .subscribe(() => {
          this.toDoList[this.toDoList.indexOf(updateTask)] = updateTask;
          this.toDoList[this.toDoList.indexOf(updateTask)].isNew = false;
          input.value = '';
        }, err => {
          if (err && err.errorCode === 900) {
            this.facade.errorHandlerService.showErrorMessage(err);
          } else {
            this.facade.errorHandlerService.showErrorMessage(null, 'An error has ocurred. Please try again later');
          }
          input.value = '';
          const itemIndex: number = updateTask.taskItems.indexOf(newItem);
          updateTask.taskItems.splice(itemIndex, 1);
        });
    } else {
      this.facade.errorHandlerService.showErrorMessage(null, 'You must enter a valid text.');
    }
  }

  removeItem(id: number, item: TaskItem) {
    const updateTask: Task = this.toDoList.find(this.findTaskIndex, id);
    const taskIndex: number = this.toDoList.indexOf(updateTask);
    const itemIndex: number = updateTask.taskItems.indexOf(item);

    updateTask.taskItems.splice(itemIndex, 1);
    this.facade.taskService.update(updateTask.id, updateTask)
      .subscribe(() => {
        this.toDoList[taskIndex].isNew = false;

        this.toDoList[taskIndex] = updateTask;
        // If all items are checked, task is apprvoed
        if (this.toDoList[taskIndex].taskItems.every(it => it.checked)) {
          this.toDoList[taskIndex].isApprove = true;
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
        updateTask.taskItems.splice(itemIndex, 0, item);
      });
  }

  showToDoList(option: string) {
    const currentUserEmail = this.currentUser.username;

    switch (option) {
      case 'ALL':
        this.toDoListDisplay = this.toDoList;

        if (!this.showAllTasks) {
          this.toDoListDisplay = this.toDoList
            .filter(task => this.isSameTextInLowerCase(task.user.username, currentUserEmail));
        }

        break;

      case 'OPEN':
        this.toDoListDisplay = this.toDoList.filter(task => !task.isApprove);

        if (!this.showAllTasks) {
          this.toDoListDisplay = this.toDoListDisplay
            .filter(task => this.isSameTextInLowerCase(task.user.username, currentUserEmail));
        }

        break;

      case 'CLOSED':
        this.toDoListDisplay = this.toDoList.filter(task => task.isApprove);

        if (!this.showAllTasks) {
          this.toDoListDisplay = this.toDoListDisplay
            .filter(task => this.isSameTextInLowerCase(task.user.username, currentUserEmail));
        }

        break;
    }
  }

  isSameTextInLowerCase(textA: string, textB: string) {
    return textA.toLowerCase() === textB.toLowerCase();
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    // Add New Candidate Modal
    this.controlArray = [];
    this.resetForm();
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Add New Task',
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
            const items: any[] = [];

            if (!this.facade.appService.isUserRole(['HRManagement', 'Admin'])) {
              this.validateForm.controls['user'].setValue(this.currentUser.id.toString());
            }

            for (const i in this.validateForm.controls) {
              if (this.validateForm.controls.hasOwnProperty(i)) {
                this.validateForm.controls[i].markAsDirty();
                this.validateForm.controls[i].updateValueAndValidity();

                if (!this.validateForm.controls[i].valid) {
                  isCompleted = false;
                }

                if (i.includes('item')) {
                  items.push(this.validateForm.controls[i].value);
                }
              }
            }

            if (isCompleted) {
              const newId: number = this.toDoList.length > 0 ? this.toDoList[this.toDoList.length - 1].id + 1 : 0;
              const taskItems: TaskItem[] = [];
              const userID = this.validateForm.controls['user'].value;
              const newTask: Task = {
                id: newId,
                title: this.validateForm.controls['title'].value,
                isApprove: false,
                creationDate: new Date(),
                endDate: this.validateForm.controls['endDate'].value.toISOString(),
                userId: userID,
                user: this.users.filter(user => user.id === parseInt(userID, 10))[0],
                isNew: true,
                taskItems: taskItems
              };

              if (items.length > 0) {
                let i = 0;
                items.forEach(item => {
                  const newItem: TaskItem = {
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
                  newTask.id = res.id;
                  this.toDoList.push(newTask);
                  this.facade.toastrService.success('Task was successfully created !');
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            }
          }
        }]
    });
  }

  resetForm() {
    this.validateForm = this.fb.group({
      title: [null, Validators.required],
      endDate: [null, [Validators.required, dateValidator]],
      user: [null, Validators.required]
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
    if (!task || !task.endDate) {
      return false;
    }

    const currentDate = new Date();
    const deadLineDate = new Date();
    deadLineDate.setDate(currentDate.getDate() + this.getDeadlineDays());
    const formatted = new Date(task.endDate.toString());

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
    const id = 'icon_' + ev.target.id.toString();
    document.getElementById(id).style.display = 'block';
  }

  mouseLeft(ev): void {
    const id = 'icon_' + ev.target.id.toString();
    document.getElementById(id).style.display = 'none';
  }

  canAssign(): boolean {
    return this.currentUser && this.facade.appService.isUserRole(['HRManagement', 'Admin']);
  }

  assignToMe() {
    if (this.currentUser) {
      this.validateForm.controls['user'].setValue(this.currentUser.id.toString());
    }
  }

  orderTasksBy(order: string) {
    switch (order) {
      case 'urgent':
        this.orderBy = 'Urgent';
        this.toDoListDisplay.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
        break;
      case 'new':
        this.orderBy = 'Newest';
        this.toDoListDisplay.sort((a, b) => (a.id > b.id ? 1 : -1));
        break;
      case 'old':
        this.orderBy = 'Oldest';
        this.toDoListDisplay.sort((a, b) => (a.id < b.id ? 1 : -1));
        break;
    }
  }

  getDaysLeft(endDate: Date) {
    const today = Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
    const dueDate = Date.UTC(new Date(endDate).getFullYear(), new Date(endDate).getMonth(), new Date(endDate).getDate());
    var dateDiff = Math.floor((dueDate - today) / (1000 * 3600 * 24));
    return dateDiff;
  }

  isCurrentUser(task: Task): boolean {
    return task.user.username.toLowerCase() === this.currentUser.username.toLowerCase();
  }

  filterTasks() {
    if (!this.showAllTasks) {
      this.toDoListDisplay = this.toDoListDisplay
        .filter(todo => todo.user.username.toLowerCase() === this.currentUser.username.toLowerCase());
    } else {
      this.toDoListDisplay = this.toDoList;
    }

  }

  ngOnDestroy() {
    this.tasksSubscription.unsubscribe();
  }
}
