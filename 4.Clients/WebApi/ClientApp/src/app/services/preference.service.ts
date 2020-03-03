import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { Preference } from 'src/entities/preference';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PreferenceService extends BaseService<Preference> {
  preferenceSource = new Subject<Preference>();
  preference = this.preferenceSource.asObservable();

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Preference';
  }

  changePreference(pref:Preference){
    this.preferenceSource.next(pref);
  }

}
