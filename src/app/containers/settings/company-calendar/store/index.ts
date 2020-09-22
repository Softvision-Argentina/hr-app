import { ActionReducerMap, createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromCompanyCalendar from './company-calendar.reducer';

export interface State {
    [fromCompanyCalendar.key]: fromCompanyCalendar.State;
}

export const companyCalendarReducers: ActionReducerMap<State> = {
    [fromCompanyCalendar.key]: fromCompanyCalendar.reducer,
};

export const selectCompanyCalendarState = createFeatureSelector<State, fromCompanyCalendar.State>(fromCompanyCalendar.key);

export const selectCompanyCalendarEvents = createSelector(
    selectCompanyCalendarState,
    fromCompanyCalendar.selectCompanyCalendarEvents
);
export const selectCompanyCalendarErrorMsg = createSelector(
    selectCompanyCalendarState,
    fromCompanyCalendar.selectCompanyCalendarErrorMsg
);
export const selectCompanyCalendarLoading = createSelector(
    selectCompanyCalendarState,
    fromCompanyCalendar.selectCompanyCalendarLoading
);
export const selectCompanyCalendarFailed = createSelector(
    selectCompanyCalendarState,
    fromCompanyCalendar.selectCompanyCalendarFailed
);
