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
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'tasks',
  templateUrl: 'tasks.component.html',
  styleUrls: ['tasks.component.css'],
  providers: [AppComponent]
})
export class TasksComponent implements OnInit, OnDestroy {

  showCloseIcon: boolean = false;
  searchTitle: string = '';
  consultants: Consultant[] = [];
  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  loading = true;
  toDoList: Task[] = [];
  searchSub: Subscription;
  orderBy = 'Order by';
  toDoListDisplay: any = [...this.toDoList];
  dummyTask: Task;
  showAllTasks = true;
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
        this.currentConsultant = res.filter(c => this.isSameTextInLowerCase(c.emailAddress, this.user.email))[0];
      }, err => {
        console.log(err);
      });
  }

  getTasks() {
    if (this.app.isUserRole(["HRManagement", "Admin", "Recruiter"])) {
      this.facade.taskService.get()
        .subscribe(res => {
          this.toDoList = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
          this.toDoListDisplay = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
        }, err => {
          console.log(err);
        });
    } else {
      this.facade.taskService.getByConsultant(this.user.email)
        .subscribe(res => {
          this.toDoList = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
          this.toDoListDisplay = res.sort((a, b) => (a.endDate < b.endDate ? 1 : -1));
        }, err => {
          console.log(err);
        });
    }
  }

  checkAll(id: number) {
    const updateTask = this.toDoList.find(this.findTaskIndex, id);
    const index = this.toDoList.indexOf(updateTask);
    const displayIndex = this.toDoListDisplay.indexOf(updateTask);

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
        const updateTask = this.toDoList.find(this.findTaskIndex, id);
        const index: number = this.toDoList.indexOf(updateTask);
        const displayIndex: number = this.toDoListDisplay.indexOf(updateTask);

        this.facade.taskService.delete(id)
          .subscribe(() => {
            if (index !== -1 && displayIndex !== -1) {
              this.toDoList.splice(index, 1);
            }
          }, err => {
            if (err.message) {
              this.facade.toastrService.error(err.message);
            } else {
              this.facade.toastrService.error('The service is not available now. Try again later.');
            }
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
        if (err.message) {
          this.facade.toastrService.error(err.message);
        } else {
          this.facade.toastrService.error('The service is not available now. Try again later.');
        }
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
            this.facade.toastrService.error(err.message);
          } else {
            this.facade.toastrService.error('An error has ocurred. Please try again later');
          }
          input.value = '';
          const itemIndex: number = updateTask.taskItems.indexOf(newItem);
          updateTask.taskItems.splice(itemIndex, 1);
        });
    } else {
      this.facade.toastrService.error('You must enter a valid text.');
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
        if (!err.message) {
          this.facade.toastrService.error(err.message);
        } else {
          this.facade.toastrService.error('The service is not available now. Try again later.');
        }
        updateTask.taskItems.splice(itemIndex, 0, item);
      });
  }

  showToDoList(option: string) {
    const currentConsultantEmail = this.currentConsultant.emailAddress;

    switch (option) {
      case 'ALL':
        this.toDoListDisplay = this.toDoList;

        if (!this.showAllTasks) {
          this.toDoListDisplay = this.toDoList
                                    .filter(task => this.isSameTextInLowerCase(task.consultant.emailAddress, currentConsultantEmail));
        }

        break;

      case 'OPEN':
        this.toDoListDisplay = this.toDoList.filter(task => !task.isApprove);

        if (!this.showAllTasks) {
          this.toDoListDisplay = this.toDoListDisplay
                                    .filter(task => this.isSameTextInLowerCase(task.consultant.emailAddress, currentConsultantEmail));
        }

        break;

      case 'CLOSED':
        this.toDoListDisplay = this.toDoList.filter(task => task.isApprove);

        if (!this.showAllTasks) {
          this.toDoListDisplay =  this.toDoListDisplay
                                      .filter(task => this.isSameTextInLowerCase(task.consultant.emailAddress, currentConsultantEmail));
        }

        break;
    }
  }

  isSameTextInLowerCase(textA: string, textB: string ) {
    return textA.toLowerCase() === textB.toLowerCase();
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    // Add New Candidate Modal
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
            let isCompleted = true;
            const items: any[] = [];

            if (!this.app.isUserRole(['HRManagement', 'Admin'])) {
              this.validateForm.controls['consultant'].setValue(this.currentConsultant.id.toString());
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
              const consultantID: number = this.validateForm.controls['consultant'].value;
              const newTask: Task = {
                id: newId,
                title: this.validateForm.controls['title'].value,
                isApprove: false,
                creationDate: new Date(),
                endDate: this.validateForm.controls['endDate'].value.toISOString(),
                consultantId: consultantID,
                consultant: this.consultants.filter(consultant => consultant.id === consultantID)[0],
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
                  modal.nzFooter[1].loading = false;
                  if (err.message) {
                    this.facade.toastrService.error(err.message);
                  } else {
                    this.facade.toastrService.error('The service is not available now. Try again later.');
                  }
                });
            } else {
              modal.nzFooter[1].loading = false;
            }
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
    return this.currentConsultant && this.app.isUserRole(['HRManagement', 'Admin']);
  }

  assignToMe() {
    if (this.currentConsultant) {
      this.validateForm.controls['consultant'].setValue(this.currentConsultant.id.toString());
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
    const today: Date = new Date;
    const dueDate: Date = new Date(endDate);
    const dateDiff = (dueDate.getDate() - today.getDate());
    return dateDiff;
  }

  isCurrentUser(task: Task): boolean {
    return task.consultant.emailAddress.toLowerCase() === this.currentConsultant.emailAddress.toLowerCase();
  }

  filterTasks() {
    if (!this.showAllTasks) {
      this.toDoListDisplay = this.toDoListDisplay
        .filter(todo => todo.consultant.emailAddress.toLowerCase() === this.currentConsultant.emailAddress.toLowerCase());
    } else {
      this.toDoListDisplay = this.toDoList;
    }

  }

  ngOnDestroy() {
    this.searchSub.unsubscribe();
  }
}
