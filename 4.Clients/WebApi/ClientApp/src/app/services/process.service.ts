import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../app-config/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { forkJoin } from 'rxjs/internal/observable/forkJoin';
import { catchError, tap } from 'rxjs/operators';
import { Process } from 'src/entities/process';
import { Candidate } from 'src/entities/candidate';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Globals } from '../app-globals/globals';

@Injectable()
export class ProcessService extends BaseService<Process> {

  private selectedSenioritysSource: BehaviorSubject<any[]>;
  selectedSeniorities: Observable<any[]>;

  constructor(router: Router, config: AppConfig, http: HttpClient, globals: Globals) {
    super(router, config, http);
    this.selectedSenioritysSource = new BehaviorSubject(globals.seniorityList);
    this.selectedSeniorities = this.selectedSenioritysSource.asObservable();
    this.candidatesUrl += this.apiUrl + 'Candidates';
    this.apiUrl += 'Process';
  }

  changeSeniority(seniority: any) {
    this.selectedSenioritysSource.next(seniority);
  }

  candidatesUrl = '';

  public getActiveProcessByCandidate(candidateId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${candidateId}`,
      {headers: this.headersWithAuth, observe: 'body'})
      .pipe(
        tap(data => {}),
        catchError(this.handleErrors)
      );
  }

  public approve(processID: number): Observable<any> {
    return this.http.post(this.apiUrl + '/Approve', processID, {
      headers: this.headersWithAuth, observe: 'response'
    })
      .pipe(
        tap(data => {}),
        catchError(this.handleErrors)
      );
  }

  public reject(processID: number, rejectionReason: string): Observable<any> {
    const rejectProcessVm = {
      id: processID,
      rejectionReason: rejectionReason
    };
    return this.http.post(this.apiUrl + '/Reject', rejectProcessVm, {
      headers: this.headersWithAuth, observe: 'response'
    })
      .pipe(
        tap(entities => {}),
        catchError(this.handleErrors)
      );
  }

  public updateProcessCandidate(processID: number, process: Process, candidateID: number, candidate: Candidate): Observable<any>{
    const processUrl = `${this.apiUrl}/${processID.toString()}`;
    const candidateUrl =  `${this.candidatesUrl}/${candidateID.toString()}`;

    const processCall = this.http.put(processUrl, process, {
      headers: this.headersWithAuth
    }).pipe(
      tap(_ => {}),
      catchError(this.handleErrors)
    );

    const candidateCall = this.http.put(candidateUrl, candidate, {
      headers: this.headersWithAuth
    }).pipe(
      tap(_ => {}),
      catchError(this.handleErrors)
    );

    return forkJoin([processCall, candidateCall]);
  }
}
