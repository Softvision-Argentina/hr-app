import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DeclineReason } from '@shared/models/decline-reason.model';
import { Observable, timer } from 'rxjs';
import { shareReplay, switchMap } from 'rxjs/operators';

const CACHE_SIZE = 1;
const REFRESH_INTERVAL = 3600000;

@Injectable()
export class DeclineReasonService extends BaseService<DeclineReason> {
  private cache$: Observable<Array<DeclineReason>>;

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'DeclineReason';
  }

  getDeclineReasons(type: string): Observable<DeclineReason[]> {
    if (!this.cache$) {
      const timer$ = timer(0, REFRESH_INTERVAL);
      this.cache$ = timer$.pipe(
        switchMap(_ => this.get(type)),
        shareReplay(CACHE_SIZE)
      );
    }
    return this.cache$;
  }

}
