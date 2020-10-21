import { Injectable } from '@angular/core';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { officeActions } from '@shared/store/office/office.actions';
import { Office } from '@shared/models/office.model';
import { State, selectOfficesLoading, selectOfficesErrorMsg,selectOfficesFailed  } from '@shared/store';

@Injectable()
export class OfficeSandbox extends Sandbox {

    officesLoading$ = this.appState$.pipe(
        select(selectOfficesLoading)
    );

    communtiesFailed$ = this.appState$.pipe(
        select(selectOfficesFailed)
    );

    communtiesErrorMsg$ = this.appState$.pipe(
        select(selectOfficesErrorMsg)
    );

    constructor(private appState$: Store<State>) {
        super(appState$);
    }
            
    addOffice(newOffice: Office) {
        this.appState$.dispatch(officeActions.add({ office: newOffice }));
    }

    removeOffice(officeId: number) {
        this.appState$.dispatch(officeActions.remove({ officeId }));
    }

    editOffice(office: Office) {
        this.appState$.dispatch(officeActions.edit({ office: office }));
    }

    resetFailed() {
        this.appState$.dispatch(officeActions.resetFailed());
    }
}
