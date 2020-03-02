import { Component, OnInit, SimpleChanges } from '@angular/core';
import { Process } from 'src/entities/process';
import { FacadeService } from '../services/facade.service';
import { ChartType, ChartOptions, ChartDataSets } from 'chart.js';
import { SingleDataSet, Label } from 'ng2-charts';
import { Skill } from 'src/entities/skill';
import { CandidateSkill } from 'src/entities/candidateSkill';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { AppComponent } from '../app.component';
import { HireProjection } from 'src/entities/hireProjection';
import { EmployeeCasualty } from 'src/entities/employeeCasualty';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  providers: [AppComponent]
})
export class DashboardComponent implements OnInit {

  processes: Process[] = [];
  skillList: Skill[] = [];
  candidatesSkills: CandidateSkill[] = [];
  employeeCasualty: EmployeeCasualty[] = [];
  pieChartLoading: boolean = true;
  topSkillsLoading: boolean = true;
  carousellLoading: boolean = true;
  completedProcessLoading: boolean = true;
  showPieChart: boolean = false;
  hireProjections: HireProjection[] = [];
  month: Date = new Date();
  hasProjections: boolean = false;

  //Porcesses Chart
  processCompleted: number = 0;
  processInProgress: number = 0;
  processPercentage: number = 0;
  processNotStarted: number = 0;
  processSuccessPercentage: number = 0;
  public pieChartLabels: Label[] = ['IN PROGRESS', 'NOT STARTED'];
  public pieChartData: SingleDataSet = [0, 0, 0];
  public pieChartType: ChartType = 'pie';
  public pieChartPlugins = [pluginDataLabels];
  public pieChartColors: Array<any> = [
    {
      backgroundColor: ["#81FB15", "#F6FB15", "#6FC8CE"]
    }
  ];
  public pieChartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      datalabels: {
        formatter: (value, ctx) => {
          const label = ctx.chart.data.labels[ctx.dataIndex];
          return label;
        },
      },
    }
  };

  //Completed Processes Chart
  processFinishedSuccess: number = 0;
  stadisticFinished: number = 0;
  stadisticFailed: number = 0;

  isChartComplete: boolean = false;

  //Ranking Chart
  skillRankedList: any[] = [
    { id: 0, name: '', points: 0 },
    { id: 0, name: '', points: 0 },
    { id: 0, name: '', points: 0 }
  ];

  constructor(private facade: FacadeService, private app: AppComponent) {

  }

  ngOnInit(): void {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getProcesses();
    this.getHireProjection();
    this.getEmployeeCasualties();
    this.app.hideLoading();
  }

  // ngAfterViewChecked(): void {
  //   if (!this.isChartComplete) {
  //     setTimeout(() => {
  //       // this.getProgressPercentage();
  //       // this.getHireProjectionReport();
  //     });
  //   }
  // }

  getProcesses() {
    this.facade.processService.get()
      .subscribe(res => {
        this.processes = res;
        this.processCompleted = res.filter(process => process => process.status === ProcessStatusEnum.Declined ||
          process.status === ProcessStatusEnum.Hired || process.status === ProcessStatusEnum.Rejected).length;
        this.processFinishedSuccess = res.filter(process => process.status === ProcessStatusEnum.Hired).length;
        this.processInProgress = res.filter(process => process.status === ProcessStatusEnum.InProgress || process.status == ProcessStatusEnum.Recall || process.status == ProcessStatusEnum.OfferAccepted).length;
        this.processNotStarted = res.filter(process => process.status === ProcessStatusEnum.Declined || process.status === ProcessStatusEnum.Rejected).length;
      }, err => {
        console.log(err);
      });
  }

  getHireProjection() {
    this.facade.hireProjectionService.get()
      .subscribe(res => {
        this.hireProjections = res;
      });
  }

  getEmployeeCasualties() {
    this.facade.employeeCasulatyService.get()
      .subscribe(res => {
        this.employeeCasualty = res;
      });
  }

  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
  }

  checkIndex(i: number): boolean {
    if (i < 3) return true;
    else return false;
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
}
