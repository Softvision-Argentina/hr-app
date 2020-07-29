import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { EmployeeCasualty } from '@shared/models/employee-casualty.model';
import { HireProjection } from '@shared/models/hire-projection.model';
import { Process } from '@shared/models/process.model';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { AppComponent } from '@app/app.component';


@Component({
  selector: 'app-report-hire-casualties',
  templateUrl: './report-hire-casualties.component.html',
  styleUrls: ['./report-hire-casualties.component.scss']
})
export class ReportHireCasualtiesComponent implements OnInit {

  @Input() _processes;
  @Input() _employeeCasualty;


  constructor(private app: AppComponent) { }

  processes: Process[] = [];
  employeeCasualty: EmployeeCasualty[] = [];
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
    changes._employeeCasualty;
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

  getHireProjectionReport() {
    let date = new Date(this.month);
    let actualHires: number = 0;
    let casualties: EmployeeCasualty;
    if (this.employeeCasualty.filter(ec => ec.month === date.getMonth() + 1 && ec.year === date.getFullYear()).length > 0) {
      casualties = this.employeeCasualty.filter(ec => ec.month === (date.getMonth() + 1) && ec.year === date.getFullYear())[0];
      //------------------------------------------
      this.processes.forEach(proc => {
        if (proc.status === ProcessStatusEnum.Hired) {
          if (new Date(proc.offerStage.date).getMonth() === date.getMonth() && new Date(proc.offerStage.date).getFullYear() === date.getFullYear()) actualHires++;
        }
      });
      //-----------------------------
      this.hireChartData = [
        { data: [casualties.value], label: 'Casualties' },
        { data: [actualHires], label: 'Actual Hires' }
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
