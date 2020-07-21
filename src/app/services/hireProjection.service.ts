import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { HireProjection } from 'src/entities/hireProjection';


@Injectable()
export class HireProjectionService extends BaseService<HireProjection> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'HireProjections';
  }

}
