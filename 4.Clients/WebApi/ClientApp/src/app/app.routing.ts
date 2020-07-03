import { Routes } from '@angular/router';
import { HRGuard } from './guards/hr-guard.service';
import { ManagementGuard } from './guards/management-guard.service';
import { CommonGuard } from './guards/common-guard.service';

export const appRoutes: Routes = [
  {
    path: 'daysOff',
    loadChildren: () => import('./days-off/days-off.module').then(m => m.DaysOffModule),
    canLoad: [ManagementGuard]
  },
  {
    path: 'processes',
    loadChildren: () => import('./processes/processes/processes.module').then(m => m.ProcessesModule),
    canLoad: [HRGuard]
  },
  {
    path: 'process-details/:id',
    loadChildren: () => import('./processes/process-detail/processes-detail.module').then(m => m.ProcessDetailModule),
    canLoad: [HRGuard]
  },
  {
    path: 'process-steps/:id',
    loadChildren: () => import('./processes/process-steps/process-steps.module').then(m => m.ProcessStepsModule),
    canLoad: [HRGuard]
  },
  {
    path: 'welcome',
    loadChildren: './welcome-page/welcome-page.module#WelcomePageModule',
    canActivate: [CommonGuard]
  },
  {
    path: 'referrals',
    loadChildren: () => import('./referrals/referrals/referrals.module').then(m => m.ReferralsModule),
    canActivate: [CommonGuard]
  },
  {
    path: 'stage-details/:id',
    loadChildren: () => import('./stages/stage-detail/stage-detail.module').then(m => m.StageDetailModule),
    canActivate: [CommonGuard]
  },
  {
    path: 'stage-edit/:id',
    loadChildren: () => import('./stages/stage-edit/stage-edit.module').then(m => m.StageEditModule),
    canActivate: [CommonGuard]
  },
  {
    path: 'candidates-profile',
    loadChildren: () => import('./candidates-profile/candidates-profile.module').then(m => m.CandidatesProfileModule),
    canLoad: [HRGuard]
  },
  {
    path: 'communities',
    loadChildren: () => import('./communities/communities.module').then(m => m.CommunitiesModule),
    canLoad: [HRGuard]
  },
  {
    path: 'people',
    loadChildren: () => import('./people/people.module').then(m => m.PeopleModule),
    canLoad: [HRGuard]
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule),
    canLoad: [HRGuard]
  },
  {
    path: 'reports',
    loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule),
    canLoad: [ManagementGuard]
  },
  {
    path: 'settings',
    loadChildren: () => import('./settings/settings.module').then(m => m.SettingsModule),
    canLoad: [ManagementGuard]
  },

  {
    path: 'tasks',
    loadChildren: () => import('./tasks/tasks.module').then(m => m.TasksModule),
    canLoad: [HRGuard]
  },
  {
    path: 'reservation',
    loadChildren: () => import('./reservations/reservations.module').then(m => m.ReservationsModule),
    canLoad: [HRGuard]
  },
  {
    path: 'employees',
    loadChildren: () => import('./employees/employees.module').then(m => m.EmployeesModule)
  },
  {
    path: 'postulants',
    loadChildren: () => import('./postulants/postulants.module').then(m => m.PostulantsModule)
  },
  {
    path: 'login',
    loadChildren: () => import('./login/login.module').then(m => m.LoginModule)
  },
  {
    path: 'unauthorized',
    loadChildren: () => import('./unauthorized/unauthorized.module').then(m => m.UnauthorizedModule)
  },
  {
    path: '404',
    loadChildren: () => import('./not-found/not-found.module').then(m => m.NotFoundModule)
  },
  {
    path: '',
    redirectTo: '/processes',
    pathMatch: 'full',
    canActivate: [HRGuard]
  },
  {
    path: '**',
    loadChildren: () => import('./not-found/not-found.module').then(m => m.NotFoundModule)
  }
];
