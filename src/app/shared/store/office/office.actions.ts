import { createAction, props } from '@ngrx/store';
import { Office } from '@shared/models/office.model';

export const officeActions = {
    add: createAction('[Office] add', props<{ office: Office }>()),
    addSuccess: createAction('[Office] addSuccess', props<Office>()),
    addFailed: createAction('[Office] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[Office] load'),
    loadSuccess: createAction('[Office] loadSuccess', props<{ offices: Office[] }>()),
    loadFailed: createAction('[Office] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[Office] edit', props<{ office: Office }>()),
    editSuccess: createAction('[Office] editSuccess', props<{ office: Office }>()),
    editFailed: createAction('[Office] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[Office] remove', props<{ officeId: number }>()),
    removeSuccess: createAction('[Office] removeSuccess', props<{ officeId: number }>()),
    removeFailed: createAction('[Office] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[Office] resetFailed'),
}