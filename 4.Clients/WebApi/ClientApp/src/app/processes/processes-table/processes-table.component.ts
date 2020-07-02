import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-processes-table',
    templateUrl: './processes-table.component.html',
    styleUrls: ['./processes-table.component.scss'],
})

export class ProcessTableComponent {

    @Input() listOfDisplayData;
    @Input() filteredProcesses;
    @Input() profiles;
    @Input() communities;
    @Input() statusList;
    @Input() currentStageList;
    @Input() emptyProcess;

    @Output() candidateId = new EventEmitter();
    @Output() userId = new EventEmitter();
    @Output() showProcessStart = new EventEmitter();
    @Output() showDeleteConfirm = new EventEmitter();
    @Output() showApproveProcessConfirm = new EventEmitter();
    @Output() rejectProcess = new EventEmitter();

    selectedProfiles = [];
    selectedCommunities = [];
    selectedStatus = [];
    selectedStages = [];
    filterParameters = [];
    processesAfterFilter = [];
    listOfDisplayDataAfterFilter = [];

    constructor() {}

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

    getStatus(status: number): string {
        return this.statusList.find(st => st.id === status).name;
    }

    getCurrentStage(cr: number): string {
        return this.currentStageList.find(st => st.id === cr).name;
    }

    selectedProfileOption(id) {
        return this.selectedProfiles.indexOf(id) < 0 ? false : true;
    }

    selectedCommunityOption(id) {
        return this.selectedCommunities.indexOf(id) < 0 ? false : true;
    }

    selectedStatusOption(id) {
        return this.selectedStatus.indexOf(id) < 0 ? false : true;
    }

    selectedStagesOption(id) {
        return this.selectedStages.indexOf(id) < 0 ? false : true;
    }

    pushProfileIdToSelectedProfiles(id) {
        this.selectedProfiles.indexOf(id) < 0 ? this.selectedProfiles.push(id) : this.selectedProfiles.splice(this.selectedProfiles.indexOf(id), 1);
    }

    pushCommunityIdToSelectedCommunities(id) {
        this.selectedCommunities.indexOf(id) < 0 ? this.selectedCommunities.push(id) : this.selectedCommunities.splice(this.selectedCommunities.indexOf(id), 1);
    }

    pushStatusIdToSelectedStatus(id) {
        this.selectedStatus.indexOf(id) < 0 ? this.selectedStatus.push(id) : this.selectedStatus.splice(this.selectedProfiles.indexOf(id), 1);
    }

    pushStagesIdToSelectedStages(id) {
        this.selectedStages.indexOf(id) < 0 ? this.selectedStages.push(id) : this.selectedStages.splice(this.selectedStages.indexOf(id), 1);
    }


    resetFilter() {
        this.filterParameters = [];
        this.listOfDisplayDataAfterFilter = [];
    }

    setProfileFilter(idsArray) {
        this.selectedProfiles = [];
        this.setProfileFilterParameters(idsArray);
        this.setFilter(idsArray);
    }

    setCommunityFilter(idsArray) {
        this.selectedCommunities = [];
        this.setCommunityFilterParameters(idsArray);
        this.setFilter(idsArray);
    }

    setStatusFilter(id) {
        this.selectedStatus = [];
        this.setStatusFilterParameters(id);
        this.setFilter(id);
    }

    setCurrentStageFilter(id) {
        this.selectedStages = [];
        this.setCurrentStageFilterParameters(id);
        this.setFilter(id);
    }

    setProfileFilterParameters(arr) {
        if (this.filterParameters.length === 0) {
            this.filterParameters.push({ 'profile': arr });
        } else if (this.filterParameters.length > 0) {
            if (this.filterParameters[this.filterParameters.length - 1]['profile']) {
                arr.forEach(item => {
                    this.filterParameters[this.filterParameters.length - 1]['profile'].push(item);
                });
            } else {
                this.filterParameters.push({ 'profile': arr });
            }
        }
    }

    setCommunityFilterParameters(arr) {
        if (this.filterParameters.length === 0) {
            this.filterParameters.push({ 'community': arr });
        } else if (this.filterParameters.length > 0) {
            if (this.filterParameters[this.filterParameters.length - 1]['community']) {
                arr.forEach(item => {
                    this.filterParameters[this.filterParameters.length - 1]['community'].push(item);
                });
            } else {
                this.filterParameters.push({ 'community': arr });
            }
        }
    }

