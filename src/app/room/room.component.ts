import { Component, OnInit, TemplateRef, Input, SimpleChanges, OnChanges, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import { FacadeService } from '../services/facade.service';
import { Office } from 'src/entities/office';
import { Room } from 'src/entities/room';

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

  constructor(private fb: FormBuilder, private facade: FacadeService) { }

  ngOnInit() {
    this.getOffices();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.getOffices();
  }

  getOfficeNameByID(id: number) {
    const COffice = this.offices.find(c => c.id === id);
    return COffice !== undefined ? COffice.name : '';
  }

  getOffices() {
    this.facade.OfficeService.get()
    .subscribe(res => {
      this.offices = res.sort((a,b) => (a.name.localeCompare(b.name)));
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
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Add New Room',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
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
                officeId: this.roomForm.controls['profileId'].value.toString(),
                office: null,
                reservationItems: null
              };
              this.facade.RoomService.add(newRoom)
                .subscribe(() => {
                  this.roomsChanged.emit();
                  this.facade.toastrService.success('Room was successfully created !');

                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.resetForm();
    this.editingRoomId = id;
    this.isEdit = true;
    let editedRoom: Room = this._detailedRoom.filter(room => room.id === id)[0];

    this.fillRoomForm(editedRoom);

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: 'Edit Room',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
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
                id: 0,
                name: this.roomForm.controls['name'].value.toString(),
                description: this.roomForm.controls['description'].value.toString(),
                officeId: this.roomForm.controls['profileId'].value.toString(),
                office: null,
                reservationItems: null
              };
              this.facade.RoomService.update(id, editedRoom)
                .subscribe(() => {
                  this.roomsChanged.emit();
                  this.facade.toastrService.success('Room was successfully edited !');
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            }
          }
        }],
    });
  }

  showDeleteConfirm(RoomID: number): void {
  const RoomDelete: Room = this._detailedRoom.filter(c => c.id === RoomID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + RoomDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.RoomService.delete(RoomID)
        .subscribe(() => {
          this.roomsChanged.emit();
          this.facade.toastrService.success('Room was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
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
