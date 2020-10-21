import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Skill } from '@shared/models/skill.model';
import { SkillService } from '@shared/services/skill.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { skillsActions } from './skills.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class SkillsEffects {
    loadSkills$ = createEffect(() =>
        this.action$.pipe(
            ofType(skillsActions.load),
            switchMap(() =>
                this.skillService.get()
                    .pipe(
                        map((skills: Skill[]) => skillsActions.loadSuccess({ skills })),
                        catchError((errorMsg: any) => of(skillsActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addSkill$ = createEffect(() =>
        this.action$.pipe(
            ofType(skillsActions.add),
            exhaustMap(action =>
                this.skillService.add(action.skill)
                    .pipe(
                        map((skill: { id: number }) => skillsActions.addSuccess({ ...action.skill, id: skill.id })),
                        catchError((errorMsg: any) => of(skillsActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editSkill$ = createEffect(() =>
        this.action$.pipe(
            ofType(skillsActions.edit),
            exhaustMap(action =>
                this.skillService.update(action.skill.id, action.skill)
                    .pipe(
                        map(() => skillsActions.editSuccess({ skill: action.skill })),
                        catchError((errorMsg: any) => of(skillsActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeSkill$ = createEffect(() =>
        this.action$.pipe(
            ofType(skillsActions.remove),
            exhaustMap((action) =>
                this.skillService.delete(action.skillId)
                    .pipe(
                        map(() => skillsActions.removeSuccess({ skillId: action.skillId })),
                        catchError((errorMsg: any) => of(skillsActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private skillService: SkillService) {

    }
}
