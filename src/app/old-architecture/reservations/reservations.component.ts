import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Office } from '@shared/models/office.model';
import { Reservation } from '@shared/models/reservation.model';
import { Room } from '@shared/models/room.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import differenceInCalendarDays from 'date-fns/differenceInCalendarDays';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';

// check in
@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.scss']
})
export class ReservationsComponent implements OnInit {

  @Input()
  private _users: User[];
  public get users(): User[] {
    return this._users;
  }
  public set users(value: User[]) {
    this.fillUsers = value;
    }

  fillUsers: User[] = [];
  reservations: Reservation[] = [];
  room: Room[] = [];
  filteredRoom: Room[] = [];
  offices: Office[] = [];
  currentUser: User;
  currentDayReservations: Reservation[] = [];
  selectedOffice: string;
  selectedDate: Date;
  calendarMode: string;

  today = new Date();
  startValue: Date | null = null;
  endValue: Date | null = null;

  lastHour = 18; // Day time when day ends, used for multiple days reservations

  constructor(private fb: FormBuilder, private facade: FacadeService, private modalService: NzModalService) { }

  reservationForm: FormGroup = this.fb.group({
    description: [null, [Validators.required]],
    sinceReservation: [null, [Validators.required]],
    untilReservation: [null, [Validators.required]],
    user: [null, [Validators.required]],
    roomId: [null, [Validators.required]],
    room: [null],
    office: [2, [Validators.required]]
  });

  ngOnInit() {
    this.fillUsers = this._users;
    this.getUsers();
    this.getReservations();
    this.getOffices();
    this.getRooms();
    this.selectedDate = new Date();
    this.calendarMode = 'month';
    this.selectedOffice = '2';
  }

