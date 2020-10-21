import { Injectable } from '@angular/core';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { State, selectReaddressReasonTypesFailed, selectReaddressReasonTypesLoading, selectReaddressReasonTypesErrorMsg } from '@shared/store/index';
import { readdressReasonTypesActions } from '@shared/store/readdress-reason-type/readdress-reason-type.actions';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
@Injectable()

export class ReaddressReasonTypesSandbox extends Sandbox {

    readdressReasonTypesLoading$ = this.appState$.pipe(
        select(selectReaddressReasonTypesLoading)
    );

    readdressReasonTypesFailed$ = this.appState$.pipe(
        select(selectReaddressReasonTypesFailed)
    );

    readdressReasonTypesErrorMsg$ = this.appState$.pipe(
        select(selectReaddressReasonTypesErrorMsg)
    );

    constructor(private appState$: Store<State>) {
        super(appState$);
    }

    addReaddressReasonType(newReaddressReasonType: ReaddressReasonType) {
        this.appState$.dispatch(readdressReasonTypesActions.add({ readdressReasonType: newReaddressReasonType }));
    }

    removeReaddressReasonType(readdressReasonTypeId: number) {
        this.appState$.dispatch(readdressReasonTypesActions.remove({ readdressReasonTypeId }));
    }

    editReaddressReasonType(readdressReasonType: ReaddressReasonType) {
        this.appState$.dispatch(readdressReasonTypesActions.edit({ readdressReasonType }));
    }

    resetFailed() {
        this.appState$.dispatch(readdressReasonTypesActions.resetFailed());
    }
}
