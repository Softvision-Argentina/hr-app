import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { ChartType, ChartOptions, ChartDataSets } from 'chart.js';
import { HireProjection } from 'src/entities/hireProjection';
import { Process } from 'src/entities/process';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { AppComponent } from 'src/app/app.component';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';


@Component({
  selector: 'app-report-hire-projection',
  templateUrl: './report-hire-projection.component.html',
  styleUrls: ['./report-hire-projection.component.css']
})
export class ReportHireProjectionComponent implements OnInit, OnChanges {

  public hireChartOptions: ChartOptions = {
    responsive: true,
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public hireChartLabels: Label[] = ['Hires'];
  public hireChartType: ChartType = 'bar';
  public hireChartLegend = true;
  public hireChartPlugins = [pluginDataLabels];

  public hireChartData: ChartDataSets[] = [
    { data: [0], label: 'Actual' },
    { data: [0], label: 'Projected' }
  ];

  @Input() _processes;
  @Input() _hireProjections;

  // Ranking Chart

  constructor(private facade: FacadeService, private app: AppComponent) { }

  processes: Process[] = [];
  hireProjections: HireProjection[] = [];
  month: Date = new Date();
  hasProjections = false;

  isChartComplete = false;

  ngOnInit() {
    this.app.showLoading();
    this.getHireProjectionReport();
    this.app.hideLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    // tslint:disable-next-line: no-unused-expression
    changes._processes;
    // tslint:disable-next-line: no-unused-expression
    changes._hireProjections;
    this.complete();
    if (!this.isChartComplete) {
      setTimeout(() => {
        this.getHireProjectionReport();
      });
    }
  }

  complete() {
    this.processes = this._processes;
    this.hireProjections = this._hireProjections;
  }

  getHireProjectionReport() {
    const date = new Date(this.month);
    let projection: HireProjection;
    let actualHires = 0;
    if (this.hireProjections.filter(hp => hp.month === date.getMonth() + 1 && hp.year === date.getFullYear()).length > 0) {
      projection = this.hireProjections.filter(hp => hp.month === (date.getMonth() + 1) && hp.year === date.getFullYear())[0];
      this.processes.forEach(proc => {
        if (proc.status === ProcessStatusEnum.Hired) {
          const offerStageDate = new Date(proc.offerStage.date);
          if (offerStageDate.getMonth() === date.getMonth() && offerStageDate.getFullYear() === date.getFullYear()) {
            actualHires++;
          }
        }
      });
      this.hireChartData = [
        { data: [actualHires], label: 'Actual' },
        { data: [projection.value], label: 'Projected' }
      ];
      this.hasProjections = true;
    } else { this.hasProjections = false; }
  }


  nextHireMonth() {
    const oldMonth: Date = new Date(this.month);
    this.month = new Date(oldMonth.setMonth(oldMonth.getMonth() + 1));
    this.getHireProjectionReport();
  }

  previousHireMonth() {
    const oldMonth: Date = new Date(this.month);
    this.month = new Date(oldMonth.setMonth(oldMonth.getMonth() - 1));
    this.getHireProjectionReport();
  }
}
