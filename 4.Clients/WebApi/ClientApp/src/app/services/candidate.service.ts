import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable()
export class CandidateService extends BaseService {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Candidates';
  }

  // public dniExists(dni: number): Observable<any>{
  //   return this.http.get(this.apiUrl + '/exists/' + dni, {
  //     headers: this.headersWithAuth
  //   })
  //     .pipe(
  //       tap(data => {}),
  //       catchError(this.handleErrors)
  //     );
  // }

  public idExists(id: number): Observable<any>{
    return this.http.get(this.apiUrl + '/exists/' + id, {
      headers: this.headersWithAuth
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
