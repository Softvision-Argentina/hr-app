import { createAction, props } from '@ngrx/store';
import { Room } from '@shared/models/room.model';

export const roomActions = {
    add: createAction('[Room] add', props<{ room: Room }>()),
    addSuccess: createAction('[Room] addSuccess', props<Room>()),
    addFailed: createAction('[Room] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[Room] load'),
    loadSuccess: createAction('[Room] loadSuccess', props<{ rooms: Room[] }>()),
    loadFailed: createAction('[Room] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[Room] edit', props<{ room: Room }>()),
    editSuccess: createAction('[Room] editSuccess', props<{ room: Room }>()),
    editFailed: createAction('[Room] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[Room] remove', props<{ roomId: number }>()),
    removeSuccess: createAction('[Room] removeSuccess', props<{ roomId: number }>()),
    removeFailed: createAction('[Room] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[Room] resetFailed'),
}