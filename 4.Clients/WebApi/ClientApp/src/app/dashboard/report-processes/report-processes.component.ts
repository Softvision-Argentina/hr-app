import { Component, OnInit, Input, NgModule, SimpleChanges, OnChanges, AfterViewChecked } from '@angular/core';
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
  styleUrls: ['./report-processes.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class ReportProcessesComponent implements OnInit, OnChanges, AfterViewChecked {

  @Input()
  private _detailedProcesses: Process[];
  public get detailedProcesses(): Process[] {
    return this._detailedProcesses;
  }
  public set detailedProcesses(value: Process[]) {
    this._detailedProcesses = value;
  }

  public processCompleted = 0;
  public processInProgress = 0;
  public processPercentage = 0;
  public processNotStarted = 0;
  public processSuccessPercentage = 0;
  public completedProcessLoading = true;
  public pieChartLoading = true;

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
      backgroundColor: ['#81FB15', '#F6FB15', '#6FC8CE']
    }
  ];

  // Completed Processes Chart
  processFinishedSuccess = 0;
  stadisticFinished = 0;
  stadisticFailed = 0;
  isChartComplete = false;

  constructor(private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.complete(this._detailedProcesses);
    this.app.hideLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    // tslint:disable-next-line: no-unused-expression
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
    this.processCompleted = process.filter(p => p.status === ProcessStatusEnum.Declined ||
      p.status === ProcessStatusEnum.Hired || p.status === ProcessStatusEnum.Rejected).length;
    this.processFinishedSuccess = process.filter(p => p.status === ProcessStatusEnum.Hired).length;
    // tslint:disable-next-line: max-line-length
    this.processInProgress = process.filter(p => p.status === ProcessStatusEnum.InProgress || p.status === ProcessStatusEnum.Recall || p.status === ProcessStatusEnum.OfferAccepted).length;
    // tslint:disable-next-line: max-line-length
    this.processNotStarted = process.filter(p => p.status === ProcessStatusEnum.Declined || p.status === ProcessStatusEnum.Rejected).length;
  }

  getProgressPercentage() {
    this.app.showLoading();
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

  isPieData(): boolean {
    const totalProcces: number = this.processInProgress + this.processNotStarted;
    if (totalProcces > 0) { return true; } else { return false; }
  }

}
