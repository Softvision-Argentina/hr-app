import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { tap, catchError } from 'rxjs/operators';
import { Candidate } from 'src/entities/candidate';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class ReferralsService extends BaseService<Candidate> {
  currentReferralList: Candidate[] = [];
  private referralList = new BehaviorSubject<Candidate[]>(this.currentReferralList);
  referrals = this.referralList.asObservable();

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Referrals';
  }

  public idExists(id: number): Observable<any> {
    return this.http.get(this.apiUrl + '/exists/' + id, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(_ => { }),
        catchError(this.handleErrors)
      );
  }

  public getCandidatesBySkills(candidatesFilters): Observable<any> {

    return this.http.post(this.apiUrl + '/filter/', candidatesFilters, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(_ => { }),
        catchError(this.handleErrors)
      );
  }

  public addNew(newReferral: Candidate): void {
    this.currentReferralList = this.referralList.getValue();
    this.currentReferralList.push(newReferral);
    this.referralList.next(this.currentReferralList);
  }

  public updateList(referrals: Candidate[]): void {
    this.referralList.next(referrals);
  }
}
