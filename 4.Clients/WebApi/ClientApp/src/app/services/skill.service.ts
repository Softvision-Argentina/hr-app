import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { Router } from '@angular/router';
import { Skill } from 'src/entities/skill';

@Injectable()
export class SkillService extends BaseService<Skill> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Skills';
  }

}
