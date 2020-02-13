import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label } from 'ng2-charts';
import { AppComponent } from 'src/app/app.component';
import { Process } from 'src/entities/process';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { FacadeService } from 'src/app/services/facade.service';
import { DeclineReason } from 'src/entities/declineReason';

@Component({
  selector: 'app-report-decline-reasons',
  templateUrl: './report-decline-reasons.component.html',
  styleUrls: ['./report-decline-reasons.component.css']
})
export class ReportDeclineReasonsComponent implements OnInit {

  @Input() _processes;  

  constructor(private facade: FacadeService, private app: AppComponent) { }

  processes: Process[] = [];    
  hasProjections: boolean = false;
  isChartComplete: boolean = false;
  filteredDeclineReasons : DeclineReason[] = [];

  ngOnInit() {
    this.app.showLoading();
    this.getDeclineReasons();
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

  getDeclineReasons(){
    this.facade.declineReasonService.get<DeclineReason>()
      .subscribe(res => {
        this.filteredDeclineReasons = res;        
      }, err => {
        console.log(err);
      });
  }

  complete() {
    this.processes = this._processes;  
  }

  public chartLabels: Label[] = [];
  public chartType: ChartType = 'radar';
  public chartLegend = false;
  public chartPlugins = [pluginDataLabels];
  public chartData: ChartDataSets[] = [
    { data: [], label: '' }    
  ];
  public chartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };

  getProjectionReport() {    
    let reasons : string[] = [];
    let dataArray : number[] = [];
    let quantity : number;
    let procArray = this.processes.filter(proc => proc.status == ProcessStatusEnum.Declined);
    if (procArray.length > 0){
      this.filteredDeclineReasons.forEach(decR => {
        quantity = 0;
        procArray.forEach(procA =>{
          if(decR.name == procA.declineReason.name){
            quantity++;
          }
        })
        if (quantity >0){
          reasons.push(decR.name);
          dataArray.push(quantity);
        }
      });
      this.chartData = [
        { data: dataArray, label: 'Times' }        
      ];
      this.chartLabels = reasons;
      this.hasProjections = true;
    }
    else this.hasProjections = false;
  }

  doSomething(){
    console.log(this.filteredDeclineReasons);
    console.log(this.chartLabels);
    console.log(this.chartData[0].data);
  }
}
