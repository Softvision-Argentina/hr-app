import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Interview } from '@shared/models/interview.model';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { handleError } from '@shared/utils/utils.functions';

@Injectable()
export class InterviewService extends BaseService<Interview> {
  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Interviews';
  }

  public updateMany(clientStageId : number, interviews : Interview[]): Observable<any> {
    return this.http
      .put(this.apiUrl + "/Update/" + clientStageId, interviews, {
        headers: this.headersWithAuth,
      })
      .pipe(
        tap(res => {
          this.get().subscribe();
        }),
        catchError((errorResponse) => handleError(errorResponse, throwError))
      );
  }
}
