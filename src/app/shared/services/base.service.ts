import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { throwError } from 'rxjs/internal/observable/throwError';
import { Observable, Subject, BehaviorSubject } from 'rxjs';
import { AppConfig } from '@shared/utils/app.config';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ErrorResponse } from '@shared/models/error-response.model';

@Injectable()
export class BaseService<T> {
  public headers: HttpHeaders;
  public headersWithAuth: HttpHeaders;
  public token: string;
  public apiUrl: string;
  public data: BehaviorSubject<T[]> = new BehaviorSubject<T[]>(null);
  public currentId: Subject<number> = new Subject<number>();
  public defaultServerErrorMessage: string = "The service is not available now. Try again later.";

  constructor(private router: Router, private config: AppConfig, public http: HttpClient) {
    let user = JSON.parse(localStorage.getItem('currentUser'));
    this.token = user !== null ? user.token : null;
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    this.headersWithAuth = new HttpHeaders({
      Authorization: 'Bearer ' + this.token,
      'Content-Type': 'application/json',
    });
    this.http = http;
    this.apiUrl = this.config.getConfig('apiUrl');
  }

  public getData(): Observable<T[]> {
    return this.data.pipe(
      tap((res) => {
        if (!res) {
          // En caso de que T[] sea null, pido los datos a la base.
          this.get().subscribe();
        }
      })
    );
  }

  public get(urlAdd?: string): Observable<T[]> {
    const url = urlAdd === undefined ? this.apiUrl : `${this.apiUrl}/${urlAdd}`;
    return this.http.get<T[]>(url,
      { headers: this.headersWithAuth, observe: "body" })
      .pipe(
        tap((res) => {
          this.data.next(res);
        }),
        catchError(this.handleErrors)
      );
  }

  public getByID(id: number): Observable<T> {
    const url = `${this.apiUrl}/${id}`;
    return this.http
      .get<T>(url, {
        headers: this.headersWithAuth,
      })
      .pipe(catchError(this.handleErrors));
  }

  public add(entity: T): Observable<T> {
    return this.http
      .post<T>(this.apiUrl, entity, {
        headers: this.headersWithAuth,
      })
      .pipe(
        tap(res => {
          this.get().subscribe();
        }),
        catchError(this.handleErrors));
  }

  public update(id, entity: T): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http
      .put(url, entity, {
        headers: this.headersWithAuth,
      })
      .pipe(
        tap(res => this.get().subscribe()),
        catchError(this.handleErrors)
      );
  }

  public delete(id): Observable<T> {
    const url = `${this.apiUrl}/${id}`;
    return this.http
      .delete<T>(url, {
        headers: this.headersWithAuth,
      })
      .pipe(
        tap(res => this.get().subscribe()),
        catchError(this.handleErrors));
  }

  public handleErrors = (httpErrorResponse) => {

    let errorObject: ErrorResponse = httpErrorResponse.error;

    if (httpErrorResponse.status == 500){
      return throwError([this.defaultServerErrorMessage]);
    }

    else if (httpErrorResponse.status == 400){
      if (errorObject.errorCode == 200){
        //back-end validation error from fluent validation
        let errorArray = [];
        errorObject.validationErrors.forEach(error => {
          errorArray.push(error.description);
        })
        return throwError(errorArray);
      }
      else{
        //back-end business validations such as uniqueness of names and others
        return throwError([errorObject.exceptionMessage]);
      }
    }

    else {
      return throwError([this.defaultServerErrorMessage]);
    }

  }
}
