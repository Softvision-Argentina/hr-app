import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { Router } from '@angular/router';
import { Skill } from '@shared/models/skill.model';
import { Observable, timer } from 'rxjs';
import { shareReplay, switchMap } from 'rxjs/operators';

const CACHE_SIZE = 1;
const REFRESH_INTERVAL = 3600000;
@Injectable()
export class SkillService extends BaseService<Skill> {
  private cache$: Observable<Array<Skill>>;

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Skills';
  }

  getSkills(profileId: number){
    const skillsByProfileApi = this.apiUrl.replace('Skills', `SkillProfile/${profileId}`);
    return this.http.get<any>(skillsByProfileApi);
  }

  getSkillList(): Observable<Skill[]> {
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