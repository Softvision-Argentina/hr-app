import { createAction, props } from '@ngrx/store';
import { CompanyCalendar } from '@shared/models/company-calendar.model';

export const companyCalendarActions = {
    add: createAction('[CompanyCalendar] add', props<{ event: CompanyCalendar }>()),
    addSuccess: createAction('[CompanyCalendar] addSuccess', props<{ event: CompanyCalendar }>()),
    addFailed: createAction('[CompanyCalendar] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[CompanyCalendar] load'),
    loadSuccess: createAction('[CompanyCalendar] loadSuccess', props<{ events: CompanyCalendar[] }>()),
    loadFailed: createAction('[CompanyCalendar] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[CompanyCalendar] edit', props<{ event: CompanyCalendar }>()),
    editSuccess: createAction('[CompanyCalendar] editSuccess', props<{ event: CompanyCalendar }>()),
    editFailed: createAction('[CompanyCalendar] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[CompanyCalendar] remove', props<{ eventId: number }>()),
    removeSuccess: createAction('[CompanyCalendar] removeSuccess', props<{ eventId: number }>()),
    removeFailed: createAction('[CompanyCalendar] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[CompanyCalendar] resetFailed'),
};
