import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Globals } from 'src/app/app-globals/globals';

@Component({
  selector: 'app-open-positions',
  templateUrl: './open-positions.component.html',
  styleUrls: ['./open-positions.component.scss']
})
export class OpenPositionsComponent {  
  @Input() positionsDisplayData = [];
  @Input() communities;
  seniorityList: any[] = [];  
  
  @Output() searchCommunity = new EventEmitter();
  @Output() searchPriority = new EventEmitter();

  constructor(private globals: Globals) {    
    this.seniorityList = globals.seniorityList
  }

  emitCommunityId(id: number) {
    this.searchCommunity.emit(id);
  }

  emitPriority(isHot: number){
    this.searchPriority.emit(isHot);
  }

  getSeniorityName(id: number) : string{
    return this.seniorityList.filter(x => x.id == id )[0].name;
  }
}
