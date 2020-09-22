import { createAction, props } from '@ngrx/store';
import { CandidateProfile } from '@shared/models/candidate-profile.model';

export const candidateProfilesActions = {
    add: createAction('[CandidateProfile] add', props<{ candidateProfile: CandidateProfile }>()),
    addSuccess: createAction('[CandidateProfile] addSuccess', props<CandidateProfile>()),
    addFailed: createAction('[CandidateProfile] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[CandidateProfile] load'),
    loadSuccess: createAction('[CandidateProfile] loadSuccess', props<{ candidateProfiles: CandidateProfile[] }>()),
    loadFailed: createAction('[CandidateProfile] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[CandidateProfile] edit', props<{ candidateProfile: CandidateProfile }>()),
    editSuccess: createAction('[CandidateProfile] editSuccess', props<{ candidateProfile: CandidateProfile }>()),
    editFailed: createAction('[CandidateProfile] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[CandidateProfile] remove', props<{ candidateProfileId: number }>()),
    removeSuccess: createAction('[CandidateProfile] removeSuccess', props<{ candidateProfileId: number }>()),
    removeFailed: createAction('[CandidateProfile] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[CandidateProfile] resetFailed'),
}