import { createAction, props } from '@ngrx/store';
import { Role } from '@shared/models/role.model';

export const roleActions = {
    add: createAction('[Role] add', props<{ role: Role }>()),
    addSuccess: createAction('[Role] addSuccess', props<{ role: Role }>()),
    addFailed: createAction('[Role] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[Role] load'),
    loadSuccess: createAction('[Role] loadSuccess', props<{ roles: Role[] }>()),
    loadFailed: createAction('[Role] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[Role] edit', props<{ role: Role }>()),
    editSuccess: createAction('[Role] editSuccess', props<{ role: Role }>()),
    editFailed: createAction('[Role] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[Role] remove', props<{ roleId: number }>()),
    removeSuccess: createAction('[Role] removeSuccess', props<{ roleId: number }>()),
    removeFailed: createAction('[Role] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[Role] resetFailed'),
};
