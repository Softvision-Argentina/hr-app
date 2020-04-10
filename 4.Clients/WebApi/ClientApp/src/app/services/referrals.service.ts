import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { tap, catchError } from 'rxjs/operators';
import { Candidate } from 'src/entities/candidate';

@Injectable()
export class ReferralsService extends BaseService<Candidate> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Referrals';
  }
  
  public idExists(id: number): Observable<any>{
    return this.http.get(this.apiUrl + '/exists/' + id, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(_ => {}),
        catchError(this.handleErrors)
      );
  }

  public getCandidatesBySkills(candidatesFilters): Observable<any>{

    return this.http.post(this.apiUrl + '/filter/' , candidatesFilters ,{
      headers: this.headersWithAuth
    })
      .pipe(
        tap(_ => {}),
        catchError(this.handleErrors)
      );
  }
}
