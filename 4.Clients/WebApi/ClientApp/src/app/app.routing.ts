import { Routes } from '@angular/router';
import { ProcessesComponent } from './processes/processes/processes.component';
import { HRGuard } from './guards/hr-guard.service';
import { ProcessDetailComponent } from './processes/process-detail/process-detail.component';
import { ProcessStepsComponent } from './processes/process-steps/process-steps.component';
import { ReferralsComponent } from './referrals/referrals/referrals.component';
import { CommonGuard } from './guards/common-guard.service';
import { StageDetailComponent } from './stages/stage-detail/stage-detail.component';
import { StageEditComponent } from './stages/stage-edit/stage-edit.component';
import { CandidatesProfileComponent } from './candidates-profile/candidates-profile.component';
import { CommunitiesComponent } from './communities/communities.component';
import { PeopleComponent } from './people/people.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ReportsComponent } from './reports/reports.component';
import { ManagementGuard } from './guards/management-guard.service';
import { SettingsComponent } from './settings/settings.component';
import { CompanyCalendarComponent } from './company-calendar/company-calendar.component';
import { HireProjectedComponent } from './hire-projected/hire-projected.component';
import { EmployeeCasualtiesComponent } from './employee-casualties/employee-casualties.component';
import { SkillsComponent } from './skills/skills.component';
import { SkillTypeComponent } from './skill-type/skill-type.component';
import { ProfilesComponent } from './profiles/profiles.component';
import { LocationsComponent } from './locations/locations.component';
import { RoleComponent } from './role/role.component';
import { DeclineReasonComponent } from './decline-reasons/decline-reasons.component';
import { DaysOffComponent } from './days-off/days-off.component';
import { TasksComponent } from './tasks/tasks.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { EmployeesComponent } from './employees/employees.component';
import { PostulantsComponent } from './postulants/postulants.component';


export const routes: Routes = [
  { path: '', component: ProcessesComponent, pathMatch: 'full', canActivate: [HRGuard] },
  { path: 'processes', component: ProcessesComponent, canActivate: [HRGuard] },
  { path: 'process-details/:id', component: ProcessDetailComponent, canActivate: [HRGuard] },
  { path: 'process-steps/:id', component: ProcessStepsComponent, canActivate: [HRGuard] },
  { path: 'referrals', component: ReferralsComponent, canActivate: [CommonGuard] },
  { path: 'stage-details/:id', component: StageDetailComponent, canActivate: [CommonGuard] },
  { path: 'stage-edit/:id', component: StageEditComponent, canActivate: [CommonGuard] },
  { path: 'candidates-profile', component: CandidatesProfileComponent, canActivate: [HRGuard] },
  { path: 'communities', component: CommunitiesComponent, canActivate: [HRGuard] },
  { path: 'people', component: PeopleComponent, canActivate: [HRGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [HRGuard] },
  { path: 'reports', component: ReportsComponent, canActivate: [ManagementGuard] },
  { path: 'settings', component: SettingsComponent, canActivate: [ManagementGuard], children: [
    { path: 'festivities', component: CompanyCalendarComponent },
    { path: 'hire-projected', component: HireProjectedComponent },
    { path: 'employee-casualties', component: EmployeeCasualtiesComponent },
    { path: 'skills-list', component: SkillsComponent },
    { path: 'skills-types', component: SkillTypeComponent },
    { path: 'profiles/:tab', component: ProfilesComponent },
    { path: 'locations/:tab', component: LocationsComponent },
    { path: 'roles', component: RoleComponent },
    { path: 'declining-reasons', component: DeclineReasonComponent },
  ] },
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
  },
];
