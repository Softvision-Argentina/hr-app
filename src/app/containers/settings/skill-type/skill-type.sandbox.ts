import { Injectable } from '@angular/core';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { State, selectSkillTypesFailed, selectSkillTypesLoading, selectSkillTypesErrorMsg } from '@shared/store/index';
import { skillTypeActions } from '@shared/store/skill-type/skill-type.actions';
import { SkillType } from '@shared/models/skill-type.model';
@Injectable()

export class SkillTypesSandbox extends Sandbox {

    skillTypesLoading$ = this.appState$.pipe(
        select(selectSkillTypesLoading)
    );

    skillTypesFailed$ = this.appState$.pipe(
        select(selectSkillTypesFailed)
    );

    skillTypesErrorMsg$ = this.appState$.pipe(
        select(selectSkillTypesErrorMsg)
    );

    constructor(private appState$: Store<State>) {
        super(appState$);
    }

    addSkillType(newSkillType: SkillType) {
        this.appState$.dispatch(skillTypeActions.add({ skillType: newSkillType }));
    }

    removeSkillType(skillTypeId: number) {
        this.appState$.dispatch(skillTypeActions.remove({ skillTypeId }));
    }

    editSkillType(skillType: SkillType) {
        this.appState$.dispatch(skillTypeActions.edit({ skillType }));
    }

    resetFailed() {
        this.appState$.dispatch(skillTypeActions.resetFailed());
    }
}
