import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DeclineReason } from '@shared/models/decline-reason.model';


@Injectable()
export class DeclineReasonService extends BaseService<DeclineReason> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'DeclineReason';
  }

}
