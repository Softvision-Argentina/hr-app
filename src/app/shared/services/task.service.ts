import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '@shared/utils/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { catchError, tap } from 'rxjs/operators';
import { Observable, timer } from 'rxjs';
import { Task } from '@shared/models/task.model';
import { shareReplay, switchMap } from 'rxjs/operators';

const CACHE_SIZE = 1;
const REFRESH_INTERVAL = 3600000;
@Injectable()
export class TaskService extends BaseService<Task> {
  private cache$: Observable<Array<Task>>;

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Tasks';
  }

  public approve(taskID: number): Observable<any> {
    return this.http.post(this.apiUrl + '/Approve/' + taskID, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(data => { }),
        catchError(this.handleErrors)
      );
  }

  public getTasks(role: string, email: string) {
    if (!this.cache$) {
      const timer$ = timer(0, REFRESH_INTERVAL);
      if (role !== 'Admin' && role !== 'HRManagement') {
        this.cache$ = timer$.pipe(
          switchMap(_ => this.getByUser(email)),
          shareReplay(CACHE_SIZE)
        );
      } else {
        this.cache$ = timer$.pipe(
          switchMap(_ => this.get()),
          shareReplay(CACHE_SIZE)
        );
      }
    }
    return this.cache$;
  }

  public getByUser(userEmail: string): Observable<any> {
    return this.http.get(this.apiUrl + '/GetByUser/' + userEmail, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(data => { }),
        catchError(this.handleErrors)
      );
  }
}
