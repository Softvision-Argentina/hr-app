import { Injectable } from '@angular/core';
// import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { TasksState, getTasksState, getOpenModal, getTasksLoading, getTasksError, getTasksAddingLoading } from './store/task.reducer';
import { tasksActions } from './store/tasks.actions';
// import { State } from '@shared/store';
@Injectable()

export class TasksSandbox {
    tasks$ = this.tasksState$.pipe(
        select(getTasksState)
    );

    // tasksLoading$ = this.appState$.pipe(
    //     select(getTasksLoading)
    // );

    tasksLoadingError$ = this.tasksState$.pipe(
        select(getTasksError)
    );

    tasksIsModalOpen$ = this.tasksState$.pipe(
        select(getOpenModal)
    );

    tasksOnAddingLoading$ = this.tasksState$.pipe(
        select(getTasksAddingLoading)
    );

    constructor(private tasksState$: Store<TasksState>) {
    }

    add(newTask: any) {
        this.tasksState$.dispatch(tasksActions.add({ task: newTask }));
    }

    remove(taskId: number) {
        this.tasksState$.dispatch(tasksActions.remove({ taskId }));
    }

    loadTasks(userRole, userEmail) {
        this.tasksState$.dispatch(tasksActions.load({userRole, userEmail}));
    }

    toogleModal() {
        this.tasksState$.dispatch(tasksActions.toogleModal());
    }
}
