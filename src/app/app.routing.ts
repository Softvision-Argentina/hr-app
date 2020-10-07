import { Routes } from '@angular/router';
import { HRGuard } from '@shared/guards/hr.guard';
import { ManagementGuard } from '@shared/guards/management.guard';
import { CommonGuard } from '@shared/guards/common.guard';

export const appRoutes: Routes = [
  {
    path: 'daysOff',
    loadChildren: () => import('@old-architecture/days-off/days-off.module').then(m => m.DaysOffModule),
    canLoad: [ManagementGuard]
  },
  {
    path: 'processes',
    loadChildren: () => import('@old-architecture/processes/processes/processes.module').then(m => m.ProcessesModule),
    canLoad: [HRGuard]
  },
  {
    path: 'process-details/:id',
    loadChildren: () => import('@old-architecture/processes/process-detail/processes-detail.module').then(m => m.ProcessDetailModule),
    canLoad: [HRGuard]
  },
  {
    path: 'process-steps/:id',
    loadChildren: () => import('@old-architecture/processes/process-steps/process-steps.module').then(m => m.ProcessStepsModule),
    canLoad: [HRGuard]
  },
  {
    path: 'welcome',
    loadChildren: () => import('@old-architecture/welcome-page/welcome-page.module').then(m => m.WelcomePageModule),
    canActivate: [CommonGuard]
  },
  {
    path: 'referrals/:openpositions',
    loadChildren: () => import('./containers/referrals/referrals/referrals.module').then(m => m.ReferralsModule),
    canActivate: [CommonGuard]
  },
  {
    path: 'referrals',
    loadChildren: () => import('./containers/referrals/referrals/referrals.module').then(m => m.ReferralsModule),
    canActivate: [CommonGuard]
  },
  {
    path: 'stage-details/:id',
    loadChildren: () => import('@old-architecture/stages/stage-detail/stage-detail.module').then(m => m.StageDetailModule),
    canActivate: [CommonGuard]
  },
  {
    path: 'stage-edit/:id',
    loadChildren: () => import('@old-architecture/stages/stage-edit/stage-edit.module').then(m => m.StageEditModule),
    canActivate: [CommonGuard]
  },
  {
    path: 'people',
    loadChildren: () => import('@old-architecture/people/people.module').then(m => m.PeopleModule),
    canLoad: [HRGuard]
  },
  {
    path: 'dashboard',
    loadChildren: () => import('@old-architecture/dashboard/dashboard.module').then(m => m.DashboardModule),
    canLoad: [HRGuard]
  },
  {
    path: 'reports',
    loadChildren: () => import('@old-architecture/reports/reports.module').then(m => m.ReportsModule),
    canLoad: [ManagementGuard]
  },
  {
    path: 'settings',
    loadChildren: () => import('@app/containers/settings/settings.module').then(m => m.SettingsModule),
    canLoad: [ManagementGuard]
  },

  {
    path: 'tasks',
    loadChildren: () => import('@app/containers/tasks/tasks.module').then(m => m.TasksModule),
    canLoad: [HRGuard]
  },
  {
    path: 'reservation',
    loadChildren: () => import('@old-architecture/reservations/reservations.module').then(m => m.ReservationsModule),
    canLoad: [HRGuard]
  },
  {
    path: 'employees',
    loadChildren: () => import('@old-architecture/employees/employees.module').then(m => m.EmployeesModule)
  },
  {
    path: 'postulants',
    loadChildren: () => import('@old-architecture/postulants/postulants.module').then(m => m.PostulantsModule)
  },
  {
    path: 'login',
    loadChildren: () => import('@old-architecture/login/login.module').then(m => m.LoginModule)
  },
  {
    path: 'terms-and-conditions',
    loadChildren: () => import('@old-architecture/terms-and-conditions/terms-and-conditions.module').then(m => m.TermsAndConditionsModule)
  },
  {
    path: 'unauthorized',
    loadChildren: () => import('@old-architecture/unauthorized/unauthorized.module').then(m => m.UnauthorizedModule)
  },
  {
    path: '404',
    loadChildren: () => import('@old-architecture/not-found/not-found.module').then(m => m.NotFoundModule)
  },
  {
    path: '',
    redirectTo: '/processes',
    pathMatch: 'full',
    canActivate: [HRGuard]
  },
  {
    path: '**',
    loadChildren: () => import('@old-architecture/not-found/not-found.module').then(m => m.NotFoundModule)
  }
];
