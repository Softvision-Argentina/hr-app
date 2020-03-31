import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core'

@Component({
  selector: 'app-processes-table',
  templateUrl: './processes-table.component.html',
  styleUrls: ['./processes-table.component.css']
})

export class ProcessTableComponent implements OnInit {
  @Input() listOfDisplayData;
  @Input() profiles;
  @Input() communities;
  @Input() statusList;
  @Input() currentStageList;
  @Input() emptyProcess;

  @ViewChild('dropdownCurrentStage') currentStageDropdown;

  @Output() candidateId = new EventEmitter();
  @Output() recruiterId = new EventEmitter();
  @Output() searchProfile = new EventEmitter();
  @Output() searchCommunity = new EventEmitter();
  @Output() searchStatus = new EventEmitter();
  @Output() showProcessStart = new EventEmitter();
  @Output() showDeleteConfirm = new EventEmitter();
  @Output() showApproveProcessConfirm = new EventEmitter();
  @Output() rejectProcess = new EventEmitter();
  @Output() profileName = new EventEmitter()
  @Output() searchValueCurrentStage = new EventEmitter()
  @Output() status = new EventEmitter()
  @Output() recruiterValue = new EventEmitter()
  @Output() resetRecruiter = new EventEmitter()
  @Output() candidateValue = new EventEmitter()
  @Output() resetCandidate = new EventEmitter()
  @Output() resetStatus = new EventEmitter()

  profile = '';
  CurrentStageValue = '';
  searchRecruiterValue = '';
  searchValue = '';

  ngOnInit() {

  }

  emitCandidateId(id) {
    this.candidateId.emit(id)
  }

  emitRecruiterId(id) {
    this.recruiterId.emit(id)
  }

  emitProfileId(id) {
    this.searchProfile.emit(id)
  }

  emitCommunityId(id) {
    this.searchCommunity.emit(id)
  }

  emitStatusId(id) {
    this.searchStatus.emit(id)
  }

  emitEdit(id) {
    this.showProcessStart.emit(id)
  }

  emitDelete(id) {
    this.showDeleteConfirm.emit(id)
  }

  emitApproval(id) {
    this.showApproveProcessConfirm.emit(id)
  }

  emitReject(id) {
    this.rejectProcess.emit(id)
  }

  emitProfileName(name) {
    this.profileName.emit(name);
    this.profile = ''
  }

  emitStatus(id) {
    console.log({ "status id": id, "status name": this.statusList[id].name })
    this.status.emit(id)
  }

  emitCurrentStageValue(val) {
    this.searchValueCurrentStage.emit(val);
  }

  emitRecruiterValue() {
    this.recruiterValue.emit(this.searchRecruiterValue)
  }

  emitResetRecruiter() {
    this.resetRecruiter.emit()
  }

  emitCandidateValue() {
    this.candidateValue.emit(this.searchValue)
  }

  emitResetCandidate() {
    this.resetCandidate.emit()
  }

  getStatus(status: number): string {
    return this.statusList.find(st => st.id === status).name;
  }

  getCurrentStage(cr: number): string {
    return this.currentStageList.find(st => st.id === cr).name;
  }

  emitResetStatus() {
    this.resetStatus.emit()
  }

}
