import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Process } from 'src/entities/process';
import { FacadeService } from 'src/app/services/facade.service';
import { AppComponent } from 'src/app/app.component';
import { HireProjection } from 'src/entities/hireProjection';
import { Label } from 'ng2-charts';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';

@Component({
  selector: 'app-report-timetofill2',
  templateUrl: './report-timetofill2.component.html',
  styleUrls: ['./report-timetofill2.component.css']
})
export class ReportTimetofill2Component implements OnInit {

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
  public hireChartLabels: Label[] = [];
  public hireChartType: ChartType = 'bar';
  public hireChartLegend = true;
  public hireChartPlugins = [pluginDataLabels];

  public hireChartData: ChartDataSets[] = [
    { data: [0], label: 'Sarlacatnaa' }    
  ];

  getHireProjectionReport() {
    let date = new Date(this.month);    
    let days : number[] = [];
    let dayChartLabels: Label[] = []
    let validArray : Process[] = this.processes.filter(proc => new Date(proc.hrStage.date).getMonth() +1  == date.getMonth() + 1 && proc.status == ProcessStatusEnum.Hired && new Date(proc.hrStage.date).getFullYear() == date.getFullYear());
    if (validArray.length > 0) {
      validArray.forEach(va => {        
        days.push(new Date(va.offerStage.hireDate).getDay() - new Date(va.hrStage.date).getDay());      
        console.log(new Date(va.offerStage.hireDate).getDay() - new Date(va.hrStage.date).getDay());      
        dayChartLabels.push(new Date(va.hrStage.date).toDateString());         
      });
      this.hireChartData = [{ data: days, label: 'Days' }]
      this.hireChartLabels = dayChartLabels;
      this.hasProjections = true;
      console.log(days);
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

  doSomething(){
    this.processes.forEach(proc => {
      console.log(new Date(proc.hrStage.date).getMonth()+1);      
      console.log(new Date(proc.hrStage.date).getFullYear());      
      console.log(new Date(proc.hrStage.date));
      console.log(ProcessStatusEnum[proc.status]);            
    });    
  }
}
