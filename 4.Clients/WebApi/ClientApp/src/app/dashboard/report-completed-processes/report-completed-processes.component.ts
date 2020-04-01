import { Component, OnInit, TemplateRef, ViewChild, Input, SimpleChanges, OnChanges } from '@angular/core';
import { SingleDataSet, Label } from 'ng2-charts';
import { AppComponent } from 'src/app/app.component';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';


@Component({
  selector: 'app-report-completed-processes',
  templateUrl: './report-completed-processes.component.html',
  styleUrls: ['./report-completed-processes.component.css']
})
export class ReportCompletedProcessesComponent implements OnInit, OnChanges {

  @Input() _processes;

  processCompleted = 0;
  processInProgress = 0;
  processNotStarted = 0;
  processFinishedSuccess = 0;
  completedProcessLoading = true;
  public pieChartLabels: Label[] = ['IN PROGRESS', 'NOT STARTED'];
  public pieChartData: SingleDataSet = [0, 0, 0];
  public pieChartColors: Array<any> = [
    {
      backgroundColor: ['#81FB15', '#F6FB15', '#6FC8CE']
    }
  ];
  pieChartLoading = true;


  stadisticFinished = 0;
  stadisticFailed = 0;
  isChartComplete = false;

  constructor(private app: AppComponent) { }

  ngOnInit() {
    this.complete();
    this.app.showLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    // tslint:disable-next-line: no-unused-expression
    changes._processes;
    this.complete();
  }


  getProgressPercentage() {
    const totalCandidates: number = this.processCompleted;
    if (totalCandidates > 0) {
      this.stadisticFinished = (this.processFinishedSuccess * 100) / 2;
      // tslint:disable-next-line: max-line-length
      if (this.stadisticFinished === 100) { this.stadisticFailed = 0; } else { this.stadisticFailed = ((totalCandidates - this.processFinishedSuccess) * 100) / totalCandidates; }
      if (this.stadisticFailed === 100) { this.stadisticFinished = 0; }
      this.isChartComplete = true;
    }

    const labels: string[] = [];
    const percentages: number[] = [];
    let colors: string[] = [];

    if (this.processInProgress > 0) {
      labels.push('IN PROGRESS');
      colors.push('#81FB15');
      percentages.push(this.processInProgress);
    }
    if (this.processNotStarted > 0) {
      labels.push('NOT STARTED');
      colors.push('#6FC8CE');
      percentages.push(this.processNotStarted);
    }
    this.pieChartLabels = labels;
    this.pieChartData = percentages;
    if (colors.length === 0) { colors = ['#81FB15', '#F6FB15', '#6FC8CE']; }
    this.pieChartColors = [
      {
        backgroundColor: colors
      }
    ];
    this.pieChartLoading = false;
    this.completedProcessLoading = false;
    this.app.hideLoading();
  }

  complete() {
    this.processCompleted = this._processes.filter(process => process.status === ProcessStatusEnum.Declined ||
      process.status === ProcessStatusEnum.Hired || process.status === ProcessStatusEnum.Rejected).length;
    this.processFinishedSuccess = this._processes.filter(process => process.status === ProcessStatusEnum.Hired).length;
    // tslint:disable-next-line: max-line-length
    this.processInProgress = this._processes.filter(process => process.status === ProcessStatusEnum.InProgress || process.status === ProcessStatusEnum.Recall || process.status === ProcessStatusEnum.OfferAccepted).length;
    // tslint:disable-next-line: max-line-length
    this.processNotStarted = this._processes.filter(process => process.status === ProcessStatusEnum.Declined || process.status === ProcessStatusEnum.Rejected).length;
    this.getProgressPercentage();
  }



}
