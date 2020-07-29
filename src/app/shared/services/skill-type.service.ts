import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SkillType } from '@shared/models/skill-type.model';


@Injectable()
export class SkillTypeService extends BaseService<SkillType> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'SkillTypes';
  }

}
