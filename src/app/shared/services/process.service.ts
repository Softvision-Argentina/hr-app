import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '@shared/utils/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, timer } from 'rxjs';
import { forkJoin } from 'rxjs/internal/observable/forkJoin';
import { catchError, tap, shareReplay, switchMap } from 'rxjs/operators';
import { Process } from '@shared/models/process.model';
import { Candidate } from '@shared/models/candidate.model';
import { Globals } from '../utils/globals';
import { User } from '@shared/models/user.model';
import { ProcessTableView } from '@shared/models/process-tableView.model';

const CACHE_SIZE = 1;
const REFRESH_INTERVAL = 300000;
@Injectable()
export class ProcessService extends BaseService<Process> {
  private cache$: Observable<ProcessTableView[]>;
  private deletedProcessesCache$: Observable<ProcessTableView[]>;
  private selectedSenioritysSource: BehaviorSubject<any[]>;
  selectedSeniorities: Observable<any[]>;
  candidatesUrl = '';

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

  public getActiveProcessByCandidate(candidateId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/candidate/${candidateId}`,
      { headers: this.headersWithAuth, observe: 'body' })
      .pipe(
        tap(data => { }),
        catchError(this.handleErrors)
      );
  }

  public getProcessByUserRole(currentUser: User): Observable<any> {
    if (currentUser.role === "CommunityManager") {
      return this.http.get(`${this.apiUrl}/com/${currentUser.community.name}`,
        { headers: this.headersWithAuth, observe: "body" })
        .pipe(
          tap(data => { }),
          catchError(this.handleErrors)
        );
    }
    if (currentUser.role === "Interviewer") {
      return this.http.get(`${this.apiUrl}/owner/${currentUser.id}`,
        { headers: this.headersWithAuth, observe: "body" })
        .pipe(
          tap(data => { }),
          catchError(this.handleErrors)
        );
    }
    return this.get();
  }
  public approve(processID: number) {
    return this.http.post<ProcessTableView>(this.apiUrl + '/Approve', processID, {
      headers: this.headersWithAuth
    });
  }

  public reactivate(processID: number): Observable<any> {
    return this.http.post(this.apiUrl + '/Reactivate', processID, {
      headers: this.headersWithAuth, observe: 'body'
    })
      .pipe(
        tap(data => { }),
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
        tap(() => this.get().subscribe()),
        catchError(this.handleErrors)
      );
  }

  public updateProcessCandidate(processID: number, process: Process, candidateID: number, candidate: Candidate): Observable<any> {
    const processUrl = `${this.apiUrl}/${processID.toString()}`;
    const candidateUrl = `${this.candidatesUrl}/${candidateID.toString()}`;

    const processCall = this.http.put(processUrl, process, {
      headers: this.headersWithAuth
    }).pipe(
      tap(() => this.get().subscribe()),
      catchError(this.handleErrors)
    );

    const candidateCall = this.http.put(candidateUrl, candidate, {
      headers: this.headersWithAuth
    }).pipe(
      tap(_ => { }),
      catchError(this.handleErrors)
    );

    return forkJoin([processCall, candidateCall]);
  }

  public getTableView() {
    if (!this.cache$) {
      const url = this.apiUrl + '/tableView';
      const timer$ = timer(0, REFRESH_INTERVAL);
      this.cache$ = timer$.pipe(
        switchMap(_ => this.http.get<ProcessTableView[]>(url, {
          headers: this.headersWithAuth
        })),
        shareReplay(CACHE_SIZE)
      );
    }
    return this.cache$;
  }

  public getDeletedProcesses(): Observable<any> {
    if (!this.deletedProcessesCache$) {
      const url = `${this.apiUrl}/DeletedProcesses/`;
      const timer$ = timer(0, REFRESH_INTERVAL);
      this.deletedProcessesCache$ = timer$.pipe(
        switchMap(_ => this.http.get<ProcessTableView[]>(url, {
          headers: this.headersWithAuth
        })),
        shareReplay(CACHE_SIZE)
      );
      return this.deletedProcessesCache$;
    }
  }

  public addProcess(process: Process) {
    const url = this.apiUrl + '/tableView';
    return this.http.post<ProcessTableView>(url, process, {
      headers: this.headersWithAuth
    });
  }
}


