import { Routes } from '@angular/router';
import { SettingsComponent } from './settings.component';

export const SettingsRoutes: Routes = [
  {
    path: '',
    component: SettingsComponent,
    children: [
      {
        path: 'festivities',
        loadChildren: () => import('@app/company-calendar/company-calendar.module').then(m => m.CompanyCalendarModule)
      },
      {
        path: 'hire-projected',
        loadChildren: () => import('@app/hire-projected/hire-projected.module').then(m => m.HireProjectedModule)
      },
      {
        path: 'employee-casualties',
        loadChildren: () => import('@app/employee-casualties/employee-casualties.module').then(m => m.EmployeeCasualtiesModule)
      },
      {
        path: 'skills-list',
        loadChildren: () => import('@app/skills/skills.module').then(m => m.SkillsModule)
      },
      {
        path: 'skills-types',
        loadChildren: () => import('@app/skill-type/skill-type.module').then(m => m.SkillTypeModule)
      },
      {
        path: 'profiles/:tab',
        loadChildren: () => import('@app/profiles/profiles.module').then(m => m.ProfilesModule)
      },
      {
        path: 'locations/:tab',
        loadChildren: () => import('@app/locations/locations.module').then(m => m.LocationsModule)
      },
      {
        path: 'roles',
        loadChildren: () => import('@app/role/role.module').then(m => m.RoleModule)
      },
      {
        path: 'declining-reasons',
            loadChildren: () => import('@app/decline-reasons/decline-reasons.module').then(m => m.DeclineReasonsModule)
        },
        {
            path: 'reasons',
            loadChildren: () => import('@app/readdress-reason/readdress-reason.module').then(m => m.ReaddressReasonModule)
        },
        {
            path: 'reasons-categories',
            loadChildren: () => import('@app/readdress-reason-type/readdress-reason-type.module').then(m => m.ReaddressReasonTypeModule)
        }
    ]
  }
];
