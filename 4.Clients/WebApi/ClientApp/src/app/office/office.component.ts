import { trimValidator } from 'src/app/directives/trim.validator';
import { Component, OnInit, TemplateRef, Input, SimpleChanges, OnChanges } from '@angular/core';
import { AppComponent } from '../app.component';
import { FacadeService } from '../services/facade.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SettingsComponent } from '../settings/settings.component';
import { Office } from 'src/entities/office';
import { Room } from 'src/entities/room';

@Component({
  selector: 'app-office',
  templateUrl: './office.component.html',
  styleUrls: ['./office.component.css']
})
export class OfficeComponent implements OnInit, OnChanges {
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

  officeForm: FormGroup;
  offices: Office[] = [];
  rooms: Room[] = [];
  isDetailsVisible = false;
  officeDetails: Office;

  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent, private settings: SettingsComponent) { }

  ngOnInit() {
    this.app.removeBgImage();
    this.getRooms();

    this.officeForm = this.fb.group({
      name: [null, [Validators.required]],
      description: [null, [Validators.required]],
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    // tslint:disable-next-line: no-unused-expression
    changes._detailedOffice;
    // tslint:disable-next-line: no-unused-expression
    changes._detailedRoom;
    this.getOffices();
    this.getRooms();
  }

  getOffices() {
    this.facade.OfficeService.get().subscribe(res => {
      this.offices = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
  }

  getRooms() {
    this.facade.RoomService.get()
      .subscribe(res => {
        this.rooms = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
    });
  }

  showDeleteConfirm(officeId: number) {
    const deleteOffice = this._detailedOffice.filter(office => office.id === officeId)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + deleteOffice.name + ' office?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.OfficeService.delete(officeId)
        .subscribe(res => {
          this.settings.getOffices();
          this.facade.toastrService.success('Office was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  resetForm() {
    this.officeForm = this.fb.group({
      name: [null, [Validators.required, trimValidator]],
      description: [null, [Validators.required, trimValidator]]
    });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.resetForm();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Office',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: [{
        label: 'Cancel',
        shape: 'default',
        onClick: () => modal.destroy()
      },
      {
        label: 'Save',
        type: 'primary',
        loading: false,
        onClick: () => {
          this.app.showLoading();
          modal.nzFooter[1].loading = true;
          let isCompleted = true;
          for (const i in this.officeForm.controls) {
            if (this.officeForm.controls[i]) {
              this.officeForm.controls[i].markAsDirty();
              this.officeForm.controls[i].updateValueAndValidity();
              if ((!this.officeForm.controls[i].valid)) { isCompleted = false; }
            }
          }
          if (isCompleted) {
            const addOffice: Office = {
              id: 0,
              name: this.officeForm.controls['name'].value,
              description: this.officeForm.controls['description'].value,
              roomItems: null
            };
            this.facade.OfficeService.add(addOffice)
              .subscribe(res => {
                this.settings.getOffices();
                this.app.hideLoading();
                this.facade.toastrService.success('Office successfully created !');
                modal.destroy();
              }, err => {
                this.app.hideLoading();
                modal.nzFooter[1].loading = false;
                this.facade.errorHandlerService.showErrorMessage(err);
              });
          } else { modal.nzFooter[1].loading = false; }
          this.app.hideLoading();
        }
      }
      ]
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, officeId: number) {
    this.resetForm();
    const editOffice: Office = this._detailedOffice.filter(o => o.id === officeId)[0];
    this.fillOfficeForm(editOffice);
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Office',
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
            for (const control in this.officeForm.controls) {
              if (this.officeForm.controls[control]) {
                this.officeForm.controls[control].markAsDirty();
                this.officeForm.controls[control].updateValueAndValidity();
                if (!this.officeForm.controls[control].valid) { isCompleted = false; }
              }
            }
            if (isCompleted) {
              editOffice.name = this.officeForm.controls['name'].value;
              editOffice.description = this.officeForm.controls['description'].value;
              this.facade.OfficeService.update(officeId, editOffice).subscribe(res => {
                this.settings.getOffices();
                this.facade.toastrService.success('Office was successfully edited !');
                modal.destroy();
              }, err => {
                modal.nzFooter[1].loading = false;
                this.facade.errorHandlerService.showErrorMessage(err);
              });
            } else { modal.nzFooter[1].loading = false; }
          }
        }]
    });
  }

  fillOfficeForm(editOffice: Office) {
    this.officeForm.controls['name'].setValue(editOffice.name);
    this.officeForm.controls['description'].setValue(editOffice.description);
  }

  showDetailsModal(officeId: number) {
    this.officeDetails = this._detailedOffice.find(o => o.id === officeId);
    this.isDetailsVisible = true;
  }

  getColor(candidateroom: Room[], room: Room): string {
    const colors: string[] = ['red', 'volcano', 'orange', 'gold', 'lime', 'green', 'cyan', 'blue', 'geekblue', 'purple'];
    let index: number = candidateroom.indexOf(room);
    if (index > colors.length) { index = parseInt((index / colors.length).toString().split(',')[0], 10); }
    return colors[index];
  }
}
