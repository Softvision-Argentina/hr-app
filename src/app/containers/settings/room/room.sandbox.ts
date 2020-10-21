import { Injectable } from '@angular/core';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { roomActions } from '@shared/store/room/room.actions';
import { Room } from '@shared/models/room.model';
import { State, selectRoomsLoading, selectRoomsErrorMsg,selectRoomsFailed  } from '@shared/store';

@Injectable()
export class RoomSandbox extends Sandbox {

    roomsLoading$ = this.appState$.pipe(
        select(selectRoomsLoading)
    );

    roomsFailed$ = this.appState$.pipe(
        select(selectRoomsFailed)
    );

    roomsErrorMsg$ = this.appState$.pipe(
        select(selectRoomsErrorMsg)
    );

    constructor(private appState$: Store<State>) {
        super(appState$);
    }
            
    addRoom(newRoom: Room) {
        this.appState$.dispatch(roomActions.add({ room: newRoom }));
    }

    removeRoom(roomId: number) {
        this.appState$.dispatch(roomActions.remove({ roomId }));
    }

    editRoom(room: Room) {
        this.appState$.dispatch(roomActions.edit({ room: room }));
    }

    resetFailed() {
        this.appState$.dispatch(roomActions.resetFailed());
    }
}
