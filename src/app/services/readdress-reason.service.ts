import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ReaddressReason } from 'src/entities/ReaddressReason';

@Injectable()
export class ReaddressReasonService extends BaseService<ReaddressReason> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'ReaddressReason';
  }
}
