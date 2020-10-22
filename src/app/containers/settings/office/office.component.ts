import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Office } from '@shared/models/office.model';
import { Room } from '@shared/models/room.model';
import { FacadeService } from '@shared/services/facade.service';
import { OfficeSandbox } from './office.sandbox';

@Component({
  selector: 'app-office',
  templateUrl: './office.component.html',
  styleUrls: ['./office.component.scss']
})
export class OfficeComponent implements OnInit {

  @Output() officesChanged = new EventEmitter();

  officeForm: FormGroup;
  offices: Office[] = [];
  rooms: Room[] = [];
  isDetailsVisible = false;
  officeDetails: Office;

  constructor(private facade: FacadeService, private fb: FormBuilder, private officeSandbox: OfficeSandbox) { }

  ngOnInit() {
    this.facade.appService.removeBgImage();
    this.getRooms();
    this.getOffices();


    this.officeForm = this.fb.group({
      name: [null, [Validators.required]],
      description: [null, [Validators.required]],
    });
  }

  getOffices() {
    this.officeSandbox.loadOffices();
    this.officeSandbox.offices$.subscribe(res => {
      this.offices = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });

  }

  getRooms() {
    this.officeSandbox.loadRooms();
    this.officeSandbox.rooms$.subscribe(res => {
      this.rooms = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    })
  }

  showDeleteConfirm(officeId: number) {
    const deleteOffice = this.offices.filter(office => office.id === officeId)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + deleteOffice.name + ' office?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.officeSandbox.removeOffice(officeId);
        const errorMsg = this.officeSandbox.communtiesErrorMsg$
          .subscribe(() => {
            this.officesChanged.emit();
            this.facade.toastrService.success('Office was deleted !');
          }, err => {
            this.facade.errorHandlerService.showErrorMessage(err);
          })
        errorMsg.unsubscribe();
      }
    });
  }

  resetForm() {
    this.officeForm = this.fb.group({
      name: [null, Validators.required],
      description: [null, [Validators.required]]
    });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.resetForm();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Office',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'recru-modal recru-modal--sm',
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
            this.facade.appService.startLoading();
            let isCompleted = true;
            for (const i in this.officeForm.controls) {
              if (this.officeForm.controls.hasOwnProperty(i)) {
                this.officeForm.controls[i].markAsDirty();
                this.officeForm.controls[i].updateValueAndValidity();
                if (!this.officeForm.controls[i].valid) {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {
              const addOffice: Office = {
                id: 0,
                name: this.officeForm.controls['name'].value,
                description: this.officeForm.controls['description'].value,
                roomItems: null
              };
              this.officeSandbox.addOffice(addOffice);
              const errorMsg = this.officeSandbox.communtiesErrorMsg$
                .subscribe(() => {
                  this.facade.appService.stopLoading();
                  this.facade.toastrService.success('Office successfully created !');
                  modal.destroy();
                }, err => {
                  this.facade.appService.stopLoading();
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
              errorMsg.unsubscribe();
            }
            this.facade.appService.stopLoading();
          }
        }
      ]
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, officeId: number) {
    this.resetForm();
    const editOffice: Office = this.offices.filter(office => office.id === officeId)[0];
    this.fillOfficeForm(editOffice);
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--sm',
      nzTitle: 'Edit Office',
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
            for (const control in this.officeForm.controls) {
              if (this.officeForm.controls.hasOwnProperty(control)) {
                this.officeForm.controls[control].markAsDirty();
                this.officeForm.controls[control].updateValueAndValidity();
                if (!this.officeForm.controls[control].valid) {
                  isCompleted = false;
                }
              }
            }

            if (isCompleted) {
              const editedOffice = {
                id: officeId,
                name: this.officeForm.controls.name.value,
                description: this.officeForm.controls.description.value,
                roomItems: editOffice.roomItems
              };
              this.officeSandbox.editOffice(editedOffice);
              const errorMsg = this.officeSandbox.communtiesErrorMsg$.subscribe(() => {
                this.facade.toastrService.success('Office was successfully edited !');
                modal.destroy();
              }, err => {
                this.facade.errorHandlerService.showErrorMessage(err);
              });
              errorMsg.unsubscribe();
            }
          }
        }]
    });
  }

  fillOfficeForm(editOffice: Office) {
    this.officeForm.controls['name'].setValue(editOffice.name);
    this.officeForm.controls['description'].setValue(editOffice.description);
  }

  showDetailsModal(officeId: number) {
    this.officeDetails = this.offices.find(o => o.id === officeId);
    this.isDetailsVisible = true;
  }

  getColor(candidateroom: Room[], room: Room): string {
    const colors: string[] = ['red', 'volcano', 'orange', 'gold', 'lime', 'green', 'cyan', 'blue', 'geekblue', 'purple'];
    let index: number = candidateroom.indexOf(room);
    if (index > colors.length) {
      index = parseInt((index / colors.length).toString().split(',')[0], 10);
    }
    return colors[index];
  }
}
