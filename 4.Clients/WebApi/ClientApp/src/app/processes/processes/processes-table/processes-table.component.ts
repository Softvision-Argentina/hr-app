import { Component, OnInit, Input, Output } from '@angular/core'
import { EventEmitter } from 'protractor';

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

  profileId;

  //@Output() candidateId = new EventEmitter();

  ngOnInit() {
    
  }

  //emitCandidateId(val){
  //  this.candidateId.emit(val)
  //} 
  
}
