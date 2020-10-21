import { Injectable } from '@angular/core';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { State, selectSkillsFailed, selectSkillsLoading, selectSkillsErrorMsg } from '@shared/store/index';
import { skillsActions } from '@shared/store/skills/skills.actions';
import { Skill } from '@shared/models/skill.model';
@Injectable()

export class SkillsSandbox extends Sandbox {

    skillsLoading$ = this.appState$.pipe(
        select(selectSkillsLoading)
    );

    skillsFailed$ = this.appState$.pipe(
        select(selectSkillsFailed)
    );

    skillsErrorMsg$ = this.appState$.pipe(
        select(selectSkillsErrorMsg)
    );

    constructor(private appState$: Store<State>) {
        super(appState$);
    }

    addSkill(newSkill: Skill) {
        this.appState$.dispatch(skillsActions.add({ skill: newSkill }));
    }

    removeSkill(skillId: number) {
        this.appState$.dispatch(skillsActions.remove({ skillId }));
    }

    editSkill(skill: Skill) {
        this.appState$.dispatch(skillsActions.edit({ skill }));
    }

    resetFailed() {
        this.appState$.dispatch(skillsActions.resetFailed());
    }
}
