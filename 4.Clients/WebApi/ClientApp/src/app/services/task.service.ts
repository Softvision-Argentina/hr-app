import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../app-config/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { catchError, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Task } from 'src/entities/task';

@Injectable()
export class TaskService extends BaseService<Task> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Tasks';
  }

  public approve(taskID: number): Observable<any> {
    return this.http.post(this.apiUrl + '/Approve/' + taskID, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(data => {}),
        catchError(this.handleErrors)
      );
  }

  public getByConsultant(consultantEmail: string): Observable<any> {
    return this.http.get(this.apiUrl + '/GetByConsultant/' + consultantEmail, {
      headers: this.headersWithAuth
    })
      .pipe(
        tap(data => {}),
        catchError(this.handleErrors)
      );
  }
}
