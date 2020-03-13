import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { ChartType, ChartOptions, ChartDataSets } from 'chart.js';
import { Process } from 'src/entities/process';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { AppComponent } from 'src/app/app.component';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import * as getISOWeek from 'date-fns/get_iso_week';
import * as addWeeks from 'date-fns/add_weeks';

@Component({
  selector: 'app-report-weekly-candidates',
  templateUrl: './report-weekly-candidates.component.html',
  styleUrls: ['./report-weekly-candidates.component.css']
})
export class ReportWeeklyCandidatesComponent implements OnInit {

  @Input() _processes;
  @Input() _hireProjections;

  constructor(private facade: FacadeService, private app: AppComponent) { }

  processes: Process[] = [];
  processesByCandidate : Process[][] = [];
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
    for (let key in this.processesByCandidate) {
      this.processesByCandidate[key] = this.processesByCandidate[key].filter(p => getISOWeek(new Date(p.createdDate)) === getISOWeek(this.date) && new Date(p.createdDate).getFullYear() === this.date.getFullYear());
    }
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