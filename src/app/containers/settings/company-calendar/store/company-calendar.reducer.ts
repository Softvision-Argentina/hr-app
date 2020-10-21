import { createReducer, on } from '@ngrx/store';
import { CompanyCalendar } from '@shared/models/company-calendar.model';
import { companyCalendarActions } from './company-calendar.actions';

export const key = 'companyCalendar';

export interface State {
    events: CompanyCalendar[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    events: [],
    errorMsg: null,
    loading: false,
    failed: null
};

export const reducer = createReducer(initialState,
    on(
        companyCalendarActions.add,
        companyCalendarActions.edit,
        (state) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        companyCalendarActions.addSuccess,
        (state, { event }) => {
            const addedEvents = [...state.events, event];
            addedEvents.sort((a, b) => {
                const d1 = new Date(a.date);
                const d2 = new Date(b.date);
                return d1 > d2 ? -1 : d1 < d2 ? 1 : 0;
            });
            return ({
                ...state,
                events: addedEvents,
                loading: false,
                failed: false
            });
        }
    ),
    on(
        companyCalendarActions.loadSuccess,
        (state, { events }) => {
            const sortedEvents = events.slice().sort((a, b) => {
                const d1 = new Date(a.date);
                const d2 = new Date(b.date);
                return d1 > d2 ? -1 : d1 < d2 ? 1 : 0;
            });

            return {
                ...state,
                events: sortedEvents,
                loading: false,
                failed: false
            };
        }
    ),
    on(
        companyCalendarActions.editSuccess,
        (state, { event }) => {
            const editedEvents = [...state.events.filter((value) => value.id !== event.id), event];
            editedEvents.sort((a, b) => {
                const d1 = new Date(a.date);
                const d2 = new Date(b.date);
                return d1 > d2 ? -1 : d1 < d2 ? 1 : 0;
            });
            return {
                ...state,
                events: editedEvents,
                loading: false,
                failed: false
            }
        }
    ),
    on(
        companyCalendarActions.removeSuccess,
        (state, { eventId }) => ({
            ...state,
            events: state.events.filter(c => c.id !== eventId),
            loading: false,
            failed: false
        })
    ),
    on(
        companyCalendarActions.loadFailed,
        companyCalendarActions.addFailed,
        companyCalendarActions.editFailed,
        companyCalendarActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        companyCalendarActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectCompanyCalendarEvents = (state: State) => state.events;
export const selectCompanyCalendarErrorMsg = (state: State) => state.errorMsg;
export const selectCompanyCalendarLoading = (state: State) => state.loading;
export const selectCompanyCalendarFailed = (state: State) => state.failed;
