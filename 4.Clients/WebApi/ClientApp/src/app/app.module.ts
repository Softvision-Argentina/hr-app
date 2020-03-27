import { APP_INITIALIZER } from '@angular/core';
import { AppConfig } from './app-config/app.config';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ReportsComponent } from './reports/reports.component';

import { RegisterService } from './services/register.service';
import { CandidateService } from './services/candidate.service';
import { ProcessService } from './services/process.service';
import { ConsultantService } from './services/consultant.service';
import { StageService } from './services/stage.service';
import { ConfigService } from './services/config.service';

import { CandidatesComponent } from './candidates/candidates.component';
import { SkillsComponent } from './skills/skills.component';
import { SkillService } from './services/skill.service';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoaderComponent } from './loader/loader.component';
import { ProcessesComponent } from './processes/processes/processes.component';
import { ProcessDetailComponent } from './processes/process-detail/process-detail.component';
import { StageDetailComponent } from './stages/stage-detail/stage-detail.component';
import { StageEditComponent } from './stages/stage-edit/stage-edit.component';
import { ConsultantsComponent } from './consultants/consultants.component';
import { GoogleSigninComponent } from './login/google-signin.component';
import { LoginComponent } from './login/login.component';
import {
  MatFormFieldModule,
  MatInputModule,
  MatAutocompleteModule,
  MatButtonModule,
  MatProgressSpinnerModule,
  MatDatepickerModule,
  MatNativeDateModule
} from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { Ng2LoadingSpinnerModule } from 'ng2-loading-spinner';
import { CommonGuard } from './guards/common-guard.service';
import { ManagementGuard } from './guards/management-guard.service';
import { AdminGuard } from './guards/admin-guard.service';
import { JwtHelper } from 'angular2-jwt';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { ChartsModule } from 'ng2-charts';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { SkillTypeComponent } from './skill-type/skill-type.component';
import { SkillTypeService } from './services/skillType.service.';
import { BaseService } from './services/base.service';
import { FacadeService } from './services/facade.service';
import { UnauthorizedComponent } from './unauthorized/unauthorized/unauthorized.component';
import { ProcessStepsComponent } from './processes/process-steps/process-steps.component';
import { CandidateDetailsComponent } from './candidates/details/candidate-details.component';
import { ConsultantDetailsComponent } from './consultants/details/consultant-details.component';
import { TasksComponent } from './tasks/tasks.component';
import { FilterPipe } from './pipes/titleFilter.pipe';
import { SortPipe } from './pipes/taskSort.pipe';
import { TaskService } from './services/task.service';
import { UserService } from './services/user.service';
import { SettingsComponent } from './settings/settings.component';
import { TruncatePipe } from './pipes/truncate.pipe';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { HRGuard } from './guards/hr-guard.service';
import { CandidateAddComponent } from './candidates/add/candidate-add.component';
import { HrStageComponent } from './stages/hr-stage/hr-stage.component';
import { TechnicalStageComponent } from './stages/technical-stage/technical-stage.component';
import { ClientStageComponent } from './stages/client-stage/client-stage.component';
import { OfferStageComponent } from './stages/offer-stage/offer-stage.component';
import { HireStageComponent } from './stages/hire-stage/hire-stage.component';
import { CSoftComponent } from './login/csoft-signin.component';
import { PeopleComponent } from './people/people.component';
import { HireProjectedComponent } from './hire-projected/hire-projected.component';
import { HireProjectionService } from './services/hireProjection.service';
import { EmployeeCasualtyService } from './services/employee-casualty.service';
import { CandidatesProfileComponent } from './candidates-profile/candidates-profile.component';
import { CommunitiesComponent } from './communities/communities.component';
import { CommunityService } from './services/community.service';
import { CandidateProfileService } from './services/candidate-profile.service';
import { EmployeeCasualtiesComponent } from './employee-casualties/employee-casualties.component';
import { ReportProcessesComponent } from './dashboard/report-processes/report-processes.component';
import { ReportCompletedProcessesComponent } from './dashboard/report-completed-processes/report-completed-processes.component';
import { ReportProgressProcessesComponent } from './dashboard/report-progress-processes/report-progress-processes.component';
import { ReportSkillsComponent } from './dashboard/report-skills/report-skills.component';
import { ReportHireProjectionComponent } from './dashboard/report-hire-projection/report-hire-projection.component';
import { ReportHireCasualtiesComponent } from './dashboard/report-hire-casualties/report-hire-casualties.component';
import { ReportWeeklyCandidatesComponent } from './dashboard/report-weekly-candidates/report-weekly-candidates.component';
import { ProcessContactComponent } from './processes/process-contact/process-contact.component';
import { EmployeeService } from './services/employee.service';
import { EmployeesComponent } from './employees/employees.component';
import { EmployeeDetailsComponent } from './employees/details/employee-details.component';
import { DaysOffComponent } from './days-off/days-off.component';
import { DaysOffService } from './services/days-off.service';
import { ReservationsComponent } from './reservations/reservations.component';
import { RoomService } from './services/room.service';
import { ReservationService } from './services/reservation.service';
import { OfficeComponent } from './office/office.component';
import { OfficeService } from './services/office.service';
import { RoomComponent } from './room/room.component';
import { RoleService } from './services/role.service';
import { RoleComponent } from './role/role.component';
import { CompanyCalendarComponent } from './company-calendar/company-calendar.component';
import { CompanyCalendarService } from './services/company-calendar.service';
import { FileUploadModule } from 'ng2-file-upload';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { HttpClientJsonpModule } from '@angular/common/http';
import { NZ_ICONS } from 'ng-zorro-antd';
import { IconDefinition } from '@ant-design/icons-angular';
import * as AllIcons from '@ant-design/icons-angular/icons';
import { OfferHistory } from './stages/offer-history/offer-history.component';
import { NzCalendarComponent } from './nz-calendar/NzCalendar';
import { NzPopoverModule } from 'ng-zorro-antd';
import { NoticeCalendarComponent } from './notice-calendar/notice-calendar.component';
import { Globals } from './app-globals/globals';
import { NumbersOnlyDirective } from './directives/numbersOnlyDirective';
import { LocationsComponent } from './locations/locations.component';
import { ProfilesComponent } from './profiles/profiles.component';
import { SlickModule } from 'ngx-slick';
import { PostulantsComponent } from './postulants/postulants.component';
import { PostulantsService } from './services/postulants.service';
import { ReportTimetofill2Component } from './dashboard/report-timetofill2/report-timetofill2.component';
import { ReportTimetofill1Component } from './dashboard/report-timetofill1/report-timetofill1.component';
import { DeclineReasonComponent } from './decline-reasons/decline-reasons.component';
import { DeclineReasonService } from './services/decline-reason.service';
import { PreferencesComponent } from './preferences/preferences.component';
import { HasRoleDirective } from 'src/app/directives/appHasRole.directive';
import { ReferralsComponent } from './referrals/referrals/referrals.component';
import { ReferralsContactComponent } from './referrals/referrals-contact/referrals-contact.component';
import { ReferralsCardComponent } from './referrals/referrals-card/referrals-card.component';
import { ReportDeclineReasonsComponent } from './dashboard/report-decline-reasons/report-decline-reasons.component';
import { OfferService } from './services/offer.service';
import { SideMenuComponent } from './side-menu/side-menu.component';


