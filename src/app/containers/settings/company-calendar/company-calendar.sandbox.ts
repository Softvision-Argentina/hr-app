import { Injectable } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { State, selectCompanyCalendarFailed, selectCompanyCalendarLoading, selectCompanyCalendarErrorMsg, selectCompanyCalendarEvents } from './store/index';
import { companyCalendarActions } from './store/company-calendar.actions';
import { CompanyCalendar } from '@shared/models/company-calendar.model';
@Injectable()

export class CompanyCalendarSandbox {

    companyCalendarEvents$ = this.companyCalendarState$.pipe(
        select(selectCompanyCalendarEvents)
    );

    companyCalendarLoading$ = this.companyCalendarState$.pipe(
        select(selectCompanyCalendarLoading)
    );

    companyCalendarFailed$ = this.companyCalendarState$.pipe(
        select(selectCompanyCalendarFailed)
    );

    companyCalendarErrorMsg$ = this.companyCalendarState$.pipe(
        select(selectCompanyCalendarErrorMsg)
    );

    constructor(private companyCalendarState$: Store<State>) {
    }

    loadCompanyCalendarEvents(): void {
        this.companyCalendarState$.dispatch(companyCalendarActions.load());
    }

    addCompanyCalendarEvent(event: CompanyCalendar) {
        this.companyCalendarState$.dispatch(companyCalendarActions.add({ event }));
    }

    removeCompanyCalendarEvent(eventId: number) {
        this.companyCalendarState$.dispatch(companyCalendarActions.remove({ eventId }));
    }

    editCompanyCalendarEvent(event: CompanyCalendar) {
        this.companyCalendarState$.dispatch(companyCalendarActions.edit({ event }));
    }

    resetFailed() {
        this.companyCalendarState$.dispatch(companyCalendarActions.resetFailed());
    }
}
