import { createAction, props } from '@ngrx/store';
import { SkillType } from '@shared/models/skill-type.model';

export const skillTypeActions = {
    add: createAction('[SkillType] add', props<{ skillType: SkillType }>()),
    addSuccess: createAction('[SkillType] addSuccess', props<SkillType>()),
    addFailed: createAction('[SkillType] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[SkillType] load'),
    loadSuccess: createAction('[SkillType] loadSuccess', props<{ skillTypes: SkillType[] }>()),
    loadFailed: createAction('[SkillType] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[SkillType] edit', props<{ skillType: SkillType }>()),
    editSuccess: createAction('[SkillType] editSuccess', props<{ skillType: SkillType }>()),
    editFailed: createAction('[SkillType] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[SkillType] remove', props<{ skillTypeId: number }>()),
    removeSuccess: createAction('[SkillType] removeSuccess', props<{ skillTypeId: number }>()),
    removeFailed: createAction('[SkillType] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[SkillType] resetFailed'),
};
