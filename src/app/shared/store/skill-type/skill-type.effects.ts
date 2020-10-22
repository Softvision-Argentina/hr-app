import { Actions, createEffect, ofType } from '@ngrx/effects';
import { SkillType } from '@shared/models/skill-type.model';
import { SkillTypeService } from '@shared/services/skill-type.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { skillTypeActions } from './skill-type.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class SkillTypesEffects {
    loadSkillTypes$ = createEffect(() =>
        this.action$.pipe(
            ofType(skillTypeActions.load),
            switchMap(() =>
                this.skillTypeService.getSkillTypes()
                    .pipe(
                        map((skillTypes: SkillType[]) => skillTypeActions.loadSuccess({ skillTypes })),
                        catchError((errorMsg: any) => of(skillTypeActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addSkillType$ = createEffect(() =>
        this.action$.pipe(
            ofType(skillTypeActions.add),
            exhaustMap(action =>
                this.skillTypeService.add(action.skillType)
                    .pipe(
                        map((skillType: { id: number }) => skillTypeActions.addSuccess({ ...action.skillType, id: skillType.id })),
                        catchError((errorMsg: any) => of(skillTypeActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editSkillType$ = createEffect(() =>
        this.action$.pipe(
            ofType(skillTypeActions.edit),
            exhaustMap(action =>
                this.skillTypeService.update(action.skillType.id, action.skillType)
                    .pipe(
                        map(() => skillTypeActions.editSuccess({ skillType: action.skillType })),
                        catchError((errorMsg: any) => of(skillTypeActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeSkillType$ = createEffect(() =>
        this.action$.pipe(
            ofType(skillTypeActions.remove),
            exhaustMap((action) =>
                this.skillTypeService.delete(action.skillTypeId)
                    .pipe(
                        map(() => skillTypeActions.removeSuccess({ skillTypeId: action.skillTypeId })),
                        catchError((errorMsg: any) => of(skillTypeActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private skillTypeService: SkillTypeService) {
    }
}
