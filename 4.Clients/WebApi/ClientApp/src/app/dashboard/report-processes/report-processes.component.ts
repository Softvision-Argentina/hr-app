import { Component, OnInit, Input, NgModule, SimpleChanges } from '@angular/core';
import { SingleDataSet, Label } from 'ng2-charts';
import { ChartType, ChartOptions } from 'chart.js';
import { Process } from 'src/entities/process';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { ChangeDetectionStrategy } from '@angular/core';
import { AppComponent } from 'src/app/app.component';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';

@Component({
  selector: 'app-report-processes',
  templateUrl: './report-processes.component.html',
  styleUrls: ['./report-processes.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class ReportProcessesComponent implements OnInit {

  @Input()
  private _detailedProcesses: Process[];
  public get detailedProcesses(): Process[] {
    return this._detailedProcesses;
  }
  public set detailedProcesses(value: Process[]) {
    this._detailedProcesses = value;
  }

  //Porcesses Chart
  public processCompleted: number = 0;
  public processInProgress: number = 0;
  public processPercentage: number = 0;
  public processNotStarted: number = 0;
  public processSuccessPercentage: number = 0;
  public completedProcessLoading: boolean = true;
  public pieChartLoading: boolean = true;

  public pieChartLabels: Label[] = ['IN PROGRESS', 'NOT STARTED'];
  public pieChartData: SingleDataSet = [0, 0, 0];
  public pieChartType: ChartType = 'pie';
  public pieChartPlugins = [pluginDataLabels];
  public pieChartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      datalabels: {
        formatter: (value, ctx) => {
          const label = ctx.chart.data.labels[ctx.dataIndex];
          return label;
        },
      },
    }
  };
  public pieChartColors: Array<any> = [
    {
      backgroundColor: ["#81FB15", "#F6FB15", "#6FC8CE"]
    }
  ];

  // Completed Processes Chart
  processFinishedSuccess: number = 0;
  stadisticFinished: number = 0;
  stadisticFailed: number = 0;
  isChartComplete: boolean = false;

  constructor(private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.complete(this._detailedProcesses);
    this.app.hideLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._detailedProcesses;
    this.app.showLoading();
    this.complete(this._detailedProcesses);
    this.getProgressPercentage();
    this.isPieData();
    this.app.hideLoading();
  }

  ngAfterViewChecked(): void {
    if (!this.isChartComplete) {
      setTimeout(() => {
        this.getProgressPercentage();
      });
    }
  }

  complete(process: Process[]) {
    this.processCompleted = process.filter(process => process.status === ProcessStatusEnum.Declined ||
      process.status === ProcessStatusEnum.Hired || process.status === ProcessStatusEnum.Rejected).length;
    this.processFinishedSuccess = process.filter(process => process.status === ProcessStatusEnum.Hired).length;
    this.processInProgress = process.filter(process => process.status === ProcessStatusEnum.InProgress || process.status === ProcessStatusEnum.Recall || process.status === ProcessStatusEnum.Accepted).length;
    this.processNotStarted = process.filter(process => process.status === ProcessStatusEnum.Declined || process.status === ProcessStatusEnum.Rejected).length;
  }

  getProgressPercentage() {
    this.app.showLoading();
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

  isPieData(): boolean {
    let totalProcces: number = this.processInProgress + this.processNotStarted;
    if (totalProcces > 0) return true;
    else return false;
  }

}
