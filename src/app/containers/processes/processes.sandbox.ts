import { Injectable } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ProcessState, getProcesses, getCurrentProcessId, getDeletedProcesses, isLoading, isLoaded, getErrorState } from './store/process.reducer';
import { processActions } from './store/process.action';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { State } from '@shared/store/';
import { Process } from '@shared/models/process.model';
import { ProcessService } from '@shared/services/process.service';
import { take } from 'rxjs/operators';
@Injectable()

export class ProcessSandbox extends Sandbox {
    processes$ = this.processState$.pipe(
        select(getProcesses),
    );

    deletedProcesses$ = this.processState$.pipe(
        select(getDeletedProcesses),
    );

    currentProcessId$ = this.processState$.pipe(
        select(getCurrentProcessId),
    );

    isLoading$ = this.processState$.pipe(
        select(isLoading)
    );

    isSuccessful$ = this.processState$.pipe(
        select(isLoaded)
    );

    getError$ = this.processState$.pipe(
        select(getErrorState)
    );


    loadProcess() {
        // Temporary fix;
        this.processes$.subscribe(processes => {
            if (processes.length === 0) {
                this.processState$.dispatch(processActions.load());
            }
        }).unsubscribe();
    }

    loadDeletedProcess() {
        this.processState$.dispatch(processActions.loadDeleted());
    }

    addProcess(process: Process) {
        this.processState$.dispatch(processActions.add({ process }));
    }

    updateProcess(process: Process) {
        this.processState$.dispatch(processActions.update({ process }));
    }

    deleteProcess(processId: number) {
        this.processState$.dispatch(processActions.remove({ processId }));
    }

    approveProcess(processId: number) {
        this.processState$.dispatch(processActions.approve({ processId }));
    }

    rejectProcess(processId: number, rejectionReason: string) {
        this.processState$.dispatch(processActions.reject({ processId, rejectionReason }));
    }

    getProcessById(processId: number) {
        return this.processService.getByID(processId)
            .pipe(
                take(1)
            );
    }

    reactivate(processId: number) {
        this.processState$.dispatch(processActions.reactivate({ processId }));
    }


    constructor(
        public appState$: Store<State>,
        private processState$: Store<ProcessState>,
        private processService: ProcessService
    ) {
        super(appState$);
    }
}
