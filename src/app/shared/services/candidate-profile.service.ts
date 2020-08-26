import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '@shared/utils/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Subject, BehaviorSubject } from 'rxjs';

@Injectable()
export class CandidateProfileService extends BaseService<CandidateProfile> {

  currentCandidateProfileId = new BehaviorSubject<number>(null);
  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'CandidateProfile';
  }

  getProfileByCommunity(communityId: number){
    const profileByCommunityApi = this.apiUrl.replace('CandidateProfile', `ProfileCommunity/${communityId}`);
    return this.http.get<any>(profileByCommunityApi);
  }
}
