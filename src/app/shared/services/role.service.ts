import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '@shared/utils/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Role } from '@shared/models/role.model';
import { Observable, timer } from 'rxjs';
import { shareReplay, switchMap } from 'rxjs/operators';

const CACHE_SIZE = 1;
const REFRESH_INTERVAL = 3600000;

@Injectable()
export class RoleService extends BaseService<Role> {
    private cache$: Observable<Array<Role>>;

    constructor(router: Router, config: AppConfig, http: HttpClient) {
        super(router, config, http);
        this.apiUrl += 'Role';
    }

    getRoles(): Observable<Role[]> {
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