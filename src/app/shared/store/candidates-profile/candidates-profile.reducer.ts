import { createReducer, on } from '@ngrx/store';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { candidateProfilesActions } from './candidates-profile.actions';

export const key = 'candidateProfiles';

export interface State {
    candidateProfiles: CandidateProfile[];
    errorMsg?: any;
    loading: boolean;
    failed: any;
}

export const initialState: State = {
    candidateProfiles: [],
    errorMsg: null,
    loading: false,
    failed: null
}

export const reducer = createReducer(initialState,
    on(
        candidateProfilesActions.add,
        candidateProfilesActions.edit,
        (state, { candidateProfile }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        candidateProfilesActions.addSuccess,
        (state, candidateProfile) => {
            const profiles = [...state.candidateProfiles, candidateProfile].sort((a, b) => (a.name).localeCompare(b.name));
            for (let i = 0; i < profiles.length; i++) {
                if (profiles[i].name === 'N/A') {
                    const NA = profiles.splice(i, 1);
                    profiles.unshift(NA[0]);
                }
            }
            return {
                ...state,
                candidateProfiles: profiles,
                loading: false,
                failed: false
            };
        }
    ),
    on(
        candidateProfilesActions.loadSuccess,
        (state, { candidateProfiles }) => {
            
            const profiles = candidateProfiles.slice().sort((a, b) => (a.name).localeCompare(b.name));
            for (let i = 0; i < profiles.length; i++) {
                if (profiles[i].name === 'N/A') {
                    const NA = profiles.splice(i, 1);
                    profiles.unshift(NA[0]);
                }
            }
            return ({
                ...state,
                candidateProfiles: profiles,
                loading: false,
                failed: false
            })
        }
    ),
    on(
        candidateProfilesActions.editSuccess,
        (state, { candidateProfile }) => {
            const updatedProfiles = [...state.candidateProfiles.filter((value) => value.id !== candidateProfile.id), candidateProfile];
            updatedProfiles.sort((a, b) => (a.name.localeCompare(b.name)));
            for (let i = 0; i < updatedProfiles.length; i++) {
                if (updatedProfiles[i].name === 'N/A') {
                    const NA = updatedProfiles.splice(i, 1);
                    updatedProfiles.unshift(NA[0]);
                }
            }
            return ({
                ...state,
                candidateProfiles: updatedProfiles,
                loading: false,
                failed: false
            })
        }
    ),
    on(
        candidateProfilesActions.removeSuccess,
        (state, { candidateProfileId }) => ({
            ...state,
            candidateProfiles: state.candidateProfiles.filter(c => c.id !== candidateProfileId),
            loading: false,
            failed: false
        })
    ),
    on(
        candidateProfilesActions.loadFailed,
        candidateProfilesActions.addFailed,
        candidateProfilesActions.editFailed,
        candidateProfilesActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        candidateProfilesActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectCandidateProfiles = (state: State) => state.candidateProfiles;
export const selectCandidateProfilesErrorMsg = (state: State) => state.errorMsg;
export const selectCandidateProfilesLoading = (state: State) => state.loading;
export const selectCandidateProfilesFailed = (state: State) => state.failed;
