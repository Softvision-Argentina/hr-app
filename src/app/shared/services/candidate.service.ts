import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { Router } from '@angular/router';
import { Observable, timer } from 'rxjs';
import { tap, catchError, shareReplay, switchMap } from 'rxjs/operators';
import { Candidate } from '@shared/models/candidate.model';
import { ICandidate } from '../interfaces/ICandidate.service';

const CACHE_SIZE = 1;
const REFRESH_INTERVAL = 3600000;
@Injectable()
export class CandidateService extends BaseService<Candidate> implements ICandidate {
  private cache$: Observable<Array<Candidate>>;

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Candidates';
  }

  public getCandidates(): Observable<Candidate[]> {
    if (!this.cache$) {
      const timer$ = timer(0, REFRESH_INTERVAL);
      this.cache$ = timer$.pipe(
        switchMap(_ => this.get()),
        shareReplay(CACHE_SIZE)
      );
    }
    return this.cache$;
  }

  public idExists(id: number): Observable<any> {
    return this.http.get(this.apiUrl + '/exists/' + id, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(data => { }),
        catchError(this.handleErrors)
      );
  }

  public exists(email: string, id: number): Observable<any> {
    return this.http.get(this.apiUrl + '/EmailExists/' + email + '/' + id, {
      headers: this.headersWithAuth,
      observe: 'response'
    })
      .pipe(
        tap(data => { }),
        catchError(this.handleErrors)
      );
  }

  public reactivate(id: number): Observable<any> {
    return this.http.post(this.apiUrl + '/Reactivate/', id, {
      headers: this.headersWithAuth, observe: 'response'
    })
      .pipe(
        tap(() => this.get().subscribe()),
        catchError(this.handleErrors)
      );
  }

  public getCandidatesBySkills(candidatesFilters): Observable<any> {

    return this.http.post(this.apiUrl + '/filter/', candidatesFilters, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(data => { }),
        catchError(this.handleErrors)
      );
  }

  public bulkAdd(data): Observable<any> {
    const headers = { Authorization: this.headersWithAuth.get("Authorization") }
    return this.http.post<any>(this.apiUrl + '/BulkAdd', data, {
      headers: headers
    })
      .pipe(
        tap(data => { }),
        catchError(this.handleErrors)
      )
  }
}
