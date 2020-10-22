import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '@shared/utils/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Subject, BehaviorSubject, Observable, timer } from 'rxjs';
import { shareReplay, switchMap } from 'rxjs/operators';

const CACHE_SIZE = 1;
const REFRESH_INTERVAL = 3600000;
@Injectable()
export class CandidateProfileService extends BaseService<CandidateProfile> {

  currentCandidateProfileId = new BehaviorSubject<number>(null);
  private cache$: Observable<Array<CandidateProfile>>;

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'CandidateProfile';
  }

  getProfileByCommunity(communityId: number){
    const profileByCommunityApi = this.apiUrl.replace('CandidateProfile', `ProfileCommunity/${communityId}`);
    return this.http.get<any>(profileByCommunityApi);
  }

  getCandidateProfiles(): Observable<CandidateProfile[]> {
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
