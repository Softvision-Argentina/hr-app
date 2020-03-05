import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../app-config/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Consultant } from 'src/entities/consultant';

@Injectable()
export class ConsultantService extends BaseService<Consultant> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Consultant';
  }

  public GetByEmail(email: string){
    return this.http.get(this.apiUrl
      + '/GetByEmail?email=' + email, { headers: this.headers, observe: "response" })
  }
}
