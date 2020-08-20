import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, Subject } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Candidate } from '@shared/models/candidate.model';
import { ICandidate } from '../interfaces/ICandidate.service';

@Injectable()
export class ReferralsService extends BaseService<Candidate> implements ICandidate{
  currentReferralList: Candidate[] = [];
  emptyReferredInfo: Candidate;

  private referralList = new BehaviorSubject<Candidate[]>(this.currentReferralList);
  referrals = this.referralList.asObservable();

  public _startReferralsModalSource = new BehaviorSubject<boolean>(false);
  startReferralsModal$ = this._startReferralsModalSource.asObservable();

  public _displayNavAndSideMenuSource = new BehaviorSubject<boolean>(false);
  _displayNavAndSideMenu$ = this._displayNavAndSideMenuSource.asObservable();

  public _createNewReferralSource = new BehaviorSubject<boolean>(false);
  _createNewReferral$ = this._createNewReferralSource.asObservable();

  public _candidateInfoSource = new BehaviorSubject<Candidate>(this.emptyReferredInfo);
  _candidateInfo$ = this._candidateInfoSource.asObservable();

  candidateAdded = new Subject<Candidate>();
  candidateDelete = new Subject<number>();
  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Referrals';
  }

  public exists(email: string, id: number): Observable<any> {
    const candidateApi = this.apiUrl.replace('Referrals', 'Candidates');
    return this.http.get(candidateApi + '/EmailExists/' + email + '/' + id, {
      headers: this.headersWithAuth,
      observe: 'response'
    })
      .pipe(
        tap(data => { }),
        catchError(this.handleErrors)
      );
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

  // Overrrides default method to prevent gettings referrals
  public add(newCandidate: Candidate): Observable<Candidate> {
    return this.http
      .post<Candidate>(this.apiUrl, newCandidate, {
        headers: this.headersWithAuth,
      })
      .pipe(
        tap(res => { 
          this.candidateAdded.next(newCandidate);
          this.data.next([...this.data.value, newCandidate]);
        } ),
        catchError(this.handleErrors)
      );
  }

  public saveCv(candidateId: number, formData: FormData): Observable<any> {

    const headers = { Authorization: this.headersWithAuth.get("Authorization") }
    const cvApi = this.apiUrl.replace('Referrals', '').replace('Candidate', '') + 'cv/' + candidateId;

    return this.http
      .post<any>(cvApi, formData, {
        headers: headers
      })
      .pipe(
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

  public startReferralsModal(instruction: boolean) {
    this._startReferralsModalSource.next(instruction);
  }

  public displayNavAndSideMenu(instruction: boolean) {
    this._displayNavAndSideMenuSource.next(instruction);
  }

  public createNewReferral(instruction: boolean) {
    this._createNewReferralSource.next(instruction);
  }

  public update(referralId: number, newCandidate: Candidate): Observable<Candidate> {
    return super.update(referralId, newCandidate)
      .pipe(
        tap(res => this.candidateAdded.next(newCandidate))
      );
  }

  public get(): Observable<any>{
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    return this.http.get(this.apiUrl + `/${currentUser.id}`)
      .pipe(
        tap(res => this.data.next(res.map(e => e.candidate)))
      );
  }

  public delete(referralId: number): Observable<any> {
    // Should we keep using candidate endpoint?
    const referralApi  = this.apiUrl.replace('Referrals', `Candidates/${referralId}`);
    return this.http.delete<any>(referralApi)
    .pipe(
      tap(res => this.candidateDelete.next(referralId))
    );
  }

  public sendCandidateInfo(info: Candidate) {
    this._candidateInfoSource.next(info);
  }

}
