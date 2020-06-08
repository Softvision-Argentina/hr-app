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
import { SkillService } from './services/skill.service';
import { LoaderComponent } from './loader/loader.component';
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
import { TaskService } from './services/task.service';
import { UserService } from './services/user.service';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { HRGuard } from './guards/hr-guard.service';
import { HireProjectionService } from './services/hireProjection.service';
import { EmployeeCasualtyService } from './services/employee-casualty.service';
import { CommunityService } from './services/community.service';
import { CandidateProfileService } from './services/candidate-profile.service';

import { EmployeeService } from './services/employee.service';
import { DaysOffService } from './services/days-off.service';
import { RoomService } from './services/room.service';
import { ReservationService } from './services/reservation.service';
import { OfficeService } from './services/office.service';
import { RoleService } from './services/role.service';
import { CompanyCalendarService } from './services/company-calendar.service';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { HttpClientJsonpModule } from '@angular/common/http';
import { NZ_ICONS } from 'ng-zorro-antd';
import { IconDefinition } from '@ant-design/icons-angular';
import * as AllIcons from '@ant-design/icons-angular/icons';
import { NzCalendarComponent } from './nz-calendar/NzCalendar';
import { NzPopoverModule } from 'ng-zorro-antd';
import { Globals } from './app-globals/globals';
import { NumbersOnlyDirective } from './directives/numbersOnlyDirective';
import { SlickModule } from 'ngx-slick';
import { PostulantsService } from './services/postulants.service';
import { DeclineReasonService } from './services/decline-reason.service';
import { InterviewService } from './services/interview.service';
import { PreferencesComponent } from './preferences/preferences.component';
import { HasRoleDirective } from 'src/app/directives/appHasRole.directive';
import { PreOfferService } from './services/pre-offer.service';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { AppRoutingModule } from './app-routing.module';
import { ReferralsService } from './services/referrals.service';
import { HashLocationStrategy, LocationStrategy  } from '@angular/common';

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
    LoaderComponent,
    PreferencesComponent,
    HasRoleDirective,
    SideMenuComponent,
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
    PreOfferService,
    InterviewService,
    { provide: NZ_I18N, useValue: en_US },
    {provide : LocationStrategy , useClass: HashLocationStrategy},
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
