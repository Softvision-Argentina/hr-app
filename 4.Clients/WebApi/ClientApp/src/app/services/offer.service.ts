import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { Router } from '@angular/router';
import { Offer } from 'src/entities/offer';

@Injectable()
export class OfferService extends BaseService<Offer> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Offer';
  }

}