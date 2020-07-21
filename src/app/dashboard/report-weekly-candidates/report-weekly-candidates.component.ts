import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { ChartType, ChartOptions, ChartDataSets } from 'chart.js';
import { Process } from 'src/entities/process';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { AppComponent } from 'src/app/app.component';
import getISOWeek from 'date-fns/getISOWeek';
import addWeeks from 'date-fns/addWeeks';

@Component({
  selector: 'app-report-weekly-candidates',
  templateUrl: './report-weekly-candidates.component.html',
  styleUrls: ['./report-weekly-candidates.component.scss']
})
export class ReportWeeklyCandidatesComponent implements OnInit {

  @Input() _processes;
  @Input() _hireProjections;

  constructor(private facade: FacadeService, private app: AppComponent) { }

  processes: Process[] = [];
  processesByCandidate : Process[][] = [];
  date: Date;
  hasCandidates: boolean = false;
  isChartComplete: boolean = false;
  public chartLabels: Label[] = ['Hires'];
  public chartType: ChartType = 'bar';
  public chartLegend = true;
  public chartPlugins = [pluginDataLabels];
  public chartData: ChartDataSets[] = [];
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

  ngOnInit() {
    this.app.showLoading();
    this.date = new Date();
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

  getWeek() {
    if (this.date) {
      this.processesByCandidate = [];

      for (let process of this.processes) {
        if (this.processesByCandidate[process.candidate.user.id] === undefined) {
          this.processesByCandidate[process.candidate.user.id] = [];
        }

        this.processesByCandidate[process.candidate.user.id].push(process)
      }

      for (let key in this.processesByCandidate) {
        this.processesByCandidate[key] = this.processesByCandidate[key].filter(p => getISOWeek(new Date(p.createdDate)) === getISOWeek(this.date) && new Date(p.createdDate).getFullYear() === this.date.getFullYear());
      }

      this.hasCandidates = this.processesByCandidate.some(pbc => pbc.length > 0);

      if (this.hasCandidates) {
        this.chartData = [];
        this.processesByCandidate.forEach(p => this.chartData.push({data: [p.length], label: p[0].candidate.user.firstName + ' ' + p[0].candidate.user.firstName }))
      }
    }
  }

  nextWeek() {
    if (this.date) {
      this.date = addWeeks(this.date, 1);
      this.getWeek();
    }
  }

  previousWeek() {
    if (this.date) {
      this.date = addWeeks(this.date, -1);
      this.getWeek();
    }
  }
}
