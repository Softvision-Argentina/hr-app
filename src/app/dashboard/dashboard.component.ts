import { Component, OnInit } from '@angular/core';
import { Process } from 'src/entities/process';
import { FacadeService } from '../services/facade.service';
import { ChartType, ChartOptions, ChartDataSets } from 'chart.js';
import { SingleDataSet, Label } from 'ng2-charts';
import { Skill } from 'src/entities/skill';
import { CandidateSkill } from 'src/entities/candidateSkill';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { HireProjection } from 'src/entities/hireProjection';
import { EmployeeCasualty } from 'src/entities/employeeCasualty';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
import { Dashboard } from 'src/entities/dashboard';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  processes: Process[] = [];
  skillList: Skill[] = [];
  candidatesSkills: CandidateSkill[] = [];
  employeeCasualty: EmployeeCasualty[] = [];
  pieChartLoading = true;
  topSkillsLoading = true;
  carousellLoading = true;
  completedProcessLoading = true;
  showPieChart = false;
  hireProjections: HireProjection[] = [];
  month: Date = new Date();
  hasProjections = false;

  //Porcesses Chart
  processCompleted = 0;
  processInProgress = 0;
  processPercentage = 0;
  processNotStarted = 0;
  processSuccessPercentage = 0;
  public pieChartLabels: Label[] = ['IN PROGRESS', 'NOT STARTED'];
  public pieChartData: SingleDataSet = [0, 0, 0];
  public pieChartType: ChartType = 'pie';
  public pieChartPlugins = [pluginDataLabels];
  public pieChartColors: Array<any> = [
    {
      backgroundColor: ['#81FB15', '#F6FB15', '#6FC8CE']
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
  processFinishedSuccess = 0;
  stadisticFinished = 0;
  stadisticFailed = 0;

  isChartComplete = false;

  //Ranking Chart
  skillRankedList: any[] = [
    { id: 0, name: '', points: 0 },
    { id: 0, name: '', points: 0 },
    { id: 0, name: '', points: 0 }
  ];

  dashboards: Dashboard[] = new Array();

  isLoaded = false;

  constructor(private facade: FacadeService) {
  }

  ngOnInit(): void {
    this.facade.appService.startLoading();
    this.facade.appService.removeBgImage();
    this.getProcesses();
    this.getHireProjection();
    this.getEmployeeCasualties();
    this.getDashboards();
    this.facade.dashboardService.dashboards.subscribe(res => this.dashboards = res);
    this.facade.appService.stopLoading();
  }

  getDashboards() {
    this.facade.dashboardService.get().subscribe(
      res => {
        res.forEach(dash => {
          this.dashboards.push(dash);
        });
        this.isLoaded = true;
      },
      error => {
        console.log(error);
      }
    );
  }

  userHasItActivated(dashId: number) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    let dashboard: Dashboard;
    dashboard = this.dashboards.find(x => x.id === dashId);
    return dashboard.userDashboards.some(x => x.userId === currentUser.id);
  }

  getProcesses() {
    this.facade.processService.get()
      .subscribe(res => {
        this.processes = res;
        this.processCompleted = res.filter(process => process => process.status === ProcessStatusEnum.Declined ||
          process.status === ProcessStatusEnum.Hired || process.status === ProcessStatusEnum.Rejected).length;
        this.processFinishedSuccess = res.filter(process => process.status === ProcessStatusEnum.Hired).length;
        this.processInProgress = res.filter(process => process.status === ProcessStatusEnum.InProgress || process.status === ProcessStatusEnum.Recall || process.status === ProcessStatusEnum.Accepted).length;
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
    return i < 3;
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
