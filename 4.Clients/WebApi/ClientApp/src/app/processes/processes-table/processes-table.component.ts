import { Component, Input, Output, EventEmitter, ViewChild } from '@angular/core';

@Component({
    selector: 'app-processes-table',
    templateUrl: './processes-table.component.html',
    styleUrls: ['./processes-table.component.scss']
})

export class ProcessTableComponent {
    @Input() listOfDisplayData;
    @Input() profiles;
    @Input() communities;
    @Input() statusList;
    @Input() currentStageList;
    @Input() emptyProcess;

    @Output() candidateId = new EventEmitter();
    @Output() userId = new EventEmitter();
    @Output() searchProfile = new EventEmitter();
    @Output() searchCommunity = new EventEmitter();
    @Output() searchStatus = new EventEmitter();
    @Output() showProcessStart = new EventEmitter();
    @Output() showDeleteConfirm = new EventEmitter();
    @Output() showApproveProcessConfirm = new EventEmitter();
    @Output() rejectProcess = new EventEmitter();
    @Output() profileName = new EventEmitter();
    @Output() searchValueCurrentStage = new EventEmitter();
    @Output() status = new EventEmitter();
    @Output() resetStatus = new EventEmitter();
    @Output() resetCurrentStage = new EventEmitter();

    emitCandidateId(id) {
        this.candidateId.emit(id);
    }

    emitUserId(id) {
        this.userId.emit(id);
    }

    emitProfileId(id) {
        this.searchProfile.emit(id);
    }

    emitCommunityId(id) {
        this.searchCommunity.emit(id);
    }

    emitStatusId(id) {
        this.searchStatus.emit(id);
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

    emitStatus(id) {
        this.status.emit(id);
    }

    emitCurrentStageValue(val) {
        this.searchValueCurrentStage.emit(val);
    }

    getStatus(status: number): string {
        return this.statusList.find(st => st.id === status).name;
    }

    getCurrentStage(cr: number): string {
        return this.currentStageList.find(st => st.id === cr).name;
    }

    emitResetStatus() {
        this.resetStatus.emit();
    }

    emitResetCurrentStage() {
        this.resetCurrentStage.emit();
    }
}
