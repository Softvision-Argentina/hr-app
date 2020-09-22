import { select, Store } from '@ngrx/store';
import { officeActions } from '@shared/store/office/office.actions';

import { selectCommunities, State, selectCandidateProfiles, selectSkills, selectSkillTypes, selectReaddressReasons, selectReaddressReasonTypes,selectRooms,selectOffices } from '../store';
import { communitiesActions } from '../store/communities/communities.actions';
import { candidateProfilesActions } from '../store/candidates-profile/candidates-profile.actions';
import { skillsActions } from '../store/skills/skills.actions';
import { skillTypeActions } from '../store/skill-type/skill-type.actions';
import { readdressReasonActions } from '@shared/store/readdress-reason/readdress-reason.actions';
import { readdressReasonTypesActions } from '@shared/store/readdress-reason-type/readdress-reason-type.actions';
import { roomActions } from '@shared/store/room/room.actions';

export abstract class Sandbox {
    communities$ = this.state$.pipe(
        select(selectCommunities)
    );

    candidateProfiles$ = this.state$.pipe(
        select(selectCandidateProfiles)
    );
    
    offices$ = this.state$.pipe(
        select(selectOffices)
    );

    rooms$ = this.state$.pipe(
        select(selectRooms)
    );

    skills$ = this.state$.pipe(
        select(selectSkills)
    );

    skillTypes$ = this.state$.pipe(
        select(selectSkillTypes)
    );

    readdressReasons$ = this.state$.pipe(
        select(selectReaddressReasons)
    );

    readdressReasonTypes$ = this.state$.pipe(
        select(selectReaddressReasonTypes)
    );

    constructor(
        private state$: Store<State>
    ) { }

    public loadCommunities(): void {
        this.state$.dispatch(communitiesActions.load());
    }

    public loadCandidateProfiles(): void {
        this.state$.dispatch(candidateProfilesActions.load());
    }

    public loadOffices(): void {
        this.state$.dispatch(officeActions.load());
    }

    public loadRooms(): void {
        this.state$.dispatch(roomActions.load());
    }
    public loadSkills(): void {
        this.state$.dispatch(skillsActions.load());
    }

    public loadSkillTypes(): void {
        this.state$.dispatch(skillTypeActions.load());
    }

    public loadReaddressReasons(): void {
        this.state$.dispatch(readdressReasonActions.load());
    }

    public loadReaddressReasonTypes(): void {
        this.state$.dispatch(readdressReasonTypesActions.load());
    }
}
