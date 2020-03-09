import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../app-config/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { catchError, tap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  http: HttpClient;
  apiUrl: string;
  constructor(http: HttpClient, private config: AppConfig) {
    this.http = http;
    this.apiUrl = this.config.getConfig('apiUrl');
  }

  public authenticate(userName: string, password: string): Observable<any> {
    let user = {UserName : userName , Password : password};
    return this.http.post(this.apiUrl + 'Auth/login', user)
      .pipe(
        tap(data => {}),
        catchError(this.handleError('authenticate', []))
      );
  }

  public externalLogin(token: string): Observable<any> { 
    let body = {token : token};
    return this.http.post(this.apiUrl + 'Auth/loginExternal', body)
      .pipe(
        tap(data => {}),
        catchError(this.handleError('externalLogin', []))
      );
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}
