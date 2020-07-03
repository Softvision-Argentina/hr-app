import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Globals } from 'src/app/app-globals/globals';
import { ColumnItem } from 'src/entities/ColumnItem';

@Component({
  selector: 'app-open-positions',
  templateUrl: './open-positions.component.html',
  styleUrls: ['./open-positions.component.scss']
})
export class OpenPositionsComponent {
  @Input() positionsDisplayData = [];
  @Input() communities;
  @Output() searchCommunity = new EventEmitter();
  @Output() searchPriority = new EventEmitter();
  
  seniorityList: any[] = [];
  listOfColumns: ColumnItem[];
  priorityFilterList: any =  [ { text: 'HOT', value: 'HOT'}, { text: 'NOT HOT', value: 'NOT HOT'} ];
  
  constructor(private globals: Globals) {
    this.seniorityList = globals.seniorityList;
  }

  ngOnChanges() {
    if (this.communities) {
      this.communities = this.communities.filter(community => community.name != 'N/A');

      this.listOfColumns = [
        {
          name: 'Title'
        },
        {
          name: 'Seniority'
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
          name: 'Apply your referral'
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

  getSeniorityName(id: number): string {
    return this.seniorityList.filter(x => x.id == id)[0].name;
  }

  getPriorityName(priority: boolean): string {
    return priority ? 'HOT' : 'NOT HOT';
  }
}
