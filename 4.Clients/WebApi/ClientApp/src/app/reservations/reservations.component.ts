import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { trimValidator } from 'src/app/directives/trim.validator';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { Reservation } from 'src/entities/reservation';
import { Consultant } from 'src/entities/consultant';
import { User } from 'src/entities/user';
import { Room } from 'src/entities/room';
import { Office } from 'src/entities/office';
import * as  differenceInCalendarDays from 'date-fns/difference_in_calendar_days';
import { getDate } from 'date-fns';

// check in
@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent implements OnInit {

  @Input()
  private _consultants: Consultant[];
  public get consultants(): Consultant[] {
    return this._consultants;
  }
  public set consultants(value: Consultant[]) {
    this.recruiters = value;
  }

  recruiters: Consultant[] = [];
  reservations: Reservation[] = [];
  room: Room[] = [];
  filteredRoom: Room[] = [];
  offices: Office[] = [];
  currentConsultant: User;
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
    description: [null, [Validators.required, trimValidator]],
    sinceReservation: [null, [Validators.required]],
    untilReservation: [null, [Validators.required]],
    recruiter: [null, [Validators.required]],
    roomId: [null, [Validators.required, trimValidator]],
    room: [null],
    office: [2, [Validators.required]]
  });

  ngOnInit() {
    this.recruiters = this._consultants;
    this.getConsultants();
    this.getReservations();
    this.getOffices();
    this.getRooms();
    this.selectedDate = new Date();
    this.calendarMode = 'month';
    this.selectedOffice = '2';
  }

  getConsultants() {
    this.facade.consultantService.get()
      .subscribe(res => {
        this.consultants = res;
      }, err => {
        console.log(err);
      });
  }

  async getReservations() {
    await this.facade.ReservationService.get()
      .toPromise()
      .then(res => this.reservations = res.filter(r => r.room.officeId.toString() === this.selectedOffice))
      .catch(err => console.log(err));
    // .subscribe(res => {
    //   this.reservations = res.filter(r => r.room.officeId.toString() === this.selectedOffice);
    // }, err => {
    //   console.log(err);
    // });
  }

  getRooms() {
    this.facade.RoomService.get()
      .subscribe(res => {
        this.room = res;
        this.filteredRoom = this.room.filter(c => c.officeId === this.reservationForm.controls['office'].value);
      }, err => {
        console.log(err);
      });
  }

  getOffices() {
    this.facade.OfficeService.get()
      .subscribe(res => {
        this.offices = res;
      }, err => {
        console.log(err);
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
          shape: 'default',
          onClick: () => modal.destroy()
        }
      ]
    });
  }

  showEditModal(reservation: Reservation, modalContent: TemplateRef<{}>) {
    this.fillEditReservationForm(reservation);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Reservation',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '50%',
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          shape: 'primary',
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
          // tslint:disable-next-line: max-line-length
          if (err.message !== undefined) { this.facade.toastrService.error(err.message); } else { this.facade.toastrService.error('The service is not available now. Try again later.'); }
        })
    });
  }

  fillEditReservationForm(reservation: Reservation) {
    this.reservationForm.controls['office'].setValue(reservation.room.officeId);
    this.reservationForm.controls['roomId'].setValue(reservation.room.id);
    this.reservationForm.controls['description'].setValue(reservation.description);
    this.reservationForm.controls['recruiter'].setValue(reservation.recruiter);
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
  }

  disabledEndDate = (date: Date): boolean => {
    if (!date || !this.startValue) {
      return differenceInCalendarDays(date, this.today) < 0;
    }
    return differenceInCalendarDays(date, this.today) < 0 ||
      date.getMonth() < this.startValue.getMonth() ||
      (date.getMonth() === this.startValue.getMonth() && date.getDate() < this.startValue.getDate());
  }

  disabledDateTime = (): object => {
    return {
      nzDisabledHours: () => this.range(0, 24).splice(0, 8)
    };
  }

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
    const until = new Date(this.reservationForm.controls['untilReservation'].value);
    const since = new Date(this.reservationForm.controls['sinceReservation'].value);
    const newSince = new Date(this.reservationForm.controls['sinceReservation'].value);
    const newUntil = new Date(this.reservationForm.controls['untilReservation'].value);
    const totalDays = differenceInCalendarDays(until, since);
    const startHour = since.getHours();
    const finishHour = until.getHours();
    const isCompleted = this.validateForm();

    if (isCompleted) {
      for (let i = 0; i <= totalDays; i++) {
        newSince.setDate(since.getDate() + i);
        newUntil.setDate(until.getDate() - (totalDays - i));

        if (i < totalDays) {
          newUntil.setHours(this.lastHour, 0, 0, 0);
        }

        const newReservation: Reservation = {
          id: 0,
          description: this.reservationForm.controls['description'].value.toString(),
          sinceReservation: newSince,
          untilReservation: newUntil,
          recruiter: this.reservationForm.controls['recruiter'].value,
          roomId: this.reservationForm.controls['roomId'].value,
          room: null
        };
        const overlap = this.checkOverlap(0, newReservation);
        if (overlap) {
          this.facade.toastrService.error('There is already a reservation for this moment.');
        } else {
          this.facade.ReservationService.add(newReservation)
            .subscribe(async res => {
              await this.getReservations();
              this.getCurrentDayReservations();
              this.facade.toastrService.success('Reservation was successfully created!');
              modal.destroy();
            }, err => {
              // tslint:disable-next-line: max-line-length
              if (err.message !== undefined) { this.facade.toastrService.error(err.message); } else { this.facade.toastrService.error('The service is not available now. Try again later.'); }
            });
        }
      }
    }
  }

  editReservation(modal: NzModalRef, reservation: Reservation) {
    const isCompleted = this.validateForm();
    if (isCompleted) {
      const editReservation: Reservation = {
        id: 0,
        description: this.reservationForm.controls['description'].value,
        roomId: this.reservationForm.controls['roomId'].value,
        room: null,
        recruiter: this.reservationForm.controls['recruiter'].value,
        sinceReservation: this.reservationForm.controls['sinceReservation'].value,
        untilReservation: this.reservationForm.controls['untilReservation'].value
      };
      const overlap = this.checkOverlap(reservation.id, editReservation);
      if (overlap) {
        this.facade.toastrService.error('There is already a reservation for this moment.');
      } else {
        const editReservationSince = new Date(Date.parse(editReservation.sinceReservation.toString())).getDate();
        const editReservationUntil = new Date(Date.parse(editReservation.untilReservation.toString())).getDate();
        const reservationSince = new Date(Date.parse(reservation.sinceReservation.toString())).getDate();
        const reservationUntil = new Date(Date.parse(reservation.untilReservation.toString())).getDate();
        if (editReservationSince !== reservationSince ||
          editReservationUntil !== reservationUntil) {
          this.facade.ReservationService.delete(reservation.id)
            .subscribe(async res => {
              await this.getReservations();
              this.reservateDay(modal);
            }, err => {
              // tslint:disable-next-line: max-line-length
              if (err.message !== undefined) { this.facade.toastrService.error(err.message); } else { this.facade.toastrService.error('The service is not available now. Try again later.'); }
            });
        } else {
          this.facade.ReservationService.update(reservation.id, editReservation)
            .subscribe(async res => {
              await this.getReservations();
              this.getCurrentDayReservations();
              this.facade.toastrService.success('Reservation was successfully updated!');
              modal.destroy();
            }, err => {
              // tslint:disable-next-line: max-line-length
              if (err.message !== undefined) { this.facade.toastrService.error(err.message); } else { this.facade.toastrService.error('The service is not available now. Try again later.'); }
            });
        }
      }
    }
  }

  checkOverlap(reservationId: number, newReservation: Reservation): boolean {
    const newReservationRoomId = newReservation.roomId;
    const newReservationSince = Math.floor(Date.parse(newReservation.sinceReservation.toString()) / 60000);
    const newReservationUntil = Math.floor(Date.parse(newReservation.untilReservation.toString()) / 60000);

    const roomReservations = this.reservations.filter((reservation) => reservation.roomId === newReservationRoomId);

    for (const reservation of roomReservations) {
      const currentReservationSince = Math.floor(Date.parse(reservation.sinceReservation.toString()) / 60000);
      const currentReservationUntil = Math.floor(Date.parse(reservation.untilReservation.toString()) / 60000);
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
    let isCompleted = true;
    const sinceHours = new Date(this.reservationForm.controls['sinceReservation'].value).getHours();
    const untilHours = new Date(this.reservationForm.controls['untilReservation'].value).getHours();
    for (const i in this.reservationForm.controls) {
      if (this.reservationForm.controls[i]) {
        this.reservationForm.controls[i].markAsDirty();
        this.reservationForm.controls[i].updateValueAndValidity();
        if (!this.reservationForm.controls[i].valid) { isCompleted = false; }
      }
    }
    if (sinceHours > untilHours) {
      this.facade.toastrService.error('The selected schedule is invalid');
      isCompleted = false;
    }
    return isCompleted;
  }
}
