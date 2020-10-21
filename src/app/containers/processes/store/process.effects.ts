import { Injectable } from '@angular/core';
import { createEffect, Actions, ofType } from '@ngrx/effects';
import { ProcessService } from '@shared/services/process.service';
import { processActions } from './process.action';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of} from 'rxjs';
import { CandidateService } from '@shared/services/candidate.service';
import { handleError } from '@shared/utils/utils.functions';
@Injectable()

export class ProcessEffects{

    loadProcesses$ = createEffect(() =>
        this.action$.pipe(
            ofType(processActions.load),
            mergeMap(() =>
                this.processService.getTableView()
                .pipe(
                    map((processesTableView) => processActions.onLoadSuccess({ processesTableView })),
                    catchError((err) => {
                        const errorMsg = (handleError(err, (errResponse) => [...errResponse]) as string[]).join('\n');
                        return of(processActions.onLoadFailed({ errorMsg }));
                    })
                )
            )
        )
    );

    loadDeletedProcesses$ = createEffect(() =>
        this.action$.pipe(
            ofType(processActions.loadDeleted),
            mergeMap(() =>
                this.processService.getDeletedProcesses()
                .pipe(
                    map((deletedProcessesTableView) => processActions.onLoadDeletedSuccess({ deletedProcessesTableView })),
                    catchError((err) => {
                        const errorMsg = (handleError(err, (errResponse) => [...errResponse]) as string[]).join('\n');
                        return of(processActions.onLoadFailed({ errorMsg }));
                    })
                )
            )
        )
    );

    addProcess$ = createEffect(() =>
        this.action$.pipe(
            ofType(processActions.add),
            mergeMap(action => {
                return this.processService.addProcess(action.process)
                .pipe(
                    map((processTableView) => {
                        processTableView.candidate.community = action.process.candidate.community;
                        processTableView.candidate.profile = action.process.candidate.profile;
                        return processActions.onAddSuccess({processTableView});
                    }),
                    catchError((err) => {
                        const errorMsg = (handleError(err, (errResponse) => [...errResponse]) as string[]).join('\n');
                        return of(processActions.onAddFailed({errorMsg}));
                    })
                );
            })
        )
    );

    updateProcess$ = createEffect(() =>
        this.action$.pipe(
            ofType(processActions.update),
            mergeMap(action => {
                return this.processService.update(action.process.id, action.process)
                .pipe(
                    map((updatedProcess) => {
                        return processActions.onUpdateSuccess({process: updatedProcess});
                    }),
                    catchError((err) => {
                        const errorMsg = (handleError(err, (errResponse) => [...errResponse]) as string[]).join('\n');
                        return of(processActions.onUpdateFailed({ errorMsg }));
                    })
                );
            })
        )
    );

    removeProcesses$ = createEffect(() =>
        this.action$.pipe(
            ofType(processActions.remove),
            mergeMap(action =>
                this.processService.delete(action.processId)
                .pipe(
                    map(() => {
                        return processActions.onRemoveSuccess({ processId: action.processId});
                    }),
                    catchError((err) => {
                        const errorMsg = (handleError(err, (errResponse) => [...errResponse]) as string[]).join('\n');
                        return of(processActions.onRemoveFailed({errorMsg}));
                    })
                )
            )
        )
    );

    reactivateProcesses$ = createEffect(() =>
        this.action$.pipe(
            ofType(processActions.reactivate),
            mergeMap(action =>
                this.processService.reactivate(action.processId)
                .pipe(
                    map((reactivatedProcess) => {
                        return processActions.onReactivateSuccess({ processId: action.processId, reactivatedStatus: reactivatedProcess.status});
                    }),
                    catchError((err) => {
                        const errorMsg = (handleError(err, (errResponse) => [...errResponse]) as string[]).join('\n');
                        return of(processActions.onRemoveFailed({errorMsg}));
                    })
                )
            )
        )
    );

    approveProcess$ = createEffect(() =>
        this.action$.pipe(
            ofType(processActions.approve),
            mergeMap(action =>
                this.processService.approve(action.processId)
                .pipe(
                    map((approvedProcess) => {
                        return processActions.onApproveSuccess({ approvedProcess });
                    }),
                    catchError((err) => {
                        const errorMsg = (handleError(err, (errResponse) => [...errResponse]) as string[]).join('\n');
                        return of(processActions.onApproveFailed({errorMsg}));
                    })
                )
            )
        )
    );

    rejectProcess$ = createEffect(() =>
        this.action$.pipe(
            ofType(processActions.reject),
            mergeMap(action =>
                this.processService.reject(action.processId, action.rejectionReason)
                .pipe(
                    map(() => {
                        return processActions.onRejectSuccess({processId: action.processId});
                    }),
                    catchError((err) => {
                        const errorMsg = (handleError(err, (errResponse) => [...errResponse]) as string[]).join('\n');
                        return of(processActions.onRejectFailed({ errorMsg }));
                    })
                )
            )
        )
    )

    constructor(
        private action$: Actions,
        private processService: ProcessService,
        private candidateService: CandidateService
    ) { }
}
