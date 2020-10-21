import { DragDropModule } from '@angular/cdk/drag-drop';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { HasRoleDirective } from '@shared/directives/app-has-role.directive';
import { AdminGuard } from '@shared/guards/admin.guard';
import { CommonGuard } from '@shared/guards/common.guard';
import { HRGuard } from '@shared/guards/hr.guard';
import { ManagementGuard } from '@shared/guards/management.guard';
import { IconsProviderModule } from '@shared/icons-provider.module';
import { PipesModule } from '@shared/pipes/pipes.module';
import { BaseService } from '@shared/services/base.service';
import { CandidateProfileService } from '@shared/services/candidate-profile.service';
import { CandidateService } from '@shared/services/candidate.service';
import { CommunityService } from '@shared/services/community.service';
import { CompanyCalendarService } from '@shared/services/company-calendar.service';
import { DaysOffService } from '@shared/services/days-off.service';
import { DeclineReasonService } from '@shared/services/decline-reason.service';
import { EmployeeCasualtyService } from '@shared/services/employee-casualty.service';
import { EmployeeService } from '@shared/services/employee.service';
import { FacadeService } from '@shared/services/facade.service';
import { HireProjectionService } from '@shared/services/hire-projection.service';
import { InterviewService } from '@shared/services/interview.service';
import { OfficeService } from '@shared/services/office.service';
import { OpenPositionService } from '@shared/services/open-position.service';
import { PostulantsService } from '@shared/services/postulants.service';
import { PreOfferService } from '@shared/services/pre-offer.service';
import { ProcessService } from '@shared/services/process.service';
import { ReaddressReasonTypeService } from '@shared/services/readdress-reason-type.service';
import { ReaddressReasonService } from '@shared/services/readdress-reason.service';
import { ReferralsService } from '@shared/services/referrals.service';
import { RegisterService } from '@shared/services/register.service';
import { ReservationService } from '@shared/services/reservation.service';
import { RoleService } from '@shared/services/role.service';
import { RoomService } from '@shared/services/room.service';
import { SkillTypeService } from '@shared/services/skill-type.service';
import { SkillService } from '@shared/services/skill.service';
import { StageService } from '@shared/services/stage.service';
import { TaskService } from '@shared/services/task.service';
import { UserService } from '@shared/services/user.service';
import { reducers } from '@shared/store';
import { CommunitiesEffects } from '@shared/store/communities/communities.effects';
import { AppConfig } from '@shared/utils/app.config';
import { Globals } from '@shared/utils/globals';
import { en_US, NzAvatarModule, NzCalendarModule, NzCardModule, NzListModule, NzModalModule, NzSwitchModule, NZ_I18N } from 'ng-zorro-antd';
import { ChartsModule } from 'ng2-charts';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './old-architecture/nav-menu/nav-menu.component';
import { PreferencesComponent } from './old-architecture/preferences/preferences.component';
import { ReferralsModule } from '@app/containers/referrals/referrals/referrals.module';
import { SideMenuComponent } from './old-architecture/side-menu/side-menu.component';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { CandidateInfoService } from '@shared/services/candidate-info.service';


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
    ReferralsModule,
    StoreModule.forRoot(reducers),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
    }),
    EffectsModule.forRoot([CommunitiesEffects])
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
    CandidateInfoService,
    { provide: NZ_I18N, useValue: en_US },
    { provide: LocationStrategy, useClass: HashLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
