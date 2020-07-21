import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Candidate } from 'src/entities/candidate';

@Injectable()
export class CandidateService extends BaseService<Candidate> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Candidates';
  }
  
  public idExists(id: number): Observable<any>{
    return this.http.get(this.apiUrl + '/exists/' + id, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(data => {}),
        catchError(this.handleErrors)
      );
  }

  public exists(email: string): Observable<any>{
    return this.http.get(this.apiUrl + '/EmailExists/' + email, {
      headers: this.headersWithAuth,
      observe: "response"
    })
      .pipe(
        tap(data => {}),
        catchError(this.handleErrors)
      );
  }

  public getCandidatesBySkills(candidatesFilters): Observable<any>{

    return this.http.post(this.apiUrl + '/filter/' , candidatesFilters ,{
      headers: this.headersWithAuth
    })
      .pipe(
        tap(data => {}),
        catchError(this.handleErrors)
      );
  }
}
