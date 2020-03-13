import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { ChartType, ChartOptions, ChartDataSets } from 'chart.js';
//import { HireProjection } from 'src/entities/hireProjection';
import { Process } from 'src/entities/process';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { AppComponent } from 'src/app/app.component';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import * as getISOWeek from 'date-fns/get_iso_week';
import * as addWeeks from 'date-fns/add_weeks';
//import { registerLocaleData } from '@angular/common';
//import esAR from '@angular/common/locales/es-AR';
//registerLocaleData(esAR);

@Component({
  selector: 'app-report-weekly-candidates',
  templateUrl: './report-weekly-candidates.component.html',
  styleUrls: ['./report-weekly-candidates.component.css']
})
export class ReportWeeklyCandidatesComponent implements OnInit {

  @Input() _processes;
  @Input() _hireProjections;

  //Ranking Chart

  constructor(private facade: FacadeService, private app: AppComponent) { }

  processes: Process[] = [];
  processesByCandidate : Process[][] = [];
  //hireProjections: HireProjection[] = [];
  month: Date = new Date();
  date: Date = new Date();
  hasCandidates: boolean = false;

  isChartComplete: boolean = false;

  ngOnInit() {
    this.app.showLoading();
    this.getWeek();
    this.app.hideLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._processes;
    changes._hireProjections;
    this.complete();
    if (!this.isChartComplete) {
      setTimeout(() => {
        this.getWeek();
      });
    }
  }

  complete() {
    this.processes = this._processes;
    //this.hireProjections = this._hireProjections;
  }

  public chartOptions: ChartOptions = {
    responsive: true,
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public chartLabels: Label[] = ['Hires'];
  public chartType: ChartType = 'bar';
  public chartLegend = true;
  public chartPlugins = [pluginDataLabels];

  public chartData: ChartDataSets[] = [];

  getWeek() {
    this.processesByCandidate = [];
    for (let process of this.processes) {
      if (this.processesByCandidate[process.candidate.recruiter.id] === undefined) {
        this.processesByCandidate[process.candidate.recruiter.id] = [];
      }
      this.processesByCandidate[process.candidate.recruiter.id].push(process)
    }
    //this.processes.forEach(process => this.processesByCandidate[process.candidate.recruiter.id].push(process));
    //if (this.hireProjections.filter(hp => hp.month == date.getMonth() + 1 && hp.year == date.getFullYear()).length > 0) {
    this.processesByCandidate.forEach(pbc => pbc = pbc.filter(p => getISOWeek(p.createdDate) === getISOWeek(this.date)));
    this.hasCandidates = this.processesByCandidate.some(pbc => pbc.length > 0);
    if (this.hasCandidates) {
      this.chartData = [];
      this.processesByCandidate.forEach(p => this.chartData.push({data: [p.length], label: p[0].candidate.recruiter.name + " " + p[0].candidate.recruiter.lastName}))
    }
  }

  nextWeek() {
    this.date = addWeeks(this.date, 1);
    this.getWeek();
  }

  previousWeek() {
    this.date = addWeeks(this.date, -1);
    this.getWeek();
  }
}
