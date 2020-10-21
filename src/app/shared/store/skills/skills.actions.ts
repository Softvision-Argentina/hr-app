import { createAction, props } from '@ngrx/store';
import { Skill } from '@shared/models/skill.model';

export const skillsActions = {
    add: createAction('[Skills] add', props<{ skill: Skill }>()),
    addSuccess: createAction('[Skills] addSuccess', props<any>()),
    addFailed: createAction('[Skills] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[Skills] load'),
    loadSuccess: createAction('[Skills] loadSuccess', props<{ skills: Skill[] }>()),
    loadFailed: createAction('[Skills] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[Skills] edit', props<{ skill: Skill }>()),
    editSuccess: createAction('[Skills] editSuccess', props<{ skill: Skill }>()),
    editFailed: createAction('[Skills] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[Skills] remove', props<{ skillId: number }>()),
    removeSuccess: createAction('[Skills] removeSuccess', props<{ skillId: number }>()),
    removeFailed: createAction('[Skills] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[Skills] resetFailed'),
};
