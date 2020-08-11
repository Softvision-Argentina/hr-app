import { Routes } from '@angular/router';
import { SettingsComponent } from './settings.component';

export const SettingsRoutes: Routes = [
  {
    path: '',
    component: SettingsComponent,
    children: [
      {
        path: 'festivities',
        loadChildren: () => import('@old-architecture/settings/company-calendar/company-calendar.module').then(m => m.CompanyCalendarModule)
      },
      {
        path: 'hire-projected',
        loadChildren: () => import('@old-architecture/settings/hire-projected/hire-projected.module').then(m => m.HireProjectedModule)
      },
      {
        path: 'employee-casualties',
        loadChildren: () => import('@old-architecture/settings/employee-casualties/employee-casualties.module').then(m => m.EmployeeCasualtiesModule)
      },
      {
        path: 'skills-list',
        loadChildren: () => import('@old-architecture/settings/skills/skills.module').then(m => m.SkillsModule)
      },
      {
        path: 'skills-types',
        loadChildren: () => import('@old-architecture/settings/skill-type/skill-type.module').then(m => m.SkillTypeModule)
      },
      {
        path: 'profiles/:tab',
        loadChildren: () => import('@old-architecture/settings/profiles/profiles.module').then(m => m.ProfilesModule)
      },
      {
        path: 'locations/:tab',
        loadChildren: () => import('@old-architecture/settings/locations/locations.module').then(m => m.LocationsModule)
      },
      {
        path: 'roles',
        loadChildren: () => import('@old-architecture/settings/role/role.module').then(m => m.RoleModule)
      },
      {
        path: 'declining-reasons',
        loadChildren: () => import('@old-architecture/settings/decline-reasons/decline-reasons.module').then(m => m.DeclineReasonsModule)
      },
      {
        path: 'reasons',
        loadChildren: () => import('@old-architecture/settings/readdress-reason/readdress-reason.module').then(m => m.ReaddressReasonModule)
      },
      {
        path: 'reasons-categories',
        loadChildren: () => import('@old-architecture/settings/readdress-reason-type/readdress-reason-type.module').then(m => m.ReaddressReasonTypeModule)
      }
    ]
  }
];
