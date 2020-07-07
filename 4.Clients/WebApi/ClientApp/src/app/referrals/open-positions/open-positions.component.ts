import { Component, EventEmitter, Input, Output, OnInit, OnChanges } from '@angular/core';
import { Globals } from 'src/app/app-globals/globals';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';
import { OpenPosition } from 'src/entities/open-position';
import { ColumnItem } from 'src/entities/ColumnItem';

@Component({
  selector: 'app-open-positions',
  templateUrl: './open-positions.component.html',
  styleUrls: ['./open-positions.component.scss']
})
export class OpenPositionsComponent implements OnInit, OnChanges {  
  @Input() positionsDisplayData = [];
  @Input() communities;
  @Input() filteredPositions;
  @Output() searchCommunity = new EventEmitter();
  @Output() searchPriority = new EventEmitter();
  @Output() showEdit = new EventEmitter();  
  @Output() showDeleteConfirm = new EventEmitter();  
  @Output() position = new EventEmitter<OpenPosition>();
  seniorityList: any[] = [];  
  currentUser: User;
  filterParameters = [];  
  listOfDisplayDataAfterFilter = [];
  selectedCommunities = [];
  listOfColumns: ColumnItem[];
  priorityFilterList: any =  [ { text: 'HOT', value: 'HOT'}, { text: 'NOT HOT', value: 'NOT HOT'} ];

  constructor(private globals: Globals, private facade: FacadeService) {    
    this.seniorityList = globals.seniorityList    
  }
  
  ngOnInit(){
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnChanges() {
    if (this.communities) {
      this.communities = this.communities.filter(community => community.name != 'N/A');

      this.listOfColumns = [
        {
          name: 'Title'
        },
        {
          name: 'Seniority',
          listOfFilter: this.seniorityList.map((value, index) => { return { text: value.name, value: value.id } }),
          filterFn: (seniorityIdList: number[], item: any) => seniorityIdList.some(id => this.getSeniorityName(item.seniority).indexOf(this.getSeniorityName(id)) !== -1)          
        },
        {
          name: 'Studio'
        },
        {
          name: 'Community',
          listOfFilter: this.communities.map((value, index) => { return { text: value.name, value: value.name } }),
          filterFn: (communityNameList: string[], item: any) => communityNameList.some(name => item.community.name.indexOf(name) !== -1)
        },
        {
          name: 'Priority',
          listOfFilter: this.priorityFilterList,
          filterFn: (priorityName: string, item: any) => priorityName.indexOf(this.getPriorityName(item.priority)) !== -1
        },
        {
          name:  this.isUserRole(['Admin', 'HRManagement', 'HRUser', 'Recruiter']) ? 'Actions' : 'Apply your referral'
        }
      ];
    }
  }

  resetFilters(): void {
    this.listOfColumns.forEach(item => {
      switch (item.name) {
        case 'Community':
          item.listOfFilter = this.communities.map((value, index) => { return { text: value.name, value: value.name } })
          break;
        case 'Priority':
          item.listOfFilter = this.priorityFilterList;
          break;
      }
    });
  }

  trackByName(_: number, item: ColumnItem): string {
    return item.name;
  }

  emitCommunityId(id: number) {
    this.searchCommunity.emit(id);
  }

  emitPriority(isHot: number) {
    this.searchPriority.emit(isHot);
  }

  emitPosition(position: OpenPosition) {
    this.position.emit(position);
  }

  getSeniorityName(id: number): string {
    return this.seniorityList.filter(x => x.id === id)[0].name;
  }

  getPriorityName(priority: boolean): string {
    return priority ? 'HOT' : 'NOT HOT';
  }
  emitDelete(id: number){
    this.showDeleteConfirm.emit(id);
  }

  emitEdit(positionToEdit: OpenPosition){
    this.showEdit.emit(positionToEdit);
  }

  isUserRole(roles: string[]): boolean {
    return this.facade.appService.isUserRole(roles);
  }
}
