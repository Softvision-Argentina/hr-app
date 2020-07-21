import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '../app-config/app.config';
import { Router } from '@angular/router';
import { PreOffer } from 'src/entities/pre-offer';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable()
export class PreOfferService extends BaseService<PreOffer> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'PreOffer';
  }

  public getByProcessId(processId: number): Observable<any>{
    let url = this.apiUrl + '/GetByProcess/' + processId.toString();
    return this.http.get<PreOffer[]>(url, 
      { headers: this.headersWithAuth, observe: "body" })
        .pipe(
          tap((res) => {
            this.data.next(res);
          }),
          catchError(this.handleErrors)
        );
  }
}
