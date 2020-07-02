import { Routes } from '@angular/router';
import { SettingsComponent } from './settings.component';

export const SettingsRoutes: Routes = [
  {
    path: '',
    component: SettingsComponent,
    children: [
      {
        path: 'festivities',
        loadChildren: '../company-calendar/company-calendar.module#CompanyCalendarModule'
      },
      {
        path: 'hire-projected',
        loadChildren: '../hire-projected/hire-projected.module#HireProjectedModule'
      },
      {
        path: 'employee-casualties',
        loadChildren: '../employee-casualties/employee-casualties.module#EmployeeCasualtiesModule'
      },
      {
        path: 'skills-list',
        loadChildren: '../skills/skills.module#SkillsModule'
      },
      {
        path: 'skills-types',
        loadChildren: '../skill-type/skill-type.module#SkillTypeModule'
      },
      {
        path: 'profiles/:tab',
        loadChildren: '../profiles/profiles.module#ProfilesModule'
      },
      {
        path: 'locations/:tab',
        loadChildren: '../locations/locations.module#LocationsModule'
      },
      {
        path: 'roles',
        loadChildren: '../role/role.module#RoleModule'
      },
      {
        path: 'declining-reasons',
        loadChildren: '../decline-reasons/decline-reasons.module#DeclineReasonsModule'
      },
      {
        path: 'reasons',
        loadChildren: '../readdress-reason/readdress-reason.module#ReaddressReasonModule'
      },
      {
        path: 'reasons-categories',
        loadChildren: '../readdress-reason-type/readdress-reason-type.module#ReaddressReasonTypeModule'
      }
    ]
  }
];
