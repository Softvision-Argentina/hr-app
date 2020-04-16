import { CommunitiesModule } from './communities/communities.module';
import { PipesModule } from './pipes/pipes.module';
import { APP_INITIALIZER } from '@angular/core';
import { AppConfig } from './app-config/app.config';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { RegisterService } from './services/register.service';
import { CandidateService } from './services/candidate.service';
import { ProcessService } from './services/process.service';
import { StageService } from './services/stage.service';
import { ConfigService } from './services/config.service';
import { CandidatesComponent } from './candidates/candidates.component';
import { SkillService } from './services/skill.service';
import { LoaderComponent } from './loader/loader.component';
import { ProcessesComponent } from './processes/processes/processes.component';
import { ProcessTableComponent } from './processes/processes-table/processes-table.component';
import { ProcessDetailComponent } from './processes/process-detail/process-detail.component';
import { StageDetailComponent } from './stages/stage-detail/stage-detail.component';
import { StageEditComponent } from './stages/stage-edit/stage-edit.component';
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
import { SkillTypeService } from './services/skillType.service.';
import { BaseService } from './services/base.service';
import { FacadeService } from './services/facade.service';
import { ProcessStepsComponent } from './processes/process-steps/process-steps.component';
import { CandidateDetailsComponent } from './candidates/details/candidate-details.component';
import { UserDetailsComponent } from './users/details/user-details.component';
import { TasksComponent } from './tasks/tasks.component';
import { TaskService } from './services/task.service';
import { UserService } from './services/user.service';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { HRGuard } from './guards/hr-guard.service';
import { CandidateAddComponent } from './candidates/add/candidate-add.component';
import { HrStageComponent } from './stages/hr-stage/hr-stage.component';
import { TechnicalStageComponent } from './stages/technical-stage/technical-stage.component';
import { ClientStageComponent } from './stages/client-stage/client-stage.component';
import { OfferStageComponent } from './stages/offer-stage/offer-stage.component';
import { HireStageComponent } from './stages/hire-stage/hire-stage.component';
import { PeopleComponent } from './people/people.component';
import { HireProjectionService } from './services/hireProjection.service';
import { EmployeeCasualtyService } from './services/employee-casualty.service';
import { CommunityService } from './services/community.service';
import { CandidateProfileService } from './services/candidate-profile.service';
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
import { OfficeService } from './services/office.service';
import { RoleService } from './services/role.service';
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
import { Globals } from './app-globals/globals';
import { NumbersOnlyDirective } from './directives/numbersOnlyDirective';
import { SlickModule } from 'ngx-slick';
import { PostulantsComponent } from './postulants/postulants.component';
import { PostulantsService } from './services/postulants.service';
import { ReportTimetofill2Component } from './dashboard/report-timetofill2/report-timetofill2.component';
import { ReportTimetofill1Component } from './dashboard/report-timetofill1/report-timetofill1.component';
import { DeclineReasonService } from './services/decline-reason.service';
import { PreferencesComponent } from './preferences/preferences.component';
import { HasRoleDirective } from 'src/app/directives/appHasRole.directive';
import { ReferralsComponent } from './referrals/referrals/referrals.component';
import { ReferralsContactComponent } from './referrals/referrals-contact/referrals-contact.component';
import { ReferralsCardComponent } from './referrals/referrals-card/referrals-card.component';
import { ReportDeclineReasonsComponent } from './dashboard/report-decline-reasons/report-decline-reasons.component';
import { OfferService } from './services/offer.service';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { AppRoutingModule } from './app-routing.module';
import { ReportsComponent } from './reports/reports.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ReferralsService } from './services/referrals.service';


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
    LoaderComponent,
    ProcessesComponent,
    ProcessTableComponent,
    ProcessDetailComponent,
    StageDetailComponent,
    StageEditComponent,
    ReportsComponent,
    ProcessStepsComponent,
    UserDetailsComponent,
    TasksComponent,
    CandidateAddComponent,
    HrStageComponent,
    TechnicalStageComponent,
    ClientStageComponent,
    OfferStageComponent,
    HireStageComponent,
    PeopleComponent,
    EmployeesComponent,
    EmployeeDetailsComponent,
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
    PostulantsComponent,
    PreferencesComponent,
    ReportTimetofill1Component,
    HasRoleDirective,
    ReportDeclineReasonsComponent,
    SideMenuComponent,
    OfferHistory,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserModule,
    HttpModule,
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
    PipesModule,
    CommunitiesModule,
    Ng2LoadingSpinnerModule.forRoot({}),
    AppRoutingModule,
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
    ReferralsService,
    SkillService,
    ProcessService,
    StageService,
    ConfigService,
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
