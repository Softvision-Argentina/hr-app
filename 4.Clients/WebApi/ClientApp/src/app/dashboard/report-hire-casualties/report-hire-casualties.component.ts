import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { ChartType, ChartOptions, ChartDataSets } from 'chart.js';
import { HireProjection } from 'src/entities/hireProjection';
import { Process } from 'src/entities/process';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { SingleDataSet, Label } from 'ng2-charts';
import { AppComponent } from 'src/app/app.component';
import { EmployeeCasualty } from 'src/entities/employeeCasualty';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';


@Component({
  selector: 'app-report-hire-casualties',
  templateUrl: './report-hire-casualties.component.html',
  styleUrls: ['./report-hire-casualties.component.css']
})
export class ReportHireCasualtiesComponent implements OnInit, OnChanges {

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
  public hireChartLabels: Label[] = ['Hires and Casualties'];
  public hireChartType: ChartType = 'horizontalBar';
  public hireChartLegend = true;
  public hireChartPlugins = [pluginDataLabels];

  public hireChartData: ChartDataSets[] = [
    { data: [0], label: 'Casualties' },
    { data: [0], label: 'Hires' }
  ];

  @Input() _processes;
  @Input() _employeeCasualty;


  constructor(private app: AppComponent) { }

  processes: Process[] = [];
  employeeCasualty: EmployeeCasualty[] = [];
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
    changes._employeeCasualty;
    // tslint:disable-next-line: no-unused-expression
    changes._processes;
    this.complete();
    if (!this.isChartComplete) {
      setTimeout(() => {
        this.getHireProjectionReport();
      });
    }
  }

  complete() {
    this.processes = this._processes;
    this.employeeCasualty = this._employeeCasualty;
  }

  getHireProjectionReport() {
    const date = new Date(this.month);
    let actualHires = 0;
    let casualties: EmployeeCasualty;
    if (this.employeeCasualty.filter(ec => ec.month === date.getMonth() + 1 && ec.year === date.getFullYear()).length > 0) {
      casualties = this.employeeCasualty.filter(ec => ec.month === (date.getMonth() + 1) && ec.year === date.getFullYear())[0];

      this.processes.forEach(proc => {
        if (proc.status === ProcessStatusEnum.Hired) {
          // tslint:disable-next-line: max-line-length
          if (new Date(proc.offerStage.date).getMonth() === date.getMonth() && new Date(proc.offerStage.date).getFullYear() === date.getFullYear()) { actualHires++; }
        }
      });

      this.hireChartData = [
        { data: [casualties.value], label: 'Casualties' },
        { data: [actualHires], label: 'Actual Hires' }
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
