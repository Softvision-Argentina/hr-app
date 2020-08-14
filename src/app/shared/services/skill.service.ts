import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { Router } from '@angular/router';
import { Skill } from '@shared/models/skill.model';

@Injectable()
export class SkillService extends BaseService<Skill> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Skills';
  }

  getSkills(profileId: number){
    const skillsByProfileApi = this.apiUrl.replace('Skills', `SkillProfile/${profileId}`);
    return this.http.get<any>(skillsByProfileApi);
  }
}