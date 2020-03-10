import { Injectable } from '@angular/core';
import { Dashboard } from 'src/entities/dashboard';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { AppConfig } from '../app-config/app.config';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardService extends BaseService<Dashboard> {
statusSource = new Subject<boolean[]>();
status = this.statusSource.asObservable();

constructor(router: Router, config: AppConfig, http: HttpClient) { 
  super(router, config, http);
    this.apiUrl += 'Dashboard';
}

changePreference(stat:boolean[]){
  this.statusSource.next(stat);
}

}
