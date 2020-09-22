import { createReducer, on } from '@ngrx/store';
import { officeActions } from './office.actions';
import { Office } from '@shared/models/office.model';

export const key = 'office';

export interface State {
    offices: Office[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    offices: [],
    errorMsg: null,
    loading: false,
    failed: null
}

export const reducer = createReducer(initialState,
    on(
        officeActions.add,
        officeActions.edit,
        (state, { office }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        officeActions.addSuccess,
        (state, office) => {
            const addedOffices = [...state.offices.filter((value) => value.id !== office.id), office];
            addedOffices.sort((a, b) => a.id - b.id);
            for (let i = 0; i < addedOffices.length; i++) {
                if (addedOffices[i].name === 'Indistinto') {
                    const indistinto = addedOffices.splice(i, 1);
                    addedOffices.push(indistinto[0]);
                }
            }

            return {
                ...state,
                offices: addedOffices,
                loading: false,
                failed: false
            }
        }
    ),
    on(
        officeActions.loadSuccess,
        (state, { offices }) => {
            const loadedOffices = offices.slice()
            for (let i = 0; i < loadedOffices.length; i++) {
                if (loadedOffices[i].name === 'Indistinto') {
                    const indistinto = loadedOffices.splice(i, 1);
                    loadedOffices.push(indistinto[0]);
                }
            }

            return {
                ...state,
                offices: loadedOffices,
                loading: false,
                failed: false
            }
        }
    ),
    on(
        officeActions.editSuccess,
        (state, { office }) => {
            const editedOffices = [...state.offices.filter((value) => value.id !== office.id), office];
            editedOffices.sort((a, b) => a.id - b.id);
            for (let i = 0; i < editedOffices.length; i++) {
                if (editedOffices[i].name === 'Indistinto') {
                    const indistinto = editedOffices.splice(i, 1);
                    editedOffices.push(indistinto[0]);
                }
            }

            return {
                ...state,
                offices: editedOffices,
                loading: false,
                failed: false
            };
        }
    ),
    on(
        officeActions.removeSuccess,
        (state, { officeId }) => ({
            ...state,
            offices: state.offices.filter(c => c.id !== officeId),
            loading: false,
            failed: false
        })
    ),
    on(
        officeActions.loadFailed,
        officeActions.addFailed,
        officeActions.editFailed,
        officeActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        officeActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectOffices = (state: State) => state.offices;
export const selectOfficesErrorMsg = (state: State) => state.errorMsg;
export const selectOfficesLoading = (state: State) => state.loading;
export const selectOfficesFailed = (state: State) => state.failed;
