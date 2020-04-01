import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../app-config/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { DefaultStage } from 'src/entities/defaultStage';

@Injectable()
export class DefaultStageService extends BaseService<DefaultStage> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'DefaultStages';
  }
}
