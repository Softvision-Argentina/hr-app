import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ReaddressReasonType } from 'src/entities/ReaddressReasonType';

@Injectable()
export class ReaddressReasonTypeService extends BaseService<ReaddressReasonType> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'ReaddressReasonType';
  }

}
