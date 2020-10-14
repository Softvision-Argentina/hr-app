import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Process } from '@shared/models/process.model';
import { FacadeService } from '@shared/services/facade.service';

const ERROR_STATUS = 'error';
const PROCESS_STATUS = 'process';

@Component({
  selector: 'app-process-steps',
  templateUrl: './process-steps.component.html',
  styleUrls: ['./process-steps.component.scss']
})
export class ProcessStepsComponent implements OnInit {
  
  processID: number = 0;
  process: Process = null;
  processStatus: string = 'process';
  states = ['error','finish','process','wait'];
  currentStagePosition: number = 0;

  selectedView = 'vertical';

  constructor(private route: ActivatedRoute, private facade: FacadeService) { }

  ngOnInit(): void {
    this.processID = this.route.snapshot.params['id'];
    this.getProcessByID(this.processID);
  }

  getProcessByID(id) {
    this.facade.processService.getByID(id)
      .subscribe(res => {
        this.process = res;
      }, err => {
        console.log(err);
      });
    this.facade.appService.stopLoading();
  }

  getStatusColor(status: string) {
    switch(status.toLowerCase()) {
      case "finish":
        return 'green';
      case "process":
        return 'blue'
      case "wait":
        return 'gray';
      case "error":
        return 'red';
      default:
        return 'blue';  
    }
  }

}
