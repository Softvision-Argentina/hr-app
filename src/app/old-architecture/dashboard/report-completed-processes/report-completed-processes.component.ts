import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { Label, SingleDataSet } from 'ng2-charts';
import { AppComponent } from '@app/app.component';


@Component({
  selector: 'app-report-completed-processes',
  templateUrl: './report-completed-processes.component.html',
  styleUrls: ['./report-completed-processes.component.scss']
})
export class ReportCompletedProcessesComponent implements OnInit {

  @Input() _processes;
  // public get processes(): any[] {
  //   return this._processes;
  // }
  // public set process(value: any[]) {
  //     this._processes = value;
  // }


  processCompleted: number = 0;
  processInProgress: number = 0;
  processNotStarted: number = 0;
  processFinishedSuccess: number = 0;
  completedProcessLoading: boolean = true;
  public pieChartLabels: Label[] = ['IN PROGRESS', 'NOT STARTED'];
  public pieChartData: SingleDataSet = [0, 0, 0];
  public pieChartColors: Array<any> = [
    {
      backgroundColor: ["#81FB15", "#F6FB15", "#6FC8CE"]
    }
  ];
  pieChartLoading: boolean = true;


  stadisticFinished: number = 0;
  stadisticFailed: number = 0;
  isChartComplete: boolean = false;

  constructor(private app: AppComponent) { }

  ngOnInit() {
    //this.ngAfterViewChecked();
    this.complete();
    this.app.showLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._processes;
    this.complete();
  }


  getProgressPercentage() {
    let totalCandidates: number = this.processCompleted;
    if (totalCandidates > 0) {
      this.stadisticFinished = (this.processFinishedSuccess * 100) / 2;
      if (this.stadisticFinished === 100) this.stadisticFailed = 0;
      else this.stadisticFailed = ((totalCandidates - this.processFinishedSuccess) * 100) / totalCandidates;
      if (this.stadisticFailed === 100) this.stadisticFinished = 0;
      this.isChartComplete = true;
    }

    let labels: string[] = [];
    let percentages: number[] = [];
    let colors: string[] = [];

    if (this.processInProgress > 0) {
      labels.push('IN PROGRESS');
      colors.push("#81FB15");
      percentages.push(this.processInProgress);
    }
    if (this.processNotStarted > 0) {
      labels.push('NOT STARTED');
      colors.push("#6FC8CE");
      percentages.push(this.processNotStarted);
    }
    this.pieChartLabels = labels;
    this.pieChartData = percentages;
    if (colors.length === 0) colors = ["#81FB15", "#F6FB15", "#6FC8CE"];
    this.pieChartColors = [
      {
        backgroundColor: colors
      }
    ];
    this.pieChartLoading = false;
    this.completedProcessLoading = false;
    this.app.hideLoading();
  }

  // ngAfterViewChecked(): void {
  //   if (!this.isChartComplete) {
  //     setTimeout(() => {
  //       this.getProgressPercentage();
  //     });
  //   }
  // }


  complete() {
    this.processCompleted = this._processes.filter(process => process => process.status === ProcessStatusEnum.Declined ||
      process.status === ProcessStatusEnum.Hired || process.status === ProcessStatusEnum.Rejected).length;
    this.processFinishedSuccess = this._processes.filter(process => process.status === ProcessStatusEnum.Hired).length;
    this.processInProgress = this._processes.filter(process => process.status === ProcessStatusEnum.InProgress || process.status === ProcessStatusEnum.Recall || process.status === ProcessStatusEnum.Accepted).length;
    this.processNotStarted = this._processes.filter(process => process.status === ProcessStatusEnum.Declined || process.status === ProcessStatusEnum.Rejected).length;
    this.getProgressPercentage();
  }



}
