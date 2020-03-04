import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../app-config/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { CompanyCalendar } from 'src/entities/Company-Calendar';

@Injectable()
export class CompanyCalendarService extends BaseService<CompanyCalendar> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'CompanyCalendar';
  }

}