import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OpenPosition } from 'src/entities/open-position';

@Injectable()
export class OpenPositionService extends BaseService<OpenPosition> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'OpenPosition';
  }

}