import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { Router } from '@angular/router';
import { PreOffer } from 'src/entities/pre-offer';

@Injectable()
export class PreOfferService extends BaseService<PreOffer> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'PreOffer';
  }

}
