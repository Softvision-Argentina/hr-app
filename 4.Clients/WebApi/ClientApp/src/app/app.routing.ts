import { Routes } from '@angular/router';

import { PostulantsComponent } from './postulants/postulants.component';
import { EmployeesComponent } from './employees/employees.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { TasksComponent } from './tasks/tasks.component';
import { HRGuard } from './guards/hr-guard.service';
import { DaysOffComponent } from './days-off/days-off.component';
import { ReportsComponent } from './reports/reports.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PeopleComponent } from './people/people.component';
import { ManagementGuard } from './guards/management-guard.service';
import { ProcessesComponent } from './processes/processes/processes.component';
import { CommonGuard } from './guards/common-guard.service';
import { ProcessDetailComponent } from './processes/process-detail/process-detail.component';
import { ProcessStepsComponent } from './processes/process-steps/process-steps.component';
import { ReferralsComponent } from './referrals/referrals/referrals.component';
import { StageDetailComponent } from './stages/stage-detail/stage-detail.component';
import { StageEditComponent } from './stages/stage-edit/stage-edit.component';

export const appRoutes: Routes = [
  { path: '', component: ProcessesComponent, pathMatch: 'full', canActivate: [HRGuard] },
  { path: 'processes', component: ProcessesComponent, canActivate: [HRGuard] },
  { path: 'process-details/:id', component: ProcessDetailComponent, canActivate: [HRGuard] },
  { path: 'process-steps/:id', component: ProcessStepsComponent, canActivate: [HRGuard] },
  { path: 'referrals', component: ReferralsComponent, canActivate: [CommonGuard] },
  { path: 'stage-details/:id', component: StageDetailComponent, canActivate: [CommonGuard] },
  { path: 'stage-edit/:id', component: StageEditComponent, canActivate: [CommonGuard] },
  {
    path: 'candidates-profile',
    loadChildren: './candidates-profile/candidates-profile.module#CandidatesProfileModule',
    canLoad: [HRGuard]
  },
  {
    path: 'communities',
    loadChildren: './communities/communities.module#CommunitiesModule',
    canLoad: [HRGuard]
  },
  { path: 'people', component: PeopleComponent, canActivate: [HRGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [HRGuard] },
  { path: 'reports', component: ReportsComponent, canActivate: [ManagementGuard] },
  {
    path: 'settings',
    loadChildren: './settings/settings.module#SettingsModule',
    canLoad: [ManagementGuard]
  },
  { path: 'daysOff', component: DaysOffComponent, canActivate: [ManagementGuard] },
  { path: 'tasks', component: TasksComponent, canActivate: [HRGuard] },
  { path: 'reservation', component: ReservationsComponent, canActivate: [HRGuard] },
  { path: 'employees', component: EmployeesComponent },
  { path: 'postulants', component: PostulantsComponent },
  {
    path: 'login',
    loadChildren: './login/login.module#LoginModule'
  },
  {
    path: 'unauthorized',
    loadChildren: './unauthorized/unauthorized.module#UnauthorizedModule'
  },
  {
    path: '404',
    loadChildren: './not-found/not-found.module#NotFoundModule'
  },
  {
    path: '**',
    loadChildren: './not-found/not-found.module#NotFoundModule'
  }
];
