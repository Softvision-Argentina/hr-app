import { Injectable, Injector } from '@angular/core';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { AppConfig } from '@shared/utils/app.config';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Postulant } from '@shared/models/postulant.model';
import { tap, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable()
export class PostulantsService extends BaseService<Postulant> {
  private baseUrl = 'http://localhost:61059/';
  public headers: HttpHeaders;


  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Postulants';
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  public idExists(id: number): Observable<any> {
    return this.http.get(this.baseUrl + 'api/Postulants/' + id, {
      headers: this.headers, observe: 'response'
    })
      .pipe(
        tap(_ => {}),
        catchError(this.handleErrors)
      );
  }
}
