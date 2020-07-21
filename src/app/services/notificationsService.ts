import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { Globals } from '../app-globals/globals';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService extends BaseService<Notification> {

constructor(router: Router, config: AppConfig, http: HttpClient, globals: Globals) {
  super(router, config, http);
  this.apiUrl += 'Notifications';
}

public getNotifications(): Observable<any>{

  return this.http.get(this.apiUrl, {
    headers: this.headersWithAuth
  })
    .pipe(
      tap(data => {}),
      catchError(this.handleErrors)
    );
}

public readNotifications(id: number): Observable<any> {

  const notiurl = `${this.apiUrl}/${id.toString()}`;
  
  let noticall = this.http.put(notiurl, {
    headers: this.headersWithAuth
  }).pipe(
    tap( _ => {}),
    catchError(this.handleErrors)
  );

  return noticall;
}

}
