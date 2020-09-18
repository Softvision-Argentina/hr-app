import { createAction, props } from '@ngrx/store';


export const tasksActions = {
    add: createAction('[TaskComponent] add', props<{ task: any }>()),
    addSuccess: createAction('[TaskComponent] addSuccess', props<{ task: any }>()),
    addFailed: createAction('[TaskComponent] addFailed'),
    toogleModal: createAction('[TaskComponent] toogleModal'),
    load: createAction('[TaskComponent] load', props<{ userId: number }>()),
    loadSuccess: createAction('[TaskComponent] loadSuccess', props<{ tasks: any }>()),
    loadFailed: createAction('[TaskComponent] loadFailed'),
    remove: createAction('[TaskComponent] remove', props<{ taskId: number }>()),
    onRemoveSuccess: createAction('[TaskComponent] onRemoveSuccess', props<{ taskId: number }>()),
    onRemoveFailed: createAction('[TaskComponent] onRemoveFailed'),
};