  getUsers() {
    this.facade.userService.get()
      .subscribe(res => {
        this.fillUsers = res.sort((a,b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
    this.facade.appService.stopLoading();
  }

  async getReservations() {
    await this.facade.ReservationService.get()
      .toPromise()
      .then(res => this.reservations = res.filter(r => r.room.officeId.toString() == this.selectedOffice))
      .catch(err => this.facade.errorHandlerService.showErrorMessage(err));
  }

  getRooms() {
    this.facade.RoomService.get()
      .subscribe(res => {
        this.room = res;
        this.filteredRoom = this.room.filter(c => c.officeId === this.reservationForm.controls['office'].value).sort((a,b) => (a.name.localeCompare(b.name)));
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getOffices() {
    this.facade.OfficeService.get()
      .subscribe(res => {
        this.offices = res.sort((a,b) => (a.name.localeCompare(b.name)));
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  officeChanged(officeId) {
    this.reservationForm.controls['roomId'].reset();
    this.filteredRoom = this.room.filter(c => c.officeId === officeId);
  }


  addReservationModal(modalContent: TemplateRef<{}>): void {
    this.reservationForm.reset();
    this.startValue = null;
    this.endValue = null;
    const modal: NzModalRef = this.modalService.create({
      nzTitle: 'New Reservation',
      nzContent: modalContent,
      nzWidth: '50%',
      nzFooter: [
        {
          label: 'New',
          type: 'primary',
          onClick: () => {
            this.reservateDay(modal);
          }
        },
        {
          label: 'Close',
          onClick: () => modal.destroy()
        }
      ]
    });
  }

  showEditModal(reservation: Reservation, modalContent: TemplateRef<{}>) {
    this.fillEditReservationForm(reservation);
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Edit Reservation',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '50%',
      nzFooter: [
        {
          label: 'Cancel',

          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          onClick: () => {
            this.editReservation(modal, reservation);
          }
        }
      ]
    });
  }

  showDeleteModal(reservationId: number) {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete this reservation?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.ReservationService.delete(reservationId)
        .subscribe(async res => {
          await this.getReservations();
          this.getCurrentDayReservations();
          this.facade.toastrService.success('Reservation deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  fillEditReservationForm(reservation: Reservation) {
    this.reservationForm.controls['office'].setValue(reservation.room.officeId);
    this.reservationForm.controls['roomId'].setValue(reservation.room.id);
    this.reservationForm.controls['description'].setValue(reservation.description);
    this.reservationForm.controls['user'].setValue(reservation.user);
    this.reservationForm.controls['sinceReservation'].setValue(reservation.sinceReservation);
    this.reservationForm.controls['untilReservation'].setValue(reservation.untilReservation);
  }

  disabledStartDate = (date: Date): boolean => {
    if (!this.endValue) {
      return differenceInCalendarDays(date, this.today) < 0;
    }
    return differenceInCalendarDays(date, this.today) < 0 ||
      date.getMonth() > this.endValue.getMonth() ||
      (date.getMonth() === this.endValue.getMonth() && date.getDate() > this.endValue.getDate());
  };

  disabledEndDate = (date: Date): boolean => {
    if (!date || !this.startValue) {
      return differenceInCalendarDays(date, this.today) < 0;
    }
    return differenceInCalendarDays(date, this.today) < 0 ||
      date.getMonth() < this.startValue.getMonth() ||
      (date.getMonth() === this.startValue.getMonth() && date.getDate() < this.startValue.getDate());
  };

  disabledDateTime = (): object => {
    return {
      nzDisabledHours: () => this.range(0, 24).splice(0, 8)
    };
  };

  onStartChange(date: Date): void {
    this.startValue = date;
  }

  onEndChange(date: Date): void {
    this.endValue = date;
  }

  range(start: number, end: number): number[] {
    const result: number[] = [];
    for (let i = start; i < end; i++) {
      result.push(i);
    }
    return result;
  }

  reservateDay(modal: NzModalRef) {
    let until = new Date(this.reservationForm.controls['untilReservation'].value);
    let since = new Date(this.reservationForm.controls['sinceReservation'].value);
    let newSince = new Date(this.reservationForm.controls['sinceReservation'].value);
    let newUntil = new Date(this.reservationForm.controls['untilReservation'].value);
    let totalDays = differenceInCalendarDays(until, since);
    let startHour = since.getHours();
    let finishHour = until.getHours();
    let isCompleted = this.validateForm();

    if (isCompleted) {
      for (var i = 0; i <= totalDays; i++) {
        newSince.setDate(since.getDate() + i);
        newUntil.setDate(until.getDate() - (totalDays - i));

        if (i < totalDays) {
          newUntil.setHours(this.lastHour, 0, 0, 0);
        }

        let newReservation: Reservation = {
          id: 0,
          description: this.reservationForm.controls['description'].value.toString(),
          sinceReservation: newSince,
          untilReservation: newUntil,
          user: this.reservationForm.controls['user'].value,
          roomId: this.reservationForm.controls['roomId'].value,
          room: null
        }
        let overlap = this.checkOverlap(0, newReservation);
        if (overlap) {
          this.facade.toastrService.error('There is already a reservation for this moment.');
        }
        else {
          this.facade.ReservationService.add(newReservation)
            .subscribe(async res => {
              await this.getReservations();
              this.getCurrentDayReservations();
              this.facade.toastrService.success('Reservation was successfully created!');
              modal.destroy();
            }, err => {
              this.facade.errorHandlerService.showErrorMessage(err);
            })
        }
      }
    }
  };

  editReservation(modal: NzModalRef, reservation: Reservation) {
    let isCompleted = this.validateForm();
    if (isCompleted) {
      let editReservation: Reservation = {
        id: 0,
        description: this.reservationForm.controls['description'].value,
        roomId: this.reservationForm.controls['roomId'].value,
        room: null,
        user: this.reservationForm.controls['user'].value,
        sinceReservation: this.reservationForm.controls['sinceReservation'].value,
        untilReservation: this.reservationForm.controls['untilReservation'].value
      }
      let overlap = this.checkOverlap(reservation.id, editReservation);
      if (overlap) {
        this.facade.toastrService.error('There is already a reservation for this moment.');
      }
      else {
        let editReservationSince = new Date(Date.parse(editReservation.sinceReservation.toString())).getDate();
        let editReservationUntil = new Date(Date.parse(editReservation.untilReservation.toString())).getDate();
        let reservationSince = new Date(Date.parse(reservation.sinceReservation.toString())).getDate();
        let reservationUntil = new Date(Date.parse(reservation.untilReservation.toString())).getDate();
        if (editReservationSince !== reservationSince ||
          editReservationUntil !== reservationUntil) {
          this.facade.ReservationService.delete(reservation.id)
            .subscribe(async res => {
              await this.getReservations();
              this.reservateDay(modal);
            }, err => {
              this.facade.errorHandlerService.showErrorMessage(err);
            })
        }
        else {
          this.facade.ReservationService.update(reservation.id, editReservation)
            .subscribe(async res => {
              await this.getReservations();
              this.getCurrentDayReservations();
              this.facade.toastrService.success('Reservation was successfully updated!');
              modal.destroy();
            }, err => {
              this.facade.errorHandlerService.showErrorMessage(err);
            })
        }
      }
    }
  }

  checkOverlap(reservationId: number, newReservation: Reservation): boolean {
    let newReservationRoomId = newReservation.roomId;
    let newReservationSince = Math.floor(Date.parse(newReservation.sinceReservation.toString()) / 60000);
    let newReservationUntil = Math.floor(Date.parse(newReservation.untilReservation.toString()) / 60000);

    let roomReservations = this.reservations.filter((reservation) => reservation.roomId === newReservationRoomId);

    for (let reservation of roomReservations) {
      let currentReservationSince = Math.floor(Date.parse(reservation.sinceReservation.toString()) / 60000)
      let currentReservationUntil = Math.floor(Date.parse(reservation.untilReservation.toString()) / 60000)
      if (reservationId !== reservation.id &&
        (((newReservationSince >= currentReservationSince && newReservationSince < currentReservationUntil) ||
          (newReservationUntil > currentReservationSince && newReservationUntil <= currentReservationUntil)) ||
          ((currentReservationSince >= newReservationSince && currentReservationSince < newReservationUntil) ||
            (currentReservationUntil > newReservationSince && currentReservationUntil <= newReservationUntil)))
      ) {
        return true;
      }
    }
    return false;
  }

  getCurrentDayReservations(): void {
    this.currentDayReservations = this.reservations
      .filter(r => r.sinceReservation.toString().substr(0, 10) === this.selectedDate.toISOString().substr(0, 10));
  }

  onChangeOffice() {
    this.currentDayReservations = [];
    this.getReservations();
  }

  validateForm() {
    let isCompleted: boolean = true;
    let sinceHours = new Date(this.reservationForm.controls['sinceReservation'].value).getHours();
    let untilHours = new Date(this.reservationForm.controls['untilReservation'].value).getHours();
    for (const i in this.reservationForm.controls) {
      this.reservationForm.controls[i].markAsDirty();
      this.reservationForm.controls[i].updateValueAndValidity();
      if (!this.reservationForm.controls[i].valid) isCompleted = false;
    }
    if (sinceHours > untilHours) {
      this.facade.toastrService.error('The selected schedule is invalid');
      isCompleted = false;
    }
    return isCompleted;
  }
}
