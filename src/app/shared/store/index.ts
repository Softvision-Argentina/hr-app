import { ActionReducerMap, createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromCommunities from './communities/communities.reducer';
import * as fromCandidateProfiles from './candidates-profile/candidates-profile.reducer';
import * as fromOffices from './office/office.reducer';
import * as fromRooms from './room/room.reducer';
import * as fromSkills from './skills/skills.reducer';
import * as fromSkillTypes from './skill-type/skill-type.reducer';
import * as fromReaddressReasons from './readdress-reason/readdress-reason.reducer';
import * as fromReaddressReasonTypes from './readdress-reason-type/readdress-reason-type.reducer';

export interface State {
    [fromCommunities.key]: fromCommunities.State;
    [fromCandidateProfiles.key]: fromCandidateProfiles.State;
    [fromOffices.key]: fromOffices.State;
    [fromRooms.key]: fromRooms.State;
    [fromSkills.key]: fromSkills.State;
    [fromSkillTypes.key]: fromSkillTypes.State;
    [fromReaddressReasons.key]: fromReaddressReasons.State;
    [fromReaddressReasonTypes.key]: fromReaddressReasonTypes.State;

}

export const reducers: ActionReducerMap<State> = {
    [fromCommunities.key]: fromCommunities.reducer,
    [fromCandidateProfiles.key]: fromCandidateProfiles.reducer,
    [fromOffices.key]: fromOffices.reducer,
    [fromRooms.key]: fromRooms.reducer,
    [fromSkills.key]: fromSkills.reducer,
    [fromSkillTypes.key]: fromSkillTypes.reducer,
    [fromReaddressReasons.key]: fromReaddressReasons.reducer,
    [fromReaddressReasons.key]: fromReaddressReasons.reducer,
    [fromReaddressReasonTypes.key]: fromReaddressReasonTypes.reducer
};

//Communities selector
export const selectCommunitiesState = createFeatureSelector<State, fromCommunities.State>(fromCommunities.key);

export const selectCommunities = createSelector(
    selectCommunitiesState,
    fromCommunities.selectCommunities
);
export const selectCommunitiesErrorMsg = createSelector(
    selectCommunitiesState,
    fromCommunities.selectCommunitiesErrorMsg
);
export const selectCommunitiesLoading = createSelector(
    selectCommunitiesState,
    fromCommunities.selectCommunitiesLoading
);
export const selectCommunitiesFailed = createSelector(
    selectCommunitiesState,
    fromCommunities.selectCommunitiesFailed
);

//Candidate Profiles selector
export const selectCandidateProfilesState = createFeatureSelector<State, fromCandidateProfiles.State>(fromCandidateProfiles.key);

export const selectCandidateProfiles = createSelector(
    selectCandidateProfilesState,
    fromCandidateProfiles.selectCandidateProfiles
);
export const selectCandidateProfilesErrorMsg = createSelector(
    selectCandidateProfilesState,
    fromCandidateProfiles.selectCandidateProfilesErrorMsg
);
export const selectCandidateProfilesLoading = createSelector(
    selectCandidateProfilesState,
    fromCandidateProfiles.selectCandidateProfilesLoading
);
export const selectCandidateProfilesFailed = createSelector(
    selectCandidateProfilesState,
    fromCandidateProfiles.selectCandidateProfilesFailed
);

//Rooms selector
export const selectRoomsState = createFeatureSelector<State, fromRooms.State>(fromRooms.key);

export const selectRooms = createSelector(
    selectRoomsState,
    fromRooms.selectRooms
);
export const selectRoomsErrorMsg = createSelector(
    selectRoomsState,
    fromRooms.selectRoomsErrorMsg
);
export const selectRoomsLoading = createSelector(
    selectRoomsState,
    fromRooms.selectRoomsLoading
);
export const selectRoomsFailed = createSelector(
    selectRoomsState,
    fromRooms.selectRoomsFailed
);


//Offices selector
export const selectOfficesState = createFeatureSelector<State, fromOffices.State>(fromOffices.key);

export const selectOffices = createSelector(
    selectOfficesState,
    fromOffices.selectOffices
);
export const selectOfficesErrorMsg = createSelector(
    selectOfficesState,
    fromOffices.selectOfficesErrorMsg
);
export const selectOfficesLoading = createSelector(
    selectOfficesState,
    fromOffices.selectOfficesLoading
);
export const selectOfficesFailed = createSelector(
    selectOfficesState,
    fromOffices.selectOfficesFailed
);
//Skills selector
export const selectSkillsState = createFeatureSelector<State, fromSkills.State>(fromSkills.key);

export const selectSkills = createSelector(
    selectSkillsState,
    fromSkills.selectSkills
);
export const selectSkillsErrorMsg = createSelector(
    selectSkillsState,
    fromSkills.selectSkillsErrorMsg
);
export const selectSkillsLoading = createSelector(
    selectSkillsState,
    fromSkills.selectSkillsLoading
);
export const selectSkillsFailed = createSelector(
    selectSkillsState,
    fromSkills.selectSkillsFailed
);

//Skill-types selector
export const selectSkillTypesState = createFeatureSelector<State, fromSkillTypes.State>(fromSkillTypes.key);

export const selectSkillTypes = createSelector(
    selectSkillTypesState,
    fromSkillTypes.selectSkillTypes
);
export const selectSkillTypesErrorMsg = createSelector(
    selectSkillTypesState,
    fromSkillTypes.selectSkillTypesErrorMsg
);
export const selectSkillTypesLoading = createSelector(
    selectSkillTypesState,
    fromSkillTypes.selectSkillTypesLoading
);
export const selectSkillTypesFailed = createSelector(
    selectSkillTypesState,
    fromSkillTypes.selectSkillTypesFailed
);

//Readdress-reasons selector
export const selectReaddressReasonsState = createFeatureSelector<State, fromReaddressReasons.State>(fromReaddressReasons.key);

export const selectReaddressReasons = createSelector(
    selectReaddressReasonsState,
    fromReaddressReasons.selectReaddressReasons
);
export const selectReaddressReasonsErrorMsg = createSelector(
    selectReaddressReasonsState,
    fromReaddressReasons.selectReaddressReasonsErrorMsg
);
export const selectReaddressReasonsLoading = createSelector(
    selectReaddressReasonsState,
    fromReaddressReasons.selectReaddressReasonsLoading
);
export const selectReaddressReasonsFailed = createSelector(
    selectReaddressReasonsState,
    fromReaddressReasons.selectReaddressReasonsFailed
);

//Readdress-reason-types selector
export const selectReaddressReasonTypesState = createFeatureSelector<State, fromReaddressReasonTypes.State>(fromReaddressReasonTypes.key);

export const selectReaddressReasonTypes = createSelector(
    selectReaddressReasonTypesState,
    fromReaddressReasonTypes.selectReaddressReasonTypes
);
export const selectReaddressReasonTypesErrorMsg = createSelector(
    selectReaddressReasonTypesState,
    fromReaddressReasonTypes.selectReaddressReasonTypesErrorMsg
);
export const selectReaddressReasonTypesLoading = createSelector(
    selectReaddressReasonTypesState,
    fromReaddressReasonTypes.selectReaddressReasonTypesLoading
);
export const selectReaddressReasonTypesFailed = createSelector(
    selectReaddressReasonTypesState,
    fromReaddressReasonTypes.selectReaddressReasonTypesFailed
);
