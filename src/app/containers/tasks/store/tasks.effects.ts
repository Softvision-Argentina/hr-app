import { Injectable } from "@angular/core";
import { createEffect, Actions, ofType, act } from '@ngrx/effects';
import { TaskService } from '@shared/services/task.service';
import { tasksActions } from './tasks.actions';
import { mergeMap, map, catchError, tap } from 'rxjs/operators';
import { of } from "rxjs";
@Injectable()

export class TasksEffects {
    addTask$ = createEffect(() =>
        this.action$.pipe(
            ofType(tasksActions.add),
            mergeMap(action =>
                this.tasksService.add(action.task)
                    .pipe(
                        map((taskId: { id: number }) => tasksActions.addSuccess({ task: { ...action.task, id: taskId.id } })),
                        catchError(() => of(tasksActions.addFailed()))
                    )
            )
        )
    );
    loadTask$ = createEffect(() =>
        this.action$.pipe(
            ofType(tasksActions.load),
            mergeMap((action) =>
                this.tasksService.getTasks(action.userRole, action.userEmail)
                    .pipe(
                        map((tasks) => tasksActions.loadSuccess({ tasks })),
                        catchError(() => of(tasksActions.loadFailed()))
                    )
            )
        )
    );

    removeTask$ = createEffect(() =>
        this.action$.pipe(
            ofType(tasksActions.remove),
            mergeMap((action) =>
                this.tasksService.delete(action.taskId)
                    .pipe(
                        map(() => tasksActions.onRemoveSuccess({ taskId: action.taskId })),
                        catchError(() => of(tasksActions.onRemoveFailed()))
                    )
            )
        )
    )

    constructor(
        private action$: Actions,
        private tasksService: TaskService
    ) { }
}
