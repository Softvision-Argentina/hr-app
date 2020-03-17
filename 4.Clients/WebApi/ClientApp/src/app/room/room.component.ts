import { Component, OnInit, TemplateRef, Input, SimpleChanges } from '@angular/core';
import { FacadeService } from '../services/facade.service';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import { SettingsComponent } from '../settings/settings.component';
import { trimValidator } from '../directives/trim.validator';
import { Office } from 'src/entities/office';
import { Room } from 'src/entities/room';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit {

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

  rooms: Room[] = [];
  offices: Office[] = [];
  roomForm: FormGroup;
  isEdit = false;
  editingRoomId: number = 0;

  constructor(private fb: FormBuilder, private facade: FacadeService, private settings: SettingsComponent) { }

  ngOnInit() {    
    this.getOffices();
  }

  ngOnChanges(changes: SimpleChanges) {
    changes._detailedOffice;
    this.getOffices();    
  }

  getOfficeNameByID(id: number) {
    const COffice = this.offices.find(c => c.id === id);
    return COffice !== undefined ? COffice.name : '';
  }

  getOffices() {
    this.facade.OfficeService.get()
    .subscribe(res => {
      this.offices = res;
    }, err => {
      console.log(err);
    });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.isEdit = false;
    this.resetForm();
    if (this.offices.length > 0) {
    this.roomForm.controls['profileId'].setValue(this.offices[0].id);
    }
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Room',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            modal.nzFooter[1].loading = true; // el boton de guardar cambios cambia a true
            let isCompleted = true;
            for (const i in this.roomForm.controls) {
              this.roomForm.controls[i].markAsDirty();
              this.roomForm.controls[i].updateValueAndValidity();
              if ((!this.roomForm.controls[i].valid)) { isCompleted = false; }
            }
            if (isCompleted) {
              let newRoom: Room = {
                id: 0,
                name: this.roomForm.controls['name'].value.toString(),
                description: this.roomForm.controls['description'].value.toString(),
                officeId: this.roomForm.controls['profileId'].value.toString(),
                office: null,
                reservationItems: null
              };
              this.facade.RoomService.add(newRoom)
                .subscribe(res => {          
                  this.settings.getRooms();                  
                  this.facade.toastrService.success('Room was successfully created !');

                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  if (err.message !== undefined) { this.facade.toastrService.error(err.message); } else
                  { this.facade.toastrService.error('The service is not available now. Try again later.'); }
                });
            } else { modal.nzFooter[1].loading = false; }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {    
    this.resetForm();
    this.editingRoomId = id;
    this.isEdit = true;    
    let editedRoom: Room = this._detailedRoom.filter(Room => Room.id === id)[0];

    this.fillRoomForm(editedRoom);

    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Room',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            modal.nzFooter[1].loading = true;
            let isCompleted = true;
            for (const i in this.roomForm.controls) {
              this.roomForm.controls[i].markAsDirty();
              this.roomForm.controls[i].updateValueAndValidity();
              if (!this.roomForm.controls[i].valid) { isCompleted = false; }
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
                .subscribe(res => {
                  this.settings.getRooms();
                  this.facade.toastrService.success('Room was successfully edited !');
                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  if (err.message !== undefined) { this.facade.toastrService.error(err.message); } else
                   { this.facade.toastrService.error('The service is not available now. Try again later.'); }
                });
            } else { modal.nzFooter[1].loading = false; }
          }
        }],
    });
  }

  showDeleteConfirm(RoomID: number): void {
  const RoomDelete: Room = this._detailedRoom.filter(c => c.id === RoomID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete ' + RoomDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.RoomService.delete(RoomID)
        .subscribe(res => {
          this.settings.getRooms();
          this.facade.toastrService.success('Room was deleted !');
        }, err => {
          if (err.message !== undefined) { this.facade.toastrService.error(err.message); } else { this.facade.toastrService.error('The service is not available now. Try again later.'); }
        })
    });
  }

  fillRoomForm(Room: Room) {
    this.roomForm.controls['name'].setValue(Room.name);
    this.roomForm.controls['description'].setValue(Room.description);
    if (this.offices.length > 0) {
    this.roomForm.controls['profileId'].setValue(this.offices[0].id);
    }
  }

  resetForm() {
    this.roomForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]],
      description: [null, [Validators.required, trimValidator]],
      profileId: [null, [Validators.required, trimValidator]]
    });
  }
}