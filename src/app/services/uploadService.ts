import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { Globals } from '../app-globals/globals';
import { Cv } from 'src/entities/cv';
import { Candidate } from 'src/entities/candidate';

@Injectable({
  providedIn: 'root'
})
export class UploadService extends BaseService<Cv> {
  cand: Candidate;

  constructor(router: Router, config: AppConfig, http: HttpClient, globals: Globals) {
    super(router, config, http);
    // TODO: What's the api url?
    // this.apiUrl;
  }

  public uploadFile(data: any) {
    return this.http.post(this.apiUrl + '/Cv/' + this.cand.id, data, {headers: this.headersWithAuth
    })
      .pipe(
        tap(_ => {}),
        catchError(this.handleErrors)
      );
  }
}
