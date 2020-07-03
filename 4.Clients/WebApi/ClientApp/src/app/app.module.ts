import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { PreferencesComponent } from './preferences/preferences.component';
import { NzListModule, NzCardModule, NzAvatarModule, NzSwitchModule, NZ_I18N, en_US, NzCalendarModule, NzModalModule } from 'ng-zorro-antd';
import { Globals } from './app-globals/globals';
import { FacadeService } from './services/facade.service';
import { AppConfig } from './app-config/app.config';
import { BaseService } from './services/base.service';
import { RegisterService } from './services/register.service';
import { CandidateService } from './services/candidate.service';
import { ReferralsService } from './services/referrals.service';
import { SkillService } from './services/skill.service';
import { ProcessService } from './services/process.service';
import { StageService } from './services/stage.service';
import { UserService } from './services/user.service';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { CommonGuard } from './guards/common-guard.service';
import { HRGuard } from './guards/hr-guard.service';
import { ManagementGuard } from './guards/management-guard.service';
import { AdminGuard } from './guards/admin-guard.service';
import { SkillTypeService } from './services/skillType.service.';
import { TaskService } from './services/task.service';
import { HireProjectionService } from './services/hireProjection.service';
import { CandidateProfileService } from './services/candidate-profile.service';
import { CommunityService } from './services/community.service';
import { EmployeeCasualtyService } from './services/employee-casualty.service';
import { EmployeeService } from './services/employee.service';
import { DaysOffService } from './services/days-off.service';
import { RoomService } from './services/room.service';
import { ReservationService } from './services/reservation.service';
import { OfficeService } from './services/office.service';
import { RoleService } from './services/role.service';
import { CompanyCalendarService } from './services/company-calendar.service';
import { PostulantsService } from './services/postulants.service';
import { DeclineReasonService } from './services/decline-reason.service';
import { ReaddressReasonService } from './services/readdress-reason.service';
import { ReaddressReasonTypeService } from './services/readdress-reason-type.service';
import { PreOfferService } from './services/pre-offer.service';
import { InterviewService } from './services/interview.service';
import { LocationStrategy, HashLocationStrategy, CommonModule } from '@angular/common';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ChartsModule } from 'ng2-charts';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ToastrModule } from 'ngx-toastr';
import { PipesModule } from './pipes/pipes.module';
import { IconsProviderModule } from './icons-provider.module';
import { HasRoleDirective } from './directives/appHasRole.directive';
import { OpenPositionService } from './services/open-position.service';
import { ReferralsModule } from './referrals/referrals/referrals.module';

@NgModule({
  declarations: [
    AppComponent,
    HasRoleDirective,
    SideMenuComponent,
    NavMenuComponent,
    PreferencesComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'recru-webapp' }),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem("currentUser");
        }
      }
    }),
    CommonModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ScrollingModule,
    ReactiveFormsModule,
    MatInputModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    BrowserAnimationsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ChartsModule,
    DragDropModule,
    ToastrModule.forRoot(),
    PipesModule,
    IconsProviderModule,
    NzAvatarModule,
    NzCardModule,
    NzListModule,
    NzSwitchModule,
    NzCalendarModule,
    NzModalModule,
    ReferralsModule
  ],
  providers: [
    Globals,
    FacadeService,
    AppConfig,
    BaseService,
    RegisterService,
    CandidateService,
    ReferralsService,
    SkillService,
    ProcessService,
    StageService,
    UserService,
    JwtHelperService,
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
    ReaddressReasonService,
    ReaddressReasonTypeService,
    InterviewService,
    OpenPositionService,
    { provide: NZ_I18N, useValue: en_US },
    { provide: LocationStrategy, useClass: HashLocationStrategy },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
