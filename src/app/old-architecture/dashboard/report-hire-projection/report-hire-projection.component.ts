import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { HireProjection } from '@shared/models/hire-projection.model';
import { Process } from '@shared/models/process.model';
import { FacadeService } from '@shared/services/facade.service';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { AppComponent } from '@app/app.component';


@Component({
  selector: 'app-report-hire-projection',
  templateUrl: './report-hire-projection.component.html',
  styleUrls: ['./report-hire-projection.component.scss']
})
export class ReportHireProjectionComponent implements OnInit {

  @Input() _processes;
  @Input() _hireProjections;

  //Ranking Chart

  constructor(private facade: FacadeService, private app: AppComponent) { }

  processes: Process[] = [];
  hireProjections: HireProjection[] = [];
  month: Date = new Date();
  hasProjections: boolean = false;

  isChartComplete: boolean = false;

  ngOnInit() {
    this.app.showLoading();
    this.getHireProjectionReport();
    this.app.hideLoading();
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._processes;
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

  getHireProjectionReport() {
    let date = new Date(this.month);
    let projection: HireProjection;
    let actualHires: number = 0;
    if (this.hireProjections.filter(hp => hp.month === date.getMonth() + 1 && hp.year === date.getFullYear()).length > 0) {
      projection = this.hireProjections.filter(hp => hp.month === (date.getMonth() + 1) && hp.year === date.getFullYear())[0];
      this.processes.forEach(proc => {
        if (proc.status === ProcessStatusEnum.Hired) {
          if (new Date(proc.offerStage.date).getMonth() === date.getMonth() && new Date(proc.offerStage.date).getFullYear() === date.getFullYear()) actualHires++;
        }
      });
      this.hireChartData = [
        { data: [actualHires], label: 'Actual' },
        { data: [projection.value], label: 'Projected' }
      ];
      this.hasProjections = true;
    }
    else this.hasProjections = false;
  }


  nextHireMonth() {
    let oldMonth: Date = new Date(this.month);
    this.month = new Date(oldMonth.setMonth(oldMonth.getMonth() + 1));
    this.getHireProjectionReport()
  }

  previousHireMonth() {
    let oldMonth: Date = new Date(this.month);
    this.month = new Date(oldMonth.setMonth(oldMonth.getMonth() - 1));
    this.getHireProjectionReport()
  }
}
