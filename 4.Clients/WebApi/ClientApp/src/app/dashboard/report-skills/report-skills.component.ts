import { Component, OnInit, TemplateRef, ViewChild, Input } from '@angular/core';
import { Skill } from 'src/entities/skill';
import { FacadeService } from 'src/app/services/facade.service';
import { CandidateSkill } from 'src/entities/candidateSkill';
import { Candidate } from 'src/entities/candidate';

@Component({
  selector: 'app-report-skills',
  templateUrl: './report-skills.component.html',
  styleUrls: ['./report-skills.component.css']
})
export class ReportSkillsComponent implements OnInit {

  @Input() _processes;

  // Ranking Chart
  skillRankedList: any[] = [
    { id: 0, name: '', points: 0 },
    { id: 0, name: '', points: 0 },
    { id: 0, name: '', points: 0 }
  ];

  topSkillsLoading = true;

  skillList: Skill[] = [];
  candidatesSkills: CandidateSkill[] = [];

  @Input() _detailedtopSkillLoading: boolean;

  constructor(private facade: FacadeService) { }

  ngOnInit() {
    this.getKnownSkills();
    this.getCandidatesSkills();
    this.getSkills();
  }

  getSkills() {
    this.facade.skillService.get()
      .subscribe(res => {
        this.skillList = res;
      });
  }

  getCandidatesSkills() {
    this.facade.candidateService.get()
      .subscribe(res => {
        res.forEach(candidate => {
          candidate.candidateSkills.forEach(skill => this.candidatesSkills.push(skill));
        });
      });
  }

  getKnownSkills(): boolean {
    try {
      const skillRanking: any = [];
      const cdSkills: CandidateSkill[] = this.candidatesSkills;
      const skills: Skill[] = this.skillList;
      if (cdSkills.length > 0 && cdSkills.length > 0) {
        skills.forEach(skill => {
          let points = 0;
          cdSkills.forEach(candSkill => {
            if (candSkill.skill.id === skill.id) { points = points + 1; }
          });
          skillRanking.push({
            id: skill.id,
            name: skill.name,
            points: points
          });
        });
      }
      if (skillRanking.length > 0) {
        skillRanking.sort((a, b) => a.points < b.points ? 1 : -1);
        this.skillRankedList = skillRanking.splice(0, 3);
      }
      this.topSkillsLoading = false;

      const totalSkillRank = this.skillRankedList[0].id + this.skillRankedList[1].id + this.skillRankedList[2].id;

      return totalSkillRank === 0;
    } catch {
      return false;
    }
  }

}
