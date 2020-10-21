import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Room } from '@shared/models/room.model';
import { RoomService } from '@shared/services/room.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { roomActions } from './room.actions';

@Injectable()
export class RoomEffects {
    loadRooms$ = createEffect(() =>
        this.action$.pipe(
            ofType(roomActions.load),
            switchMap(() =>
                this.roomService.get()
                    .pipe(
                        map((rooms: Room[]) => roomActions.loadSuccess({ rooms })),
                        catchError((errorMsg: any) => of(roomActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addRoom$ = createEffect(() =>
        this.action$.pipe(
            ofType(roomActions.add),
            exhaustMap(action =>
                this.roomService.add(action.room)
                    .pipe(
                        map((room: { id: number }) => roomActions.addSuccess({ ...action.room, id: room.id })),
                        catchError((errorMsg: any) => of(roomActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editRoom$ = createEffect(() =>
        this.action$.pipe(
            ofType(roomActions.edit),
            exhaustMap(action =>
                this.roomService.update(action.room.id, action.room)
                    .pipe(
                        map(() => roomActions.editSuccess({ room: action.room })),
                        catchError((errorMsg: any) => of(roomActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeRoom$ = createEffect(() =>
        this.action$.pipe(
            ofType(roomActions.remove),
            exhaustMap((action) =>
                this.roomService.delete(action.roomId)
                    .pipe(
                        map(() => roomActions.removeSuccess({ roomId: action.roomId })),
                        catchError((errorMsg: any) => of(roomActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private roomService: RoomService) {

    }
}