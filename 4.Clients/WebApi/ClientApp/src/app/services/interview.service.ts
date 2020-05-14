import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Interview } from 'src/entities/interview';

@Injectable()
export class InterviewService extends BaseService<Interview> {
  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Interview';
  }
}
