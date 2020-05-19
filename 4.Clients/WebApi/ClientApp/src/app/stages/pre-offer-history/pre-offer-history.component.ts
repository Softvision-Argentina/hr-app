import { Component, OnInit, TemplateRef, Input, ViewChild, OnDestroy } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { PreOffer } from 'src/entities/pre-offer';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { preOfferStatusEnum } from 'src/entities/enums/pre-offer-status.enum';
import { Globals } from 'src/app/app-globals/globals';


@Component({
  selector: 'pre-offer-history',
  templateUrl: './pre-offer-history.component.html',
  styleUrls: ['./pre-offer-history.component.css']
})
// tslint:disable-next-line:component-class-suffix
export class PreOfferHistory implements OnInit {
  @Input()
  private _processId: number;
  public get processId():  number {
    return this._processId;
  }
  public set processId(value: number) {
      this._processId = value;
  }

  editForm: FormGroup;
  preOfferStatusList: any[];
  preOffers: PreOffer[] = [];

  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {
    this.preOfferStatusList = globals.preOfferStatusList;
  }

  ngOnInit() {
    this.getPreOffers();
  }

  getPreOffers() {
    this.facade.preOfferService.get()
      .subscribe(res => {
        this.preOffers = res.filter(x => x.processId === this.processId);
      },
      err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  showModal(modalContent: TemplateRef <{}>) {
    const modal = this.facade.modalService.create({
        nzTitle: 'Pre-Offer history',
        nzContent: modalContent,
        nzClosable: false,
        nzWrapClassName: 'vertical-center-modal',
        nzWidth: 1000,
        nzFooter: null,
        nzMaskClosable: false,
        nzKeyboard: false
    });
  }

  addPreOffer(modalContent: TemplateRef<{}>) {
    this.resetEditForm();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add Offer',
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
          type: 'primary',
          loading: false,
          onClick: () => {
            modal.nzFooter[1].loading = true;
            let isCompleted = true;

            for (const i in this.editForm.controls) {
              if (this.editForm.controls.hasOwnProperty(i)) {
                this.editForm.controls[i].markAsDirty();
                this.editForm.controls[i].updateValueAndValidity();

                if (!this.editForm.controls[i].valid) {
                  isCompleted = false;
                }
              }
            }

            if (isCompleted) {
              const toAdd: PreOffer = new PreOffer();
              toAdd.id = 0;
              toAdd.offerDate = this.editForm.controls['offerDate'].value;
              toAdd.salary = this.editForm.controls['salaryOffer'].value;
              toAdd.rejectionReason = '';
              toAdd.status = preOfferStatusEnum.Pending;
              toAdd.processId = this.processId;
              this.facade.preOfferService.add(toAdd)
                .subscribe(() => {
                  this.getPreOffers();
                  this.facade.toastrService.success('Pre-Offer was successfuly created!');
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
              });
            } else {
              modal.nzFooter[1].loading = false;
            }
          }
        }],
    });
  }

  editStatus(status: string, id: number): void {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to change the status?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzWidth: '50%',
      nzOnOk: () => {
        const editedPreOffer: PreOffer = this.preOffers.filter(preOffer => preOffer.id === id)[0];
        if (status === 'Declined') {
          editedPreOffer.status = preOfferStatusEnum.Declined;
        } else {
          editedPreOffer.status = preOfferStatusEnum.Accepted;
        }
        this.facade.preOfferService.update(id, editedPreOffer)
        .subscribe(() => {
          this.getPreOffers();
          this.facade.toastrService.success('Offer was successfully edited !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        });
      }
    });
  }

  editRejectionReason(id: number, reason: string) {
    const editedPreOffer: PreOffer = this.preOffers.filter(offer => offer.id === id)[0];
    editedPreOffer.rejectionReason = reason;
    this.facade.preOfferService.update(id, editedPreOffer)
    .subscribe(() => {
      this.getPreOffers();
      this.facade.toastrService.success('Pre-Offer was successfully edited!');
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
  }

  getStatusName(status: number): string {
    return this.preOfferStatusList.find(st => st.id === status).name;
  }

  showDeleteConfirm(id: number): void {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to delete?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzWidth: '50%',
      nzOnOk: () => this.facade.preOfferService.delete(id)
      .subscribe(() => {
        this.getPreOffers();
        this.facade.toastrService.success('Pre-Offer was deleted!');
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      })
    });
  }

  resetEditForm() {
    this.editForm = this.fb.group({
      preOfferDate: [new Date(), [Validators.required]],
      salaryOffer: [null, [Validators.required]],
    });
  }

  preOfferConcluded(): boolean {
    if (this.preOffers.length === 0) {
      return false;
    } else {
      return !(this.preOffers[this.preOffers.length - 1].status === preOfferStatusEnum.Declined);
    }
  }

  getStatusColor(status: number): string {
    const statusName = this.getStatusName(status);
    switch (statusName) {
      case 'Declined': return 'error';
      case 'Accepted': return 'success';
      case 'Pending': return 'processing';
    }
  }

  validateReasons() {
    if (this.preOffers.length === 0) {
      this.facade.modalService.openModals[1].destroy();
    } else if ((this.preOffers.filter(x => x.rejectionReason === '' && x.status === preOfferStatusEnum.Declined)).length === 0) {
      this.facade.modalService.openModals[1].destroy();
    }
  }
}
