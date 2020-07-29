import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { HireProjection } from '@shared/models/hire-projection.model';


@Injectable()
export class HireProjectionService extends BaseService<HireProjection> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'HireProjections';
  }

}
