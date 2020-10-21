import { Sandbox } from '@shared/sandbox/base.sandbox';
import { State } from '@shared/store';
import { Store } from '@ngrx/store';
import { Injectable } from '@angular/core';

@Injectable()
export class ReferralsSandbox extends Sandbox {

     constructor(public appState$: Store<State>) {
        super(appState$);
    }

}