import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '@shared/utils/app.config';
import { catchError } from 'rxjs/operators';
import { Observable ,  of } from 'rxjs';

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
        catchError(this.handleError('authenticate', {}))
      );
  }

  public externalLogin(token: string): Observable<any> {
    let body = {token : token};
    return this.http.post(this.apiUrl + 'Auth/loginExternal', body)
      .pipe(
        catchError(this.handleError('externalLogin', {}))
      );
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}
