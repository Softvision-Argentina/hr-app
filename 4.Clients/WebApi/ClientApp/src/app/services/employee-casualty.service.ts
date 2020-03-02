import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { EmployeeCasualty } from 'src/entities/employeeCasualty';


@Injectable()
export class EmployeeCasualtyService extends BaseService<EmployeeCasualty> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'employeeCasualty';
  }

}
