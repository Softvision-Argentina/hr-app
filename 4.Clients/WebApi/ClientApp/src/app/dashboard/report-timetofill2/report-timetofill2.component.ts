import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Process } from 'src/entities/process';
import { AppComponent } from 'src/app/app.component';
import { Label } from 'ng2-charts';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';

@Component({
  selector: 'app-report-timetofill2',
  templateUrl: './report-timetofill2.component.html',
  styleUrls: ['./report-timetofill2.component.css']
})
export class ReportTimetofill2Component implements OnInit, OnChanges {

  public chartColors: any[] = [
    {
      backgroundColor: 'rgb(103, 139, 240, 0.3)',
      borderColor: 'rgb(103, 58, 183)',
      pointBackgroundColor: 'rgb(103, 58, 183)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(103, 58, 183, .8)'
    },
  ];

  public chartOptions: ChartOptions = {
    responsive: true,
    layout: {
      padding: {
        left: 0,
        right: 20,
        top: 30,
        bottom: 0
      }
    },
    scales: {
      yAxes: [{
        id: 'y-axis-0',
        ticks: {
          beginAtZero: true
        }
      }],
      xAxes: [{
        ticks: {
          autoSkip: true
        }
      }]
    },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public chartLabels: Label[] = [];
  public chartType: ChartType = 'line';
  public chartLegend = false;
  public chartPlugins = [pluginDataLabels];

  public chartData: ChartDataSets[] = [
    { data: [], label: 'Dates' }
  ];

  // Average: Interview date - Hire Date
  @Input() _processes;

  constructor(private app: AppComponent) { }

  processes: Process[] = [];
  month: Date = new Date();
  hasProjections = false;

  isChartComplete = false;

  ngOnInit() {
    this.app.showLoading();
    this.getProjectionReport();
    this.app.hideLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    // tslint:disable-next-line: no-unused-expression
    changes._processes;
    this.complete();
    if (!this.isChartComplete) {
      setTimeout(() => {
        this.getProjectionReport();
      });
    }
  }

  complete() {
    this.processes = this._processes;
  }

  getProjectionReport() {
    let averageDays = 0;
    const date = new Date(this.month);
    const days: number[] = [];
    const dayChartLabels: Label[] = [];
    // tslint:disable-next-line: max-line-length
    const validArray: Process[] = this.processes.filter(proc => new Date(proc.hrStage.date).getMonth() + 1 === date.getMonth() + 1 && proc.status === ProcessStatusEnum.Hired && new Date(proc.hrStage.date).getFullYear() === date.getFullYear());
    if (validArray.length > 0) {
      validArray.forEach(va => {
        // tslint:disable-next-line: max-line-length
        averageDays += Math.ceil((Math.abs(new Date(va.offerStage.hireDate).getTime() - new Date(va.hrStage.date).getTime())) / (1000 * 3600 * 24));
        // tslint:disable-next-line: max-line-length
        days.push(Math.ceil((Math.abs(new Date(va.offerStage.hireDate).getTime() - new Date(va.hrStage.date).getTime())) / (1000 * 3600 * 24)));
        dayChartLabels.push(new Date(va.hrStage.date).toDateString());
      });
      days.push(Number((averageDays / days.length).toFixed(2)));
      dayChartLabels.push('Average');
      this.chartData = [
        { data: days, label: 'Days' }
      ];
      this.chartLabels = dayChartLabels;
      this.hasProjections = true;
    } else { this.hasProjections = false; }
  }

  nextMonth() {
    const oldMonth: Date = new Date(this.month);
    this.month = new Date(oldMonth.setMonth(oldMonth.getMonth() + 1));
    this.getProjectionReport();
  }

  previousMonth() {
    const oldMonth: Date = new Date(this.month);
    this.month = new Date(oldMonth.setMonth(oldMonth.getMonth() - 1));
    this.getProjectionReport();
  }
}
