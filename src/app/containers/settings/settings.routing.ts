import { Routes } from '@angular/router';
import { SettingsComponent } from './settings.component';
import { HRGuard } from '@shared/guards/hr.guard';

export const SettingsRoutes: Routes = [
  {
    path: '',
    component: SettingsComponent,
    children: [
      {
        path: 'festivities',
        loadChildren: () => import('@app/containers/settings/company-calendar/company-calendar.module').then(m => m.CompanyCalendarModule)
      },
      {
        path: 'skills-list',
        loadChildren: () => import('@app/containers/settings/skills/skills.module').then(m => m.SkillsModule)
      },
      {
        path: 'skills-types',
        loadChildren: () => import('@app/containers/settings/skill-type/skill-type.module').then(m => m.SkillTypeModule)
      },
      {
        path: 'profiles/communities',
        loadChildren: () => import('@app/containers/settings/communities/communities.module').then(m => m.CommunitiesModule),
        canLoad: [HRGuard]
      },
      {
        path: 'profiles/candidates-profile',
        loadChildren: () => import('@app/containers/settings/candidates-profile/candidates-profile.module').then(m => m.CandidatesProfileModule),
        canLoad: [HRGuard]
      },
      {
        path: 'rooms',
        loadChildren: () => import('@app/containers/settings/room/room.module').then(m => m.RoomModule)
      },
      {
        path: 'offices',
        loadChildren: () => import('@app/containers/settings/office/office.module').then(m => m.OfficeModule)
      },
      {
        path: 'roles',
        loadChildren: () => import('@app/containers/settings/role/role.module').then(m => m.RoleModule)
      },
      {
        path: 'declining-reasons',
        loadChildren: () => import('@app/containers/settings/decline-reasons/decline-reasons.module').then(m => m.DeclineReasonsModule)
      },
      {
        path: 'reasons',
        loadChildren: () => import('@app/containers/settings/readdress-reason/readdress-reason.module').then(m => m.ReaddressReasonModule)
      },
      {
        path: 'reasons-categories',
        loadChildren: () => import('@app/containers/settings/readdress-reason-type/readdress-reason-type.module').then(m => m.ReaddressReasonTypeModule)
      }
    ]
  }
];
