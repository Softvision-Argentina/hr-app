import { AppConfig } from '../app-config/app.config';
import { Injectable, Injector } from '@angular/core';
import { SkillTypeService } from './skillType.service.';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { SkillService } from './skill.service';
import { CandidateService } from './candidate.service';
import { ConsultantService } from './consultant.service';
import { ProcessService } from './process.service';
import { ToastrService } from 'ngx-toastr';
import { StageService } from './stage.service';
import { TaskService } from './task.service';
import { HireProjectionService } from './hireProjection.service';
import { EmployeeCasualtyService } from './employee-casualty.service';
import { UserService } from './user.service';
import { CommunityService } from './community.service';
import { CandidateProfileService } from './candidate-profile.service';
import { AuthService } from './auth.service';
import { EmployeeService } from './employee.service';
import { DaysOffService } from './days-off.service';
import { ReservationService } from './reservation.service';
import { RoomService } from './room.service';
import { OfficeService } from './office.service';
import { RoleService } from './role.service';
import { CompanyCalendarService } from './company-calendar.service';
import { PostulantsService } from './postulants.service';
import { DeclineReasonService } from './decline-reason.service';
import { DashboardService } from './dashboard.service';
import { OfferService } from './offer.service';
import { NotificationService } from './notificationsService';
import { SearchbarService } from './searchbar.service';

@Injectable()
export class FacadeService {

  private _appConfig: AppConfig;
  public get appConfig(): AppConfig {
    if (!this._appConfig) {
      this._appConfig = this.injector.get(AppConfig);
    }
    return this._appConfig;
  }

  private _communityService: CommunityService;
  public get communityService(): CommunityService {
    if (!this._communityService) {
      this._communityService = this.injector.get(CommunityService);
    }
    return this._communityService;
  }

  private _daysOffService: DaysOffService;
  public get daysOffService(): DaysOffService {
    if (!this._daysOffService) {
      this._daysOffService = this.injector.get(DaysOffService);
    }
    return this._daysOffService;
  }

  private _companyCalendarService: CompanyCalendarService;
  public get companyCalendarService(): CompanyCalendarService {
    if (!this._companyCalendarService) {
      this._companyCalendarService = this.injector.get(CompanyCalendarService);
    }
    return this._companyCalendarService;
  }


  private _candidateProfileService: CandidateProfileService;
  public get candidateProfileService(): CandidateProfileService {
    if (!this._candidateProfileService) {
      this._candidateProfileService = this.injector.get(CandidateProfileService);
    }
    return this._candidateProfileService;
  }

  private _skillTypeService: SkillTypeService;
  public get skillTypeService(): SkillTypeService {
    if (!this._skillTypeService) {
      this._skillTypeService = this.injector.get(SkillTypeService);
    }
    return this._skillTypeService;
  }

  private _skillService: SkillService;
  public get skillService(): SkillService {
    if (!this._skillService) {
      this._skillService = this.injector.get(SkillService);
    }
    return this._skillService;
  }

  private _offerService: OfferService;
  public get offerService(): OfferService {
    if (!this._offerService) {
      this._offerService = this.injector.get(OfferService);
    }
    return this._offerService;
  }

  private _candidateService: CandidateService;
  public get candidateService(): CandidateService {
    if (!this._candidateService) {
      this._candidateService = this.injector.get(CandidateService);
    }
    return this._candidateService;
  }

  private _notificationsService: NotificationService;
  public get NotificationSevice(): NotificationService {
    if (!this._notificationsService) {
      this._notificationsService = this.injector.get(NotificationService);
    }
    return this._notificationsService;
  }

  private _postulantService: PostulantsService;
  public get postulantService(): PostulantsService {
    if (!this._postulantService) {
      this._postulantService = this.injector.get(PostulantsService);
    }
    return this._postulantService;
  }

  private _consultantService: ConsultantService;
  public get consultantService(): ConsultantService {
    if (!this._consultantService) {
      this._consultantService = this.injector.get(ConsultantService);
    }
    return this._consultantService;
  }

