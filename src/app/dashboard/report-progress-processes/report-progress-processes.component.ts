import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { Process } from 'src/entities/process';
import { ChangeDetectionStrategy } from '@angular/core';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';

@Component({
  selector: 'app-report-progress-processes',
  templateUrl: './report-progress-processes.component.html',
  styleUrls: ['./report-progress-processes.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})


export class ReportProgressProcessesComponent implements OnInit {

  @Input()
  private _detailedProcesses: Process[];
  public get detailedProcesses(): Process[] {
    return this._detailedProcesses;
  }
  public set detailedProcesses(value: Process[]) {
    this._detailedProcesses = value;
  }

  constructor() { }

  processInProgress: number = 0;
  processNotStarted: number = 0;

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._detailedProcesses;
    this.complete(this._detailedProcesses);
    this.haveProcesses();
  }

  complete(process: Process[]) {
    this.processInProgress = process.filter(process => process.status === ProcessStatusEnum.InProgress ||
      process.status === ProcessStatusEnum.Accepted || process.status === ProcessStatusEnum.Recall).length;
    this.processNotStarted = process.filter(process => process.status === ProcessStatusEnum.Declined ||
      process.status === ProcessStatusEnum.Hired || process.status === ProcessStatusEnum.Rejected).length;
  }


  haveProcesses(): boolean {
    let total = this.processInProgress + this.processNotStarted;
    if (total > 0) return true;
    else return false;
  }
}
