import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '@shared/utils/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Office } from '@shared/models/office.model';
import { Observable, timer } from 'rxjs';
import { shareReplay, switchMap } from 'rxjs/operators';

const CACHE_SIZE = 1;
const REFRESH_INTERVAL = 3600000;

@Injectable()
export class OfficeService extends BaseService<Office> {
    private cache$: Observable<Array<Office>>;

    constructor(router: Router, config: AppConfig, http: HttpClient) {
        super(router, config, http);
        this.apiUrl += 'Office';
    }

    getOffices(): Observable<Office[]> {
        if (!this.cache$) {
          const timer$ = timer(0, REFRESH_INTERVAL);
          this.cache$ = timer$.pipe(
            switchMap(_ => this.get()),
            shareReplay(CACHE_SIZE)
          );
        }
        return this.cache$;
      }
}
