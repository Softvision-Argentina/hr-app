import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { Process } from 'src/entities/process';
import { ChangeDetectionStrategy } from '@angular/core';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';

@Component({
  selector: 'app-report-progress-processes',
  templateUrl: './report-progress-processes.component.html',
  styleUrls: ['./report-progress-processes.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})


export class ReportProgressProcessesComponent implements OnInit, OnChanges {

  @Input()
  private _detailedProcesses: Process[];
  public get detailedProcesses(): Process[] {
    return this._detailedProcesses;
  }
  public set detailedProcesses(value: Process[]) {
    this._detailedProcesses = value;
  }

  constructor() { }

  processInProgress = 0;
  processNotStarted = 0;

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    // tslint:disable-next-line: no-unused-expression
    changes._detailedProcesses;
    this.complete(this._detailedProcesses);
    this.haveProcesses();
  }

  complete(process: Process[]) {
    this.processInProgress = process.filter(p => p.status === ProcessStatusEnum.InProgress ||
      p.status === ProcessStatusEnum.OfferAccepted || p.status === ProcessStatusEnum.Recall).length;
    this.processNotStarted = process.filter(p => p.status === ProcessStatusEnum.Declined ||
      p.status === ProcessStatusEnum.Hired || p.status === ProcessStatusEnum.Rejected).length;
  }


  haveProcesses(): boolean {
    const total = this.processInProgress + this.processNotStarted;
    if (total > 0) { return true; } else { return false; }
  }
}
