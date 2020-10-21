import { Injectable } from '@angular/core';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { State, selectReaddressReasonsFailed, selectReaddressReasonsLoading, selectReaddressReasonsErrorMsg } from '@shared/store/index';
import { readdressReasonActions } from '@shared/store/readdress-reason/readdress-reason.actions';
import { ReaddressReason } from '@shared/models/readdress-reason.model';
@Injectable()

export class ReaddressReasonSandbox extends Sandbox {

    readdressReasonsLoading$ = this.appState$.pipe(
        select(selectReaddressReasonsLoading)
    );

    readdressReasonsFailed$ = this.appState$.pipe(
        select(selectReaddressReasonsFailed)
    );

    readdressReasonsErrorMsg$ = this.appState$.pipe(
        select(selectReaddressReasonsErrorMsg)
    );

    constructor(private appState$: Store<State>) {
        super(appState$);
    }

    addReaddressReason(newReaddressReason: ReaddressReason) {
        this.appState$.dispatch(readdressReasonActions.add({ readdressReason: newReaddressReason }));
    }

    removeReaddressReason(readdressReasonId: number) {
        this.appState$.dispatch(readdressReasonActions.remove({ readdressReasonId }));
    }

    editReaddressReason(readdressReason: ReaddressReason) {
        this.appState$.dispatch(readdressReasonActions.edit({ readdressReason }));
    }

    resetFailed() {
        this.appState$.dispatch(readdressReasonActions.resetFailed());
    }
}
