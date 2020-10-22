import { Actions, createEffect, ofType } from '@ngrx/effects';
import { CompanyCalendar } from '@shared/models/company-calendar.model';
import { CompanyCalendarService } from '@shared/services/company-calendar.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { companyCalendarActions } from './company-calendar.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class CompanyCalendarEffects {
    loadCompanyCalendarEvents$ = createEffect(() =>
        this.action$.pipe(
            ofType(companyCalendarActions.load),
            switchMap(() =>
                this.companyCalendarService.getEvents()
                    .pipe(
                        map((events: CompanyCalendar[]) => companyCalendarActions.loadSuccess({ events })),
                        catchError((errorMsg: any) => of(companyCalendarActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addCompanyCalendarEvent$ = createEffect(() =>
        this.action$.pipe(
            ofType(companyCalendarActions.add),
            exhaustMap(action =>
                this.companyCalendarService.add(action.event)
                    .pipe(
                        map((newEventId: { id: number }) => {
                            const events = { ...action.event, id: newEventId.id };
                            return companyCalendarActions.addSuccess({ event: events });
                        }),
                        catchError((errorMsg: any) => of(companyCalendarActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editCompanyCalendarEvent$ = createEffect(() =>
        this.action$.pipe(
            ofType(companyCalendarActions.edit),
            exhaustMap(action =>
                this.companyCalendarService.update(action.event.id, action.event)
                    .pipe(
                        map(() => companyCalendarActions.editSuccess({ event: action.event })),
                        catchError((errorMsg: any) => of(companyCalendarActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeCompanyCalendarEvent$ = createEffect(() =>
        this.action$.pipe(
            ofType(companyCalendarActions.remove),
            exhaustMap((action) =>
                this.companyCalendarService.delete(action.eventId)
                    .pipe(
                        map(() => companyCalendarActions.removeSuccess({ eventId: action.eventId })),
                        catchError((errorMsg: any) => of(companyCalendarActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private companyCalendarService: CompanyCalendarService) {

    }
}
