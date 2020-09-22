import { createReducer, on } from '@ngrx/store';
import { roomActions } from './room.actions';
import { Room } from '@shared/models/room.model';

export const key = 'room';

export interface State {
    rooms: Room[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    rooms: [],
    errorMsg: null,
    loading: false,
    failed: null
}

export const reducer = createReducer(initialState,
    on(
        roomActions.add,
        roomActions.edit,
        (state, { room }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        roomActions.addSuccess,
        (state, room) => ({
            ...state,
            rooms: [...state.rooms, room],
            loading: false,
            failed: false
        })
    ),
    on(
        roomActions.loadSuccess,
        (state, { rooms }) => ({
            ...state,
            rooms,
            loading: false,
            failed: false
        })
    ),
    on(
        roomActions.editSuccess,
        (state, { room }) => {
            const editedRooms = [...state.rooms.filter((value) => value.id !== room.id), room];
            editedRooms.sort((a, b) => a.id - b.id);
            return {
                ...state,
                rooms: editedRooms,
                loading: false,
                failed: false
            }
        }
    ),
    on(
        roomActions.removeSuccess,
        (state, { roomId }) => ({
            ...state,
            rooms: state.rooms.filter(c => c.id !== roomId),
            loading: false,
            failed: false
        })
    ),
    on(
        roomActions.loadFailed,
        roomActions.addFailed,
        roomActions.editFailed,
        roomActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        roomActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectRooms = (state: State) => state.rooms;
export const selectRoomsErrorMsg = (state: State) => state.errorMsg;
export const selectRoomsLoading = (state: State) => state.loading;
export const selectRoomsFailed = (state: State) => state.failed;
