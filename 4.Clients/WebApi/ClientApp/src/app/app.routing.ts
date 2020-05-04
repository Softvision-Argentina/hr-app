import { Routes } from '@angular/router';
import { HRGuard } from './guards/hr-guard.service';
import { ManagementGuard } from './guards/management-guard.service';
import { CommonGuard } from './guards/common-guard.service';

export const appRoutes: Routes = [
  {
    path: 'daysOff',
    loadChildren: './days-off/days-off.module#DaysOffModule',
    canLoad: [ManagementGuard]
  },
  {
    path: 'processes',
    loadChildren: './processes/processes/processes.module#ProcessesModule',
    canLoad: [HRGuard]
  },
  {
    path: 'process-details/:id',
    loadChildren: './processes/process-detail/processes-detail.module#ProcessDetailModule',
    canLoad: [HRGuard]
  },
  {
    path: 'process-steps/:id',
    loadChildren: './processes/process-steps/process-steps.module#ProcessStepsModule',
    canLoad: [HRGuard]
  },
  {
    path: 'referrals',
    loadChildren: './referrals/referrals/referrals.module#ReferralsModule',
    canActivate: [CommonGuard]
  },
  {
    path: 'stage-details/:id',
    loadChildren: './stages/stage-detail/stage-detail.module#StageDetailModule',
    canActivate: [CommonGuard]
  },
  {
    path: 'stage-edit/:id',
    loadChildren: './stages/stage-edit/stage-edit.module#StageEditModule',
    canActivate: [CommonGuard]
  },
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
  {
    path: 'people',
    loadChildren: './people/people.module#PeopleModule',
    canLoad: [HRGuard]
  },
  {
    path: 'dashboard',
    loadChildren: './dashboard/dashboard.module#DashboardModule',
    canLoad: [HRGuard]
  },
  {
    path: 'reports',
    loadChildren: './reports/reports.module#ReportsModule',
    canLoad: [ManagementGuard]
  },
  {
    path: 'settings',
    loadChildren: './settings/settings.module#SettingsModule',
    canLoad: [ManagementGuard]
  },

  {
    path: 'tasks',
    loadChildren: './tasks/tasks.module#TasksModule',
    canLoad: [HRGuard]
  },
  {
    path: 'reservation',
    loadChildren: './reservations/reservations.module#ReservationsModule',
    canLoad: [HRGuard]
  },
  {
    path: 'employees',
    loadChildren: './employees/employees.module#EmployeesModule'
  },
  {
    path: 'postulants',
    loadChildren: './postulants/postulants.module#PostulantsModule'
  },
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
  { path: '', redirectTo: '/processes', pathMatch: 'full', canActivate: [HRGuard] },
  {
    path: '**',
    loadChildren: './not-found/not-found.module#NotFoundModule'
  }
];
