import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { ColumnItem } from '@shared/models/column-item.model';
import { Community } from '@shared/models/community.model';
import { Globals } from '@shared/utils/globals';

@Component({
    selector: 'app-processes-table',
    templateUrl: './processes-table.component.html',
    styleUrls: ['./processes-table.component.scss'],
})

export class ProcessTableComponent implements OnChanges {

    @Input() listOfDisplayData;
    @Input() filteredProcesses;
    @Input() profiles: CandidateProfile[];
    @Input() communities: Community[];
    @Input() statusList;
    @Input() currentStageList;
    @Input() emptyProcess;

    @Output() candidateId = new EventEmitter();
    @Output() userId = new EventEmitter();
    @Output() showProcessStart = new EventEmitter();
    @Output() showDeleteConfirm = new EventEmitter();
    @Output() showApproveProcessConfirm = new EventEmitter();
    @Output() rejectProcess = new EventEmitter();
    @Output() reactivateProcess = new EventEmitter();


    selectedProfiles = [];
    selectedCommunities = [];
    selectedStatus = [];
    selectedStages = [];
    filterParameters = [];
    processesAfterFilter = [];
    listOfDisplayDataAfterFilter = [];
    isFilterVisible: boolean = false;
    seniorityList: any[];
    listOfColumns: ColumnItem[];

    constructor(private globals: Globals ){
        this.seniorityList = globals.seniorityList;
        this.statusList = globals.processStatusList;

    }

    ngOnChanges() {
        if (this.profiles && this.communities) {
            this.communities = this.communities.filter(community => community.name != 'N/A');
            this.statusList = this.statusList.filter(status => status.name != 'N/A');
            this.currentStageList = this.currentStageList.filter(currentStage => currentStage.name != 'N/A');

            this.listOfColumns = [
                {
                    name: 'Candidate'
                },
                {
                    name: 'Profile',
                    listOfFilter: this.profiles.map((value, index) => { return { text: value.name, value: value.name } }),
                    filterFn: (profileNameList: string[], item: any) => profileNameList.some(name => item.candidate.profile.name.indexOf(name) !== -1)
                },
                {
                    name: 'Community',
                    listOfFilter: this.communities.map((value, index) => { return { text: value.name, value: value.name } }),
                    filterFn: (communityNameList: string[], item: any) => communityNameList.some(name => item.candidate.community.name.indexOf(name) !== -1)
                },
                {
                    name: 'Status',
                    listOfFilter: this.statusList.map((value, index) => { return { text: value.name, value: value.id } }),
                    filterFn: (statusIdList: number[], item: any) => statusIdList.some(id => this.getStatus(item.status).indexOf(this.getStatus(id)) !== -1)
                },
                {
                    name: 'Current Stage',
                    listOfFilter: this.currentStageList.map((value, index) => { return { text: value.name, value: value.id } }),
                    filterFn: (currentStageIdList: number[], item: any) => currentStageIdList.some(id => this.getCurrentStage(item.currentStage).indexOf(this.getCurrentStage(id)) !== -1)
                },
                {
                    name: 'Recruiter'
                },
                {
                    name: 'Seniority',
                    listOfFilter: this.seniorityList.map((value, index) => { return { text: value.name, value: value.id } }),
                    filterFn: (seniorityList: number[], item: any) => seniorityList.some(id => item.seniority === id)

                },
                {
                    name: 'Actions'
                },
            ];
        }
    }

    resetFilters(): void {
        this.listOfColumns.forEach(item => {
            switch (item.name) {
                case 'Profile':
                    item.listOfFilter = this.profiles.map((value, index) => { return { text: value.name, value: value.name } })
                    break;
                case 'Community':
                    item.listOfFilter = this.communities.map((value, index) => { return { text: value.name, value: value.name } })
                    break;
                case 'Status':
                    item.listOfFilter = this.statusList.map((value, index) => { return { text: value.name, value: value.id } })
                    break;
                case 'Current Stage':
                    item.listOfFilter = this.currentStageList.map((value, index) => { return { text: value.name, value: value.id } })
                    break;
            }
        });
    }

    trackByName(_: number, item: ColumnItem): string {
        return item.name;
    }


    emitCandidateId(id) {
        this.candidateId.emit(id);
    }

    emitUserId(id) {
        this.userId.emit(id);
    }

    emitEdit(id) {
        this.showProcessStart.emit(id);
    }

    emitDelete(id) {
        this.showDeleteConfirm.emit(id);
    }

    emitApproval(id) {
        this.showApproveProcessConfirm.emit(id);
    }

    emitReject(id) {
        this.rejectProcess.emit(id);
    }

    emitReactivate(id){
        this.reactivateProcess.emit(id);
    }

    getStatus(status: number): string {
        return this.statusList.find(st => st.id === status).name;        
    }

    getCurrentStage(cr: number): string {
        return this.currentStageList.find(st => st.id === cr).name;
    }
    getSeniority(id: number): string {
        return this.seniorityList.find(seniority => seniority.id === id).name;
    }
}
