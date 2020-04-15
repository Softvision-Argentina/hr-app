import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Process } from 'src/entities/process';
import { AppComponent } from 'src/app/app.component';
import { Label } from 'ng2-charts';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { FacadeService } from 'src/app/services/facade.service';
import { Offer } from 'src/entities/offer';

@Component({
  selector: 'app-report-timetofill1',
  templateUrl: './report-timetofill1.component.html',
  styleUrls: ['./report-timetofill1.component.css']
})
export class ReportTimetofill1Component implements OnInit {

  // Average: Interview date - Offer Accepted
  @Input() _processes;

  constructor(private app: AppComponent, private facade: FacadeService) { }

  processes: Process[] = [];
  month: Date = new Date();
  hasProjections: boolean = false;
  offers: Offer[] = [];
  auxDate: Date = new Date();

  isChartComplete: boolean = false;

  ngOnInit() {
    this.app.showLoading();
    this.getOffers();
    this.getProjectionReport();
    this.app.hideLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._processes;
    this.complete();
    if (!this.isChartComplete) {
      setTimeout(() => {
        this.getProjectionReport();
      });
    }
  }

  getOffers() {
    this.facade.offerService.get()
      .subscribe(res => {
        this.offers = res;
      }, err => {
        console.log(err);
      });
  }

  complete() {
    this.processes = this._processes;
  }

  public chartColors: any[] = [
    {
      backgroundColor: 'rgb(103, 255, 130, 0.3)',
      borderColor: 'rgb(103, 183, 58)',
      pointBackgroundColor: 'rgb(103, 183, 58)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(103, 183, 58, .8)'
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

  getProjectionReport() {
    let averageDays: number = 0;
    let date = new Date(this.month);
    let days: number[] = [];
    let dayChartLabels: Label[] = []
    let validArray: Process[] = this.processes.filter(proc => new Date(proc.hrStage.date).getMonth() + 1 === date.getMonth() + 1 && proc.status === ProcessStatusEnum.OfferAccepted && new Date(proc.hrStage.date).getFullYear() === date.getFullYear());
    if (validArray.length > 0) {
      validArray.forEach(va => {
        averageDays += Math.ceil((Math.abs(new Date(this.getLastOffer(va.offerStage.processId)).getTime() - new Date(va.hrStage.date).getTime())) / (1000 * 3600 * 24));
        days.push(Math.ceil((Math.abs(new Date(this.getLastOffer(va.offerStage.processId)).getTime() - new Date(va.hrStage.date).getTime())) / (1000 * 3600 * 24)));
        dayChartLabels.push(new Date(va.hrStage.date).toDateString());
      });
      days.push(Number((averageDays / days.length).toFixed(2)));
      dayChartLabels.push("Average");
      this.chartData = [
        { data: days, label: 'Days' }
      ]
      this.chartLabels = dayChartLabels;
      this.hasProjections = true;
    }
    else this.hasProjections = false;
  }

  getLastOffer(id: number): Date {
    let lastOfferDate: Date;
    lastOfferDate = (this.offers.filter(x => x.id === id)).pop().offerDate;
    return lastOfferDate;
  }

  nextMonth() {
    let oldMonth: Date = new Date(this.month);
    this.month = new Date(oldMonth.setMonth(oldMonth.getMonth() + 1));
    this.getProjectionReport()
  }

  previousMonth() {
    let oldMonth: Date = new Date(this.month);
    this.month = new Date(oldMonth.setMonth(oldMonth.getMonth() - 1));
    this.getProjectionReport()
  }
}
