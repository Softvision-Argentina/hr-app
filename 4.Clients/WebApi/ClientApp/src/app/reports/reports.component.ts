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
import { Community } from 'src/entities/community';
import { Office } from 'src/entities/office';



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
  communities: Community[] = [];
  _offices: Office[] = [];
  isLoadingResults = false;
  selectedSkill: number;
  searchValue = '';
  listOfSearchCandidates = [];
  listOfDisplayData = [...this.filteredCandidates];
  defaultOffice: Office = {
    id: null,
    name: 'NA',
    description: '',
    roomItems: []
  }
  defaultCommunity: Community = {
    id: null,
    name: 'NA',
    description: '',
    profileId: 0,
    profile: null
  }


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
    this.getCommunities();
    this.getOffices();
    this.validateSkillsForm = this.fb.group({
      community: [this.defaultCommunity],
      preferredOffice: [this.defaultOffice]
    });
    this.addField()
    this.app.hideLoading();
  }
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
    this.validateSkillsForm.addControl(this.listOfControl[index - 1].controlInstance[0], new FormControl(null, Validators.required));
    this.validateSkillsForm.addControl(this.listOfControl[index - 1].controlInstance[1], new FormControl([0, 100]));
  }

  removeField(i: { id: number; controlInstance: string[] }, e: MouseEvent): void {
    e.preventDefault();
    if (this.listOfControl.length > 1) {
      const index = this.listOfControl.indexOf(i);
      this.listOfControl.splice(index, 1);
      this.validateSkillsForm.removeControl(i.controlInstance[0]);
      this.validateSkillsForm.removeControl(i.controlInstance[1]);
    }
  }

  showDetailsModal(candidateID: number, modalContent: TemplateRef<{}>): void {
    this.emptyCandidate = this.listOfDisplayData.filter(candidate => candidate.id === candidateID)[0];
    this.detailsModal.showModal(modalContent, this.emptyCandidate.name + " " + this.emptyCandidate.lastName);
  }

  getSkills() {
    this.facade.skillService.get()
      .subscribe(res => {
        this.skills = res;
      }, err => {
        console.log(err);
      }, () => {
        this.isLoadingResults = false;
      });
  }

  getCandidates() {
    this.facade.candidateService.get()
      .subscribe(res => {
        this.candidates = res;
      }, err => {
        console.log(err);
      });
  }

  getCommunities() {
    this.facade.communityService.get()
      .subscribe(res => {
        this.communities.push(this.defaultCommunity)
        this.communities.push(...res);
      }, err => {
        console.log(err);
      });
  }

  getOffices() {
    this.facade.OfficeService.get()
      .subscribe(res => {
        this._offices.push(this.defaultOffice)
        this._offices.push(...res);
      }, err => {
        console.log(err);
      });
  }

  getProcesses() {
    this.facade.processService.get()
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

    /* this.filteredCandidates = [];
    let selectedSkill: number = this.validateSkillsForm.get("skill0").value;
    let selectedSkills: number[] = this.listOfControl.map(control => this.validateSkillsForm.get(control.controlInstance[1]).value);
    
    let rateRange: string[] = this.validateSkillsForm.get("rate0").value.toString().split(',');
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
    }); */

    //this.listOfDisplayData = this.filteredCandidates;

    let selectedSkills: Array<{ skillId: number; minRate: number; maxRate: number }> =
      this.listOfControl.map(control => {
        const result = {
          skillId: parseInt(this.validateSkillsForm.get(control.controlInstance[0]).value),
          minRate: this.validateSkillsForm.get(control.controlInstance[1]).value[0],
          maxRate: this.validateSkillsForm.get(control.controlInstance[1]).value[1]
        }

        return result;
      });

    const filteredCandidateRequest: {
      community: number
      , preferredOffice: number
      , selectedSkills: Array<{ skillId: number; minRate: number; maxRate: number }>
    } =
    {
      community: parseInt(this.validateSkillsForm.get('community').value.id),
      preferredOffice: parseInt(this.validateSkillsForm.get('preferredOffice').value.id),
      selectedSkills: selectedSkills
    }



    this.facade.candidateService.getCandidatesBySkills(filteredCandidateRequest)
      .subscribe(res => {
        this.listOfDisplayData = res;
        let skilledCandidates: number = this.listOfDisplayData.filter(candidate => candidate.candidateSkills[0].rate >= 50).length;
        let totalCandidates: number = this.listOfDisplayData.length;

        //Cards de porcentajes
        this.stadisticAbove = (skilledCandidates * 100) / totalCandidates;
        if (this.stadisticAbove === 100) this.stadisticBelow = 0;
        else this.stadisticBelow = ((totalCandidates - skilledCandidates) * 100) / totalCandidates;
        if (this.stadisticBelow === 100) this.stadisticAbove = 0;
        if (this.stadisticAbove.toString() === 'NaN') this.stadisticAbove = 0;
        if (this.stadisticBelow.toString() === 'NaN') this.stadisticBelow = 0;
        this.app.hideLoading();
      }, err => {
        console.log(err);
      });
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


