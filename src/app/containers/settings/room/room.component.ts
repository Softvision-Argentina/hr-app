import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Office } from '@shared/models/office.model';
import { Room } from '@shared/models/room.model';
import { FacadeService } from '@shared/services/facade.service';
import { RoomSandbox } from './room.sandbox';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})
export class RoomComponent implements OnInit, OnChanges {

  @Input()
  private _detailedRoom: Room[];
  public get detailedRoom(): Room[] {
    return this._detailedRoom;
  }
  public set detailedRoom(value: Room[]) {
    this._detailedRoom = value;
  }

  @Input()
  private _detailedOffice: Office[];
  public get detailedOffice(): Office[] {
    return this._detailedOffice;
  }
  public set detailedOffice(value: Office[]) {
    this._detailedOffice = value;
  }

  @Output() roomsChanged = new EventEmitter();

  rooms: Room[] = [];
  offices: Office[] = [];
  roomForm: FormGroup;
  isEdit = false;
  editingRoomId = 0;

  constructor(private fb: FormBuilder, private facade: FacadeService, private roomSandbox: RoomSandbox) { }

  ngOnInit() {
    this.getOffices();
    this.getRooms();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.getOffices();
  }

  getOfficeNameByID(id: number) {
    const COffice = this.offices.find(c => c.id === id);
    return COffice !== undefined ? COffice.name : '';
  }

  getOffices() {
    this.roomSandbox.loadOffices();
    this.roomSandbox.offices$.subscribe(res => {
      this.offices = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });

  }

  getRooms() {
    this.roomSandbox.loadRooms();
    this.roomSandbox.rooms$.subscribe(res => {
      this.rooms = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });

  }
  showAddModal(modalContent: TemplateRef<{}>): void {
    this.isEdit = false;
    this.resetForm();
    if (this.offices.length > 0) {
      this.roomForm.controls['profileId'].setValue(this.offices[0].id);
    }
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Add New Room',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        {
          label: 'Cancel',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            let isCompleted = true;
            for (const i in this.roomForm.controls) {
              if (this.roomForm.controls.hasOwnProperty(i)) {
                this.roomForm.controls[i].markAsDirty();
                this.roomForm.controls[i].updateValueAndValidity();
                if ((!this.roomForm.controls[i].valid)) {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {
              const newRoom: Room = {
                id: 0,
                name: this.roomForm.controls['name'].value.toString(),
                description: this.roomForm.controls['description'].value.toString(),
                officeId: this.roomForm.controls['profileId'].value,
                office: null,
                reservationItems: null
              };
              this.roomSandbox.addRoom(newRoom)
              const errorMsg = this.roomSandbox.roomsErrorMsg$
                .subscribe(() => {
                  this.facade.toastrService.success('Room was successfully created !');
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
              errorMsg.unsubscribe();
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.resetForm();
    this.editingRoomId = id;
    this.isEdit = true;
    let editedRoom: Room = this.rooms.filter(room => room.id === id)[0];

    this.fillRoomForm(editedRoom);

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Edit Room',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        {
          label: 'Cancel',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            let isCompleted = true;
            for (const i in this.roomForm.controls) {
              if (this.roomForm.controls.hasOwnProperty(i)) {
                this.roomForm.controls[i].markAsDirty();
                this.roomForm.controls[i].updateValueAndValidity();
                if (!this.roomForm.controls[i].valid) {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {
              editedRoom = {
                id: id,
                name: this.roomForm.controls['name'].value.toString(),
                description: this.roomForm.controls['description'].value.toString(),
                officeId: this.roomForm.controls['profileId'].value,
                office: null,
                reservationItems: null
              };
              this.roomSandbox.editRoom(editedRoom);
              const errorMsg = this.roomSandbox.roomsErrorMsg$.subscribe(() => {
                this.facade.toastrService.success('Room was successfully edited !');
                modal.destroy();
              }, err => {
                this.facade.errorHandlerService.showErrorMessage(err);
              });
              errorMsg.unsubscribe();
            }
          }
        }],
    });
  }

  showDeleteConfirm(RoomID: number): void {
    const RoomDelete: Room = this.rooms.filter(c => c.id === RoomID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + RoomDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.roomSandbox.removeRoom(RoomID);
        const errorMsg = this.roomSandbox.roomsErrorMsg$
          .subscribe(() => {
            this.facade.toastrService.success('Room was deleted !');
          }, err => {
            this.facade.errorHandlerService.showErrorMessage(err);
          })
        errorMsg.unsubscribe();
      }
    });
  }

  fillRoomForm(room: Room) {
    this.roomForm.controls['name'].setValue(room.name);
    this.roomForm.controls['description'].setValue(room.description);
    if (this.offices.length > 0) {
      this.roomForm.controls['profileId'].setValue(this.offices[0].id);
    }
  }

  resetForm() {
    this.roomForm = this.fb.group({
      name: [null, Validators.required],
      description: [null, [Validators.required]],
      profileId: [null, [Validators.required]]
    });
  }
}