  private _processService: ProcessService;
  public get processService(): ProcessService {
    if (!this._processService) {
      this._processService = this.injector.get(ProcessService);
    }
    return this._processService;
  }

  private _taskService: TaskService;
  public get taskService(): TaskService {
    if (!this._taskService) {
      this._taskService = this.injector.get(TaskService);
    }
    return this._taskService;
  }

  private _hireProjectionService: HireProjectionService;
  public get hireProjectionService(): HireProjectionService {
    if (!this._hireProjectionService) {
      this._hireProjectionService = this.injector.get(HireProjectionService);
    }
    return this._hireProjectionService;
  }

  private _employeeCasualtyService: EmployeeCasualtyService;
  public get employeeCasulatyService(): EmployeeCasualtyService {
    if (!this._employeeCasualtyService) {
      this._employeeCasualtyService = this.injector.get(EmployeeCasualtyService);
    }
    return this._employeeCasualtyService;
  }
  private _employeeService: EmployeeService;
  public get employeeService(): EmployeeService {
    if (!this._employeeService) {
      this._employeeService = this.injector.get(EmployeeService);
    }
    return this._employeeService;
  }

  private _userService: UserService;
  public get userService(): UserService {
    if (!this._userService) {
      this._userService = this.injector.get(UserService);
    }
    return this._userService;
  }

  private _reservationService: ReservationService;
  public get ReservationService(): ReservationService {
    if (!this._reservationService) { // esta seteado?
      this._reservationService = this.injector.get(ReservationService); // lo setea
    }
    return this._reservationService;
  }

  private _roomService: RoomService;
  public get RoomService(): RoomService {
    if (!this._roomService) { // esta seteado?
      this._roomService = this.injector.get(RoomService); // lo setea
    }
    return this._roomService;
  }

  private _officeService: OfficeService;
  public get OfficeService(): OfficeService {
    if (!this._officeService) { // esta seteado?
      this._officeService = this.injector.get(OfficeService); // lo setea
    }
    return this._officeService;
  }

  private _roleService: RoleService;
  public get RoleService(): RoleService {
    if (!this._roleService) { // esta seteado?
      this._roleService = this.injector.get(RoleService); // lo setea
    }
    return this._roleService;
  }

  private _stageService: StageService;
  public get stageService(): StageService {
    if (!this._stageService) {
      this._stageService = this.injector.get(StageService);
    }
    return this._stageService;
  }

  private _modalService: NzModalService;
  public get modalService(): NzModalService {
    if (!this._modalService) {
      this._modalService = this.injector.get(NzModalService);
    }
    return this._modalService;
  }

  private _messageService: NzMessageService;
  public get messageService(): NzMessageService {
    if (!this._messageService) {
      this._messageService = this.injector.get(NzMessageService);
    }
    return this._messageService;
  }



  private _toastrService: ToastrService;
  public get toastrService(): ToastrService {
    if (!this._toastrService) {
      this._toastrService = this.injector.get(ToastrService);
    }
    return this._toastrService;
  }

  private _authService: AuthService;
  public get authService(): AuthService {
    if (!this._authService) {
      this._authService = this.injector.get(AuthService);
    }
    return this._authService;
  }

  private _declineReasonService: DeclineReasonService;
  public get declineReasonService(): DeclineReasonService {
    if (!this._declineReasonService) {
      this._declineReasonService = this.injector.get(DeclineReasonService);
    }
    return this._declineReasonService;
  }

  private _dashboardService: DashboardService;
  public get dashboardService(): DashboardService {
    if (!this._dashboardService) {
      this._dashboardService = this.injector.get(DashboardService);
    }
    return this._dashboardService;
  }

  private _searchbarService: SearchbarService;
  public get searchbarService(): SearchbarService {
    if (!this._searchbarService) {
      this._searchbarService = this.injector.get(SearchbarService);
    }
    return this._searchbarService;
  }

  constructor(private injector: Injector) {

  }

}
