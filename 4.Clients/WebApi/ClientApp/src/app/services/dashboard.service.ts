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
dashboardSource = new Subject<Dashboard[]>();
dashboards = this.dashboardSource.asObservable();

constructor(router: Router, config: AppConfig, http: HttpClient) { 
  super(router, config, http);
    this.apiUrl += 'Dashboard';
}

changePreference (dashboards: Dashboard[]) {
  this.dashboardSource.next(dashboards);
}

}
