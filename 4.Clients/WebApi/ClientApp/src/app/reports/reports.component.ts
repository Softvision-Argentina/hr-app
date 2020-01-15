import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Skill } from 'src/entities/skill';
import { Candidate } from 'src/entities/candidate';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { BaseChartDirective, Label, SingleDataSet } from 'ng2-charts';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { FacadeService } from '../services/facade.service';
import { CandidateDetailsComponent } from '../candidates/details/candidate-details.component';
import { Process } from 'src/entities/process';
import { AppComponent } from '../app.component';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
import { replaceAccent } from 'src/app/helpers/string-helpers'



@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css'],
  providers: [CandidateDetailsComponent, AppComponent]
})

export class ReportsComponent implements OnInit {

  //Skill Chart Preferences
  @ViewChild(BaseChartDirective) chart: BaseChartDirective;
  public skillChartLabels: Label[] = [];
  public skillChartLegend = false;
  public skillChartType: ChartType = 'bar';
  public skillChartPlugins = [pluginDataLabels];
  public skillsPercentage: ChartDataSets[] = [{ data: [], label: 'Cantidad ' }];
  public skillChartOptions: ChartOptions = {
    responsive: true,
    scales: {
      yAxes: [{
        id: 'y-axis-0',
        ticks: {
          beginAtZero: true
        }
      }],
      xAxes: [{
        ticks: {
          autoSkip: false
        }
      }]
    },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public skillsChartColors: any[] = [
    {
      backgroundColor: 'rgb(103, 139, 240)',
      borderColor: 'rgb(103, 58, 183)',
      pointBackgroundColor: 'rgb(103, 58, 183)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(103, 58, 183, .8)'
    },
  ];


  //CandidateFilter
  @ViewChild('dropdown') nameDropdown;
  validateSkillsForm: FormGroup;
  listOfControl: Array<{ id: number; controlInstance: string[] }> = [];

  emptyCandidate: Candidate;
  skills: Skill[] = [];
  candidates: Candidate[] = [];
  processes: Process[] = [];
  filteredCandidates: Candidate[] = [];
  isLoadingResults = false;
  selectedSkill: number;
  searchValue = '';
  listOfSearchCandidates = [];
  listOfDisplayData = [...this.filteredCandidates];


  numberOfWait: number = 0;
  numberOfError: number = 0;
  numberOfFinish: number = 0;
  numberOfInProcess: number = 0;
  numberOfNotStarted: number = 0;

  stadisticAbove: number = 0;
  stadisticBelow: number = 0;

  sortName = null;
  sortValue = null;

  constructor(private facade: FacadeService, private fb: FormBuilder, private detailsModal: CandidateDetailsComponent,
    private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.isLoadingResults = true;
    this.app.removeBgImage();
    this.getSkills();
    this.getCandidates();
    this.getProcesses();
/* 
    this.validateSkillsForm = this.fb.group({
      skillSelector: [null, [Validators.required]],
      skillRateSlidder: [[0, 100]]
    }); */

    this.validateSkillsForm =  this.fb.group({});
    this.addField()
    this.app.hideLoading();
  }

/*   get skillSelectors(){
    return this.validateSkillsForm.get('skillSelectors') as FormArray
  } */

  addField(e?: MouseEvent): void {
    if (e) {
      e.preventDefault();
    }
    const id = this.listOfControl.length > 0 ? this.listOfControl[this.listOfControl.length - 1].id + 1 : 0;

    const control = {
      id,
      controlInstance: [`skill${id}`, `rate${id}`]
    };
    const index = this.listOfControl.push(control);
    console.log(this.listOfControl[this.listOfControl.length - 1]);
    this.validateSkillsForm.addControl(this.listOfControl[index - 1].controlInstance[0],new FormControl(null, Validators.required));
    this.validateSkillsForm.addControl(this.listOfControl[index - 1].controlInstance[1],new FormControl(null, Validators.required));
  }

  showDetailsModal(candidateID: number, modalContent: TemplateRef<{}>): void {
    this.emptyCandidate = this.filteredCandidates.filter(candidate => candidate.id == candidateID)[0];
    this.detailsModal.showModal(modalContent, this.emptyCandidate.name + " " + this.emptyCandidate.lastName);
  }

  getSkills() {
    this.facade.skillService.get<Skill>()
      .subscribe(res => {
        this.skills = res;
      }, err => {
        console.log(err);
      }, () => {
        this.isLoadingResults = false;
      });
  }

  getCandidates() {
    this.facade.candidateService.get<Candidate>()
      .subscribe(res => {
        this.candidates = res;
      }, err => {
        console.log(err);
      });
  }

  getProcesses() {
    this.facade.processService.get<Process>()
      .subscribe(res => {
        this.processes = res;
        let labels: string[] = [];
        let percentages: number[] = [];
        let colors: string[] = [];

        this.numberOfInProcess = this.processes.filter(p => p.status === ProcessStatusEnum.Declined).length;
        if (this.numberOfInProcess > 0) {
          labels.push('DECLINED');
          colors.push("#81FB15");
          percentages.push(this.numberOfInProcess);
        }
        this.numberOfError = this.processes.filter(p => p.status === ProcessStatusEnum.Rejected).length;
        if (this.numberOfError > 0) {
          labels.push('REJECTED');
          colors.push("#E4363FDB");
          percentages.push(this.numberOfError);
        }
        this.numberOfWait = this.processes.filter(p => p.status === ProcessStatusEnum.InProgress).length;
        if (this.numberOfWait > 0) {
          labels.push('IN PROGRESS');
          colors.push("#F6FB15");
          percentages.push(this.numberOfWait);
        }
        this.numberOfFinish = this.processes.filter(p => p.status === ProcessStatusEnum.Hired).length;
        if (this.numberOfFinish > 0) {
          labels.push('HIRED');
          colors.push("#36E4BDFC");
          percentages.push(this.numberOfFinish);
        }
        this.numberOfNotStarted = this.processes.filter(p => p.status === ProcessStatusEnum.Recall).length;
        if (this.numberOfNotStarted > 0) {
          labels.push('RECALL');
          colors.push("#6FC8CE");
          percentages.push(this.numberOfNotStarted);
        }

        this.pieChartLabels = labels;
        this.pieChartData = percentages;
        this.pieChartColors = [
          {
            backgroundColor: colors
          }
        ];
      }, err => {
        console.log(err);
      });
  }

  getCandidatesBySkill(): void {
    this.app.showLoading();
    for (const i in this.validateSkillsForm.controls) {
      this.validateSkillsForm.controls[i].markAsDirty();
      this.validateSkillsForm.controls[i].updateValueAndValidity();
    }

    this.filteredCandidates = [];
    let selectedSkill: number = this.validateSkillsForm.controls['skillSelector'].value;
    let rateRange: string[] = this.validateSkillsForm.controls['skillRateSlidder'].value.toString().split(',');
    let skilledCandidates: number = 0;
    let totalCandidates: number = 0;
    this.candidates.forEach(candidate => {
      candidate.candidateSkills.forEach(cdSkill => {
        if (cdSkill.skill.id.toString() === selectedSkill.toString()) {
          totalCandidates = totalCandidates + 1;
          if (cdSkill.rate >= 50) skilledCandidates = skilledCandidates + 1;
          if ((cdSkill.rate >= parseInt(rateRange[0])) && (cdSkill.rate <= parseInt(rateRange[1]))) {
            this.filteredCandidates.push(candidate);
          }
        }
      })
    });
    this.listOfDisplayData = this.filteredCandidates;

    //Cards de porcentajes
    this.stadisticAbove = (skilledCandidates * 100) / totalCandidates;
    if (this.stadisticAbove === 100) this.stadisticBelow = 0;
    else this.stadisticBelow = ((totalCandidates - skilledCandidates) * 100) / totalCandidates;
    if (this.stadisticBelow === 100) this.stadisticAbove = 0;
    if (this.stadisticAbove.toString() == 'NaN') this.stadisticAbove = 0;
    if (this.stadisticBelow.toString() == 'NaN') this.stadisticBelow = 0;
    this.app.hideLoading();
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchCandidates.length ? this.listOfSearchCandidates.some(candidates => item.name.indexOf(candidates) !== -1) : true) &&
        (replaceAccent(item.name.toString().toUpperCase() + item.lastName.toString().toUpperCase()).indexOf(replaceAccent(this.searchValue.toUpperCase())) !== -1);
    };
    const data = this.filteredCandidates.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.nameDropdown.nzVisible = false;
  }

  sort(sortName: string, value: boolean): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  isPieData(): boolean {
    let total = this.numberOfInProcess + this.numberOfError + this.numberOfWait + this.numberOfFinish + this.numberOfNotStarted;
    if (total > 0) return true;
    else return false;
  }

  getSkillsPercentage(): void {
    this.app.showLoading();
    let skills: Skill[] = this.skills;
    let candidates: Candidate[] = this.candidates;
    let chartLabels: Label[] = [];
    let cantidad: number;
    let skillRates: number[] = [];

    skills.forEach(skill => {
      cantidad = 0;
      candidates.forEach(candidate => {
        candidate.candidateSkills.forEach(cdSkill => {
          if (cdSkill.skill.id.toString() === skill.id.toString()) {
            cantidad = cantidad + 1;
          }
        });
      });
      if (cantidad > 0) {
        skillRates.push(cantidad);
        chartLabels.push(skill.name);
      }
    });
    this.skillsPercentage = [{ data: skillRates, label: 'Number of candidates ' }];
    this.skillChartLabels = chartLabels;
    this.app.hideLoading();
  }


  //Processes
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
  public pieChartLabels: Label[] = ['REJECTED', 'IN PROCESS', 'FINISH', 'WAIT', 'NOT STARTED'];
  public pieChartData: SingleDataSet = [0, 0, 0, 0, 0];
  public pieChartType: ChartType = 'pie';
  public pieChartLegend = true;
  public pieChartPlugins = [pluginDataLabels];
  public pieChartColors: Array<any> = [
    {
      backgroundColor: ["#E4363FDB", "#81FB15", "#36E4BDFC", "#F6FB15", "#6FC8CE"]
    }
  ];

  // events
  public chartClicked({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }

  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }
}