    setStatusFilterParameters(arr) {
        if (this.filterParameters.length === 0) {
            this.filterParameters.push({ 'status': arr });
        } else if (this.filterParameters.length > 0) {
            if (this.filterParameters[this.filterParameters.length - 1]['status']) {
                arr.forEach(item => {
                    this.filterParameters[this.filterParameters.length - 1]['status'].push(item);
                });
            } else {
                this.filterParameters.push({ 'status': arr });
            }
        }
    }

    setCurrentStageFilterParameters(arr) {
        if (this.filterParameters.length === 0) {
            this.filterParameters.push({ 'currentStage': arr });
        } else if (this.filterParameters.length > 0) {
            if (this.filterParameters[this.filterParameters.length - 1]['currentStage']) {
                arr.forEach(item => {
                    this.filterParameters[this.filterParameters.length - 1]['currentStage'].push(item);
                });
            } else {
                this.filterParameters.push({ 'currentStage': arr });
            }
        }
    }

    checkProfile(id) {
        let counter = 0;
        if (this.filterParameters.length === 0) {
            this.filteredProcesses.some(process => {
                process.candidate.profile.id === id ? counter++ : null;
            });
        } else if (this.filterParameters.length === 1) {
            const filterParameter = Object.keys(this.filterParameters[0]);
            if (filterParameter[0] === 'profile') {
                this.filteredProcesses.some(process => {
                    process.candidate.profile.id === id ? counter++ : null;
                });
            } else {
                this.listOfDisplayDataAfterFilter[0].some(process => {
                    process.candidate.profile.id === id ? counter++ : null;
                });
            }
        } else if (this.filterParameters.length === 2) {
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            if (filterParametersSecondElementKey[0] === 'profile') {
                this.listOfDisplayDataAfterFilter[0].some(process => {
                    process.candidate.profile.id === id ? counter++ : null;
                });
            } else {
                if (filterParametersFirstElementKey[0] === 'profile') {
                    const lastElementLength = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1].length;
                    if (this.listOfDisplayDataAfterFilter[0].length === lastElementLength) {
                        this.filteredProcesses.some(process => {
                            process.candidate.profile.id === id ? counter++ : null;
                        });
                    }
                }

                this.listOfDisplayDataAfterFilter[1].some(process => {
                    process.candidate.profile.id === id ? counter++ : null;
                });
            }
        } else if (this.filterParameters.length === 3) {
            const filterParameter = Object.keys(this.filterParameters[2]);
            if (filterParameter[0] === 'profile') {
                this.listOfDisplayDataAfterFilter[1].some(process => {
                    process.candidate.profile.id === id ? counter++ : null;
                });
            } else {
                const lastElement = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1];
                let processCounter = 0;
                lastElement.forEach(lastElementProcess => {
                    this.listOfDisplayDataAfterFilter[1].forEach(process => {
                        lastElementProcess.id === process.id ? processCounter++ : null;
                    });
                });

                if (this.listOfDisplayDataAfterFilter[1].length === processCounter) {
                    this.listOfDisplayDataAfterFilter[0].some(process => {
                        process.candidate.profile.id === id ? counter++ : null;
                    });
                } else {
                    lastElement.some(process => {
                        process.candidate.profile.id === id ? counter++ : null;
                    });
                }
            }
        } else if (this.filterParameters.length === 4) {
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            const filterParametersLastElementKey = Object.keys(this.filterParameters[3]);

            if (filterParametersLastElementKey[0] === 'profile') {


                if (filterParametersSecondElementKey[0] === 'profile') {
                    this.listOfDisplayDataAfterFilter[0].some(process => {
                        process.candidate.profile.id === id ? counter++ : null;
                    });
                }

                this.listOfDisplayDataAfterFilter[2].some(process => {
                    process.candidate.profile.id === id ? counter++ : null;
                });
            } else {
                if (filterParametersFirstElementKey[0] === 'profile') {
                    if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                        this.filteredProcesses.some(process => {
                            process.candidate.profile.id === id ? counter++ : null;
                        });
                    }
                } else {
                    if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                        this.listOfDisplayDataAfterFilter[0].some(process => {
                            process.candidate.profile.id === id ? counter++ : null;
                        });
                    }
                }

                const lastElement = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1];
                let processCounter = 0;
                lastElement.forEach(lastElementProcess => {
                    this.listOfDisplayDataAfterFilter[2].forEach(process => {
                        lastElementProcess.id === process.id ? processCounter++ : null;
                    });
                });

                if (this.listOfDisplayDataAfterFilter[2].length === processCounter) {
                    this.listOfDisplayDataAfterFilter[1].some(process => {
                        process.candidate.profile.id === id ? counter++ : null;
                    });
                } else {
                    lastElement.some(process => {
                        process.candidate.profile.id === id ? counter++ : null;
                    });
                }
            }
        } else {
            this.filteredProcesses.some(process => {
                process.candidate.profile.id === id ? counter++ : null;
            });
        }
        return counter > 0 ? true : false;
    }

    checkCommunity(id) {
        let counter = 0;
        if (this.filterParameters.length === 0) {
            this.filteredProcesses.some(process => {
                process.candidate.community.id === id ? counter++ : null;
            });
        } else if (this.filterParameters.length === 1) {
            const filterParameter = Object.keys(this.filterParameters[0]);
            if (filterParameter[0] === 'community') {
                this.filteredProcesses.some(process => {
                    process.candidate.community.id === id ? counter++ : null;
                });
            } else {
                this.listOfDisplayDataAfterFilter[0].some(process => {
                    process.candidate.community.id === id ? counter++ : null;
                });
            }
        } else if (this.filterParameters.length === 2) {
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            if (filterParametersSecondElementKey[0] === 'community') {
                this.listOfDisplayDataAfterFilter[0].some(process => {
                    process.candidate.community.id === id ? counter++ : null;
                });
            } else {
                if (filterParametersFirstElementKey[0] === 'community') {
                    const lastElementLength = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1].length;
                    if (this.listOfDisplayDataAfterFilter[0].length === lastElementLength) {
                        this.filteredProcesses.some(process => {
                            process.candidate.community.id === id ? counter++ : null;
                        });
                    }
                }

                this.listOfDisplayDataAfterFilter[1].some(process => {
                    process.candidate.community.id === id ? counter++ : null;
                });
            }
        } else if (this.filterParameters.length === 3) {
            const filterParameter = Object.keys(this.filterParameters[2]);
            if (filterParameter[0] === 'community') {
                this.listOfDisplayDataAfterFilter[1].some(process => {
                    process.candidate.community.id === id ? counter++ : null;
                });
            } else {

                const lastElement = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1];
                let processCounter = 0;
                lastElement.forEach(lastElementProcess => {
                    this.listOfDisplayDataAfterFilter[1].forEach(process => {
                        lastElementProcess.id === process.id ? processCounter++ : null;
                    });
                });

                if (this.listOfDisplayDataAfterFilter[1].length === processCounter) {
                    this.listOfDisplayDataAfterFilter[0].some(process => {
                        process.candidate.community.id === id ? counter++ : null;
                    });
                } else {
                    lastElement.some(process => {
                        process.candidate.community.id === id ? counter++ : null;
                    });
                }
            }
        } else if (this.filterParameters.length === 4) {
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            const filterParametersLastElementKey = Object.keys(this.filterParameters[3]);

            if (filterParametersLastElementKey[0] === 'community') {

                if (filterParametersFirstElementKey[0] === 'community') {
                    this.listOfDisplayDataAfterFilter[0].some(process => {
                        process.candidate.community.id === id ? counter++ : null;
                    });
                }

                this.listOfDisplayDataAfterFilter[2].some(process => {
                    process.candidate.community.id === id ? counter++ : null;
                });
            } else {
                if (filterParametersFirstElementKey[0] === 'community') {
                    if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                        this.filteredProcesses.some(process => {
                            process.candidate.community.id === id ? counter++ : null;
                        });
                    }
                } else {
                    if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                        this.listOfDisplayDataAfterFilter[0].some(process => {
                            process.candidate.community.id === id ? counter++ : null;
                        });
                    }
                }

                const lastElement = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1];
                let processCounter = 0;
                lastElement.forEach(lastElementProcess => {
                    this.listOfDisplayDataAfterFilter[2].forEach(process => {
                        lastElementProcess.id === process.id ? processCounter++ : null;
                    });
                });

                if (this.listOfDisplayDataAfterFilter[2].length === processCounter) {
                    this.listOfDisplayDataAfterFilter[1].some(process => {
                        process.candidate.community.id === id ? counter++ : null;
                    });
                } else {
                    lastElement.some(process => {
                        process.candidate.community.id === id ? counter++ : null;
                    });
                }
            }
        } else {
            this.filteredProcesses.some(process => {
                process.candidate.community.id === id ? counter++ : null;
            });
        }
        return counter > 0 ? true : false;
    }

    checkStatus(id) {
        let counter = 0;
        if (this.filterParameters.length === 0) {
            this.filteredProcesses.some(process => {
                process.status === id ? counter++ : null;
            });
        } else if (this.filterParameters.length === 1) {
            const filterParameter = Object.keys(this.filterParameters[0]);
            if (filterParameter[0] === 'status') {
                this.filteredProcesses.some(process => {
                    process.status === id ? counter++ : null;
                });
            } else {
                this.listOfDisplayDataAfterFilter[0].some(process => {
                    process.status === id ? counter++ : null;
                });
            }
        } else if (this.filterParameters.length === 2) {
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            if (filterParametersSecondElementKey[0] === 'status') {
                this.listOfDisplayDataAfterFilter[0].some(process => {
                    process.status === id ? counter++ : null;
                });
            } else {
                if (filterParametersFirstElementKey[0] === 'status') {
                    const lastElementLength = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1].length;
                    if (this.listOfDisplayDataAfterFilter[0].length === lastElementLength) {
                        this.filteredProcesses.some(process => {
                            process.status === id ? counter++ : null;
                        });
                    }
                }

                this.listOfDisplayDataAfterFilter[1].some(process => {
                    process.status === id ? counter++ : null;
                });
            }
        } else if (this.filterParameters.length === 3) {
            const filterParameter = Object.keys(this.filterParameters[2]);
            if (filterParameter[0] === 'status') {
                this.listOfDisplayDataAfterFilter[1].some(process => {
                    process.status === id ? counter++ : null;
                });
            } else {
                const lastElement = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1];
                let processCounter = 0;
                lastElement.forEach(lastElementProcess => {
                    this.listOfDisplayDataAfterFilter[1].forEach(process => {
                        lastElementProcess.id === process.id ? processCounter++ : null;
                    });
                });

                if (this.listOfDisplayDataAfterFilter[1].length === processCounter) {
                    this.listOfDisplayDataAfterFilter[0].some(process => {
                        process.status === id ? counter++ : null;
                    });
                } else {
                    lastElement.some(process => {
                        process.status === id ? counter++ : null;
                    });
                }
            }
        } else if (this.filterParameters.length === 4) {
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            const filterParametersLastElementKey = Object.keys(this.filterParameters[3]);

            if (filterParametersLastElementKey[0] === 'status') {
                if (filterParametersSecondElementKey[0] === 'status') {
                    this.listOfDisplayDataAfterFilter[0].some(process => {
                        process.status === id ? counter++ : null;
                    });
                }

                this.listOfDisplayDataAfterFilter[2].some(process => {
                    process.status === id ? counter++ : null;
                });
            } else {
                if (filterParametersFirstElementKey[0] === 'status') {
                    if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                        this.filteredProcesses.some(process => {
                            process.status === id ? counter++ : null;
                        });
                    }
                } else {
                    if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                        this.listOfDisplayDataAfterFilter[0].some(process => {
                            process.status === id ? counter++ : null;
                        });
                    }
                }
                const lastElement = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1];
                let processCounter = 0;
                lastElement.forEach(lastElementProcess => {
                    this.listOfDisplayDataAfterFilter[2].forEach(process => {
                        lastElementProcess.id === process.id ? processCounter++ : null;
                    });
                });

                if (this.listOfDisplayDataAfterFilter[2].length === processCounter) {
                    this.listOfDisplayDataAfterFilter[1].some(process => {
                        process.status === id ? counter++ : null;
                    });
                } else {
                    lastElement.some(process => {
                        process.status === id ? counter++ : null;
                    });
                }
            }
        } else {
            this.filteredProcesses.some(process => {
                process.status === id ? counter++ : null;
            });
        }
        return counter > 0 ? true : false;
    }

    checkCurrentStage(id) {
        let counter = 0;
        if (this.filterParameters.length === 0) {
            this.filteredProcesses.some(process => {
                process.currentStage === id ? counter++ : null;
            });
        } else if (this.filterParameters.length === 1) {
            const filterParameter = Object.keys(this.filterParameters[0]);
            if (filterParameter[0] === 'currentStage') {
                this.filteredProcesses.some(process => {
                    process.currentStage === id ? counter++ : null;
                });
            } else {
                this.listOfDisplayDataAfterFilter[0].some(process => {
                    process.currentStage === id ? counter++ : null;
                });
            }
        } else if (this.filterParameters.length === 2) {
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            if (filterParametersSecondElementKey[0] === 'currentStage') {
                this.listOfDisplayDataAfterFilter[0].some(process => {
                    process.currentStage === id ? counter++ : null;
                });
            } else {
                if (filterParametersFirstElementKey[0] === 'currentStage') {
                    const lastElementLength = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1].length;
                    if (this.listOfDisplayDataAfterFilter[0].length === lastElementLength) {
                        this.filteredProcesses.some(process => {
                            process.currentStage === id ? counter++ : null;
                        });
                    }
                }

                this.listOfDisplayDataAfterFilter[1].some(process => {
                    process.currentStage === id ? counter++ : null;
                });
            }
        } else if (this.filterParameters.length === 3) {
            const filterParameter = Object.keys(this.filterParameters[2]);
            if (filterParameter[0] === 'currentStage') {
                this.listOfDisplayDataAfterFilter[1].some(process => {
                    process.currentStage === id ? counter++ : null;
                });
            } else {
                const lastElement = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1];
                let processCounter = 0;
                lastElement.forEach(lastElementProcess => {
                    this.listOfDisplayDataAfterFilter[1].forEach(process => {
                        lastElementProcess.id === process.id ? processCounter++ : null;
                    });
                });

                if (this.listOfDisplayDataAfterFilter[1].length === processCounter) {
                    this.listOfDisplayDataAfterFilter[0].some(process => {
                        process.currentStage === id ? counter++ : null;
                    });
                } else {
                    lastElement.some(process => {
                        process.currentStage === id ? counter++ : null;
                    });
                }
            }
        } else if (this.filterParameters.length === 4) {
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            const filterParametersLastElementKey = Object.keys(this.filterParameters[3]);

            if (filterParametersLastElementKey[0] === 'currentStage') {

                if (filterParametersSecondElementKey[0] === 'currentStage') {
                    this.listOfDisplayDataAfterFilter[0].some(process => {
                        process.currentStage === id ? counter++ : null;
                    });
                }

                this.listOfDisplayDataAfterFilter[2].some(process => {
                    process.currentStage === id ? counter++ : null;
                });
            } else {
                if (filterParametersFirstElementKey[0] === 'currentStage') {
                    if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                        this.filteredProcesses.some(process => {
                            process.currentStage === id ? counter++ : null;
                        });
                    }
                } else {
                    if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                        this.listOfDisplayDataAfterFilter[0].some(process => {
                            process.currentStage === id ? counter++ : null;
                        });
                    }
                }

                const lastElement = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1];
                let processCounter = 0;
                lastElement.forEach(lastElementProcess => {
                    this.listOfDisplayDataAfterFilter[2].forEach(process => {
                        lastElementProcess.id === process.id ? processCounter++ : null;
                    });
                });

                if (this.listOfDisplayDataAfterFilter[2].length === processCounter) {
                    this.listOfDisplayDataAfterFilter[1].some(process => {
                        process.currentStage === id ? counter++ : null;
                    });
                } else {
                    lastElement.some(process => {
                        process.currentStage === id ? counter++ : null;
                    });
                }
            }
        } else {
            this.filteredProcesses.some(process => {
                process.currentStage === id ? counter++ : null;
            });
        }

        return counter > 0 ? true : false;
    }

    setFilterColumnsArray(profileId: number, communityId: number, statusId: number, currentStageId: number) {
        const columns = [
            { name: 'profile', id: profileId },
            { name: 'community', id: communityId },
            { name: 'status', id: statusId },
            { name: 'currentStage', id: currentStageId },
        ];
        return columns;
    }

    checkFilterParameter(filterParametersKey: string, parameter: string, parameterId: number, process: {}, idsArray: [], resultArray: {}[]) {
        if (filterParametersKey === parameter) {
            idsArray.forEach(id => {
                parameterId === id ? resultArray.push(process) : null;
            });
        }
    }

    setFilter(idsArray) {
        if (this.filterParameters.length === 1) {
            const listOfDisplayedProcesses = [];
            this.filteredProcesses.forEach(process => {
                const filterParametersKey = Object.keys(this.filterParameters[0]);

                const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                filterColumns.forEach(column => {
                    this.checkFilterParameter(filterParametersKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                });
            });
            this.listOfDisplayDataAfterFilter[0] = listOfDisplayedProcesses;
        } else if (this.filterParameters.length === 2) {
            const listOfDisplayedProcesses = [];
            this.listOfDisplayDataAfterFilter[0].forEach(process => {
                const filterParametersKey = Object.keys(this.filterParameters[this.filterParameters.length - 1]);

                const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                filterColumns.forEach(column => {
                    this.checkFilterParameter(filterParametersKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                });
            });
            this.listOfDisplayDataAfterFilter[1] = listOfDisplayedProcesses;
        } else if (this.filterParameters.length === 3) {
            const listOfDisplayedProcesses = [];
            const filterParametersLastElementKey = Object.keys(this.filterParameters[this.filterParameters.length - 1]);
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            const lastElementLength = this.listOfDisplayDataAfterFilter[this.listOfDisplayDataAfterFilter.length - 1].length;

            if (filterParametersLastElementKey[0] === filterParametersFirstElementKey[0]) {
                if (lastElementLength === this.listOfDisplayDataAfterFilter[0].length) {
                    this.filteredProcesses.forEach(process => {

                        const filterColumnsArray = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                        filterColumnsArray.forEach(column => {
                            this.checkFilterParameter(filterParametersLastElementKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                        });
                    });
                } else {
                    this.listOfDisplayDataAfterFilter[1].forEach(process => {
                        const filterColumnsArray = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                        filterColumnsArray.forEach(column => {
                            this.checkFilterParameter(filterParametersLastElementKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                        });
                    });
                }
                this.listOfDisplayDataAfterFilter.push(listOfDisplayedProcesses);
            } else {
                if (filterParametersLastElementKey[0] === filterParametersFirstElementKey[0]) {
                    this.filteredProcesses.forEach(process => {

                        const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                        filterColumns.forEach(column => {
                            this.checkFilterParameter(filterParametersLastElementKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                        });
                    });

                    this.listOfDisplayDataAfterFilter.push(listOfDisplayedProcesses);
                } else if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                    this.listOfDisplayDataAfterFilter[0].forEach(process => {

                        const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                        filterColumns.forEach(column => {
                            this.checkFilterParameter(filterParametersLastElementKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                        });
                    });
                    this.listOfDisplayDataAfterFilter.push(listOfDisplayedProcesses);
                } else {
                    this.listOfDisplayDataAfterFilter[1].forEach(process => {
                        const filterParametersKey = Object.keys(this.filterParameters[this.filterParameters.length - 1]);

                        const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                        filterColumns.forEach(column => {
                            this.checkFilterParameter(filterParametersKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                        });
                    });

                    this.listOfDisplayDataAfterFilter.push(listOfDisplayedProcesses);
                }
            }
        } else {
            const listOfDisplayedProcesses = [];
            const filterParametersLastElementKey = Object.keys(this.filterParameters[this.filterParameters.length - 1]);
            const filterParametersFirstElementKey = Object.keys(this.filterParameters[0]);
            const filterParametersSecondElementKey = Object.keys(this.filterParameters[1]);
            const filterParametersThirdElementKey = Object.keys(this.filterParameters[2]);

            if (filterParametersLastElementKey[0] === filterParametersFirstElementKey[0]) {
                this.filteredProcesses.forEach(process => {

                    const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                    filterColumns.forEach(column => {
                        this.checkFilterParameter(filterParametersLastElementKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                    });
                });

                this.listOfDisplayDataAfterFilter.push(listOfDisplayedProcesses);
            } else if (filterParametersLastElementKey[0] === filterParametersSecondElementKey[0]) {
                this.listOfDisplayDataAfterFilter[0].forEach(process => {

                    const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                    filterColumns.forEach(column => {
                        this.checkFilterParameter(filterParametersLastElementKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                    });
                });
                this.listOfDisplayDataAfterFilter.push(listOfDisplayedProcesses);
            } else if (filterParametersLastElementKey[0] === filterParametersThirdElementKey[0]) {
                this.listOfDisplayDataAfterFilter[1].forEach(process => {

                    const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                    filterColumns.forEach(column => {
                        this.checkFilterParameter(filterParametersLastElementKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                    });
                });
                this.listOfDisplayDataAfterFilter.push(listOfDisplayedProcesses);
            } else {
                this.listOfDisplayDataAfterFilter[2].forEach(process => {
                    const filterParametersKey = Object.keys(this.filterParameters[this.filterParameters.length - 1]);

                    const filterColumns = this.setFilterColumnsArray(process.candidate.profile.id, process.candidate.community.id, process.status, process.currentStage);

                    filterColumns.forEach(column => {
                        this.checkFilterParameter(filterParametersKey[0], column.name, column.id, process, idsArray, listOfDisplayedProcesses);
                    });
                });
                this.listOfDisplayDataAfterFilter.push(listOfDisplayedProcesses);
            }
        }
    }
}