registerLocaleData(en);

const antDesignIcons = AllIcons as {
  [key: string]: IconDefinition;
};
const icons: IconDefinition[] = Object.keys(antDesignIcons).map(key => antDesignIcons[key]);


@NgModule({
   declarations: [
      NumbersOnlyDirective,
      NzCalendarComponent,
      AppComponent,
      NavMenuComponent,
      DashboardComponent,
      CandidatesComponent,
      CandidateDetailsComponent,
      SkillsComponent,
      NotFoundComponent,
      LoaderComponent,
      ProcessesComponent,
      ProcessDetailComponent,
      StageDetailComponent,
      StageEditComponent,
      ReportsComponent,
      LoginComponent,
      GoogleSigninComponent,
      CSoftComponent,
      ConsultantsComponent,
      SkillTypeComponent,
      UnauthorizedComponent,
      ProcessStepsComponent,
      ConsultantDetailsComponent,
      TasksComponent,
      FilterPipe,
      SortPipe,
      TruncatePipe,
      SettingsComponent,
      CandidateAddComponent,
      HrStageComponent,
      TechnicalStageComponent,
      ClientStageComponent,
      OfferStageComponent,
      HireStageComponent,
      PeopleComponent,
      HireProjectedComponent,
      EmployeeCasualtiesComponent,
      EmployeesComponent,
      EmployeeDetailsComponent,
      CommunitiesComponent,
      CandidatesProfileComponent,
      ReportProcessesComponent,
      ReportCompletedProcessesComponent,
      ReportProgressProcessesComponent,
      ReportSkillsComponent,
      ReportHireProjectionComponent,
      ReportHireCasualtiesComponent,
      ReportWeeklyCandidatesComponent,
      ReportTimetofill2Component,
      ProcessContactComponent,
      DaysOffComponent,
      ProcessContactComponent,
      ReferralsComponent,
      ReferralsContactComponent,
      ReferralsCardComponent,
      ReservationsComponent,
      OfficeComponent,
      RoomComponent,
      RoleComponent,
      OfficeComponent,
      CompanyCalendarComponent,
      NoticeCalendarComponent,
      LocationsComponent,
      ProfilesComponent,
      PostulantsComponent,
      PreferencesComponent,
      ReportTimetofill1Component,
      DeclineReasonComponent,
      HasRoleDirective,
      ReportDeclineReasonsComponent,
      SideMenuComponent,
      OfferHistory
   ],
   imports: [
      BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserModule,
    FormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ReactiveFormsModule,
    NgZorroAntdModule,
    BrowserAnimationsModule,
    ScrollingModule,
    DragDropModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    FileUploadModule,
    ReactiveFormsModule,
    HttpModule,
    MatInputModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    BrowserAnimationsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgZorroAntdModule,
    NzPopoverModule,
    ChartsModule,
    DragDropModule,
    SlickModule.forRoot(),
    ToastrModule.forRoot(),
    Ng2LoadingSpinnerModule.forRoot({}),
    RouterModule.forRoot([
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
      { path: 'login', component: LoginComponent },
      { path: 'tasks', component: TasksComponent, canActivate: [HRGuard] },
      { path: 'reservation', component: ReservationsComponent, canActivate: [HRGuard] },
      { path: 'employees', component: EmployeesComponent },
      { path: 'postulants', component: PostulantsComponent },
      { path: 'unauthorized', component: UnauthorizedComponent },
      { path: '404', component: NotFoundComponent },
      { path: '**', component: NotFoundComponent }
    ])
  ],
  providers: [
    Globals,
    { provide: NZ_I18N, useValue: en_US }, { provide: NZ_ICONS, useValue: icons },
    FacadeService,
    AppConfig,
    { provide: APP_INITIALIZER, useFactory: (config: AppConfig) => () => config.load(), deps: [AppConfig], multi: true },
    BaseService,
    RegisterService,
    CandidateService,
    SkillService,
    ProcessService,
    StageService,
    ConfigService,
    ConsultantService,
    UserService,
    JwtHelper,
    CommonGuard,
    HRGuard,
    ManagementGuard,
    AdminGuard,
    SkillTypeService,
    TaskService,
    HireProjectionService,
    CandidateProfileService,
    CommunityService,
    EmployeeCasualtyService,
    EmployeeService,
    DaysOffService,
    RoomService,
    ReservationService,
    OfficeService,
    RoleService,
    CompanyCalendarService,
    PostulantsService,
    DeclineReasonService,
    OfferService,
    { provide: NZ_I18N, useValue: en_US }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
