import { Component, OnInit, TemplateRef, Input, ViewChild, OnDestroy } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { PreOffer } from 'src/entities/pre-offer';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { preOfferStatusEnum } from 'src/entities/enums/pre-offer-status.enum';
import { Globals } from 'src/app/app-globals/globals';
import { HealthInsuranceEnum } from 'src/entities/enums/health-insurance.enum';
import { formFieldHasRequiredValidator } from 'src/app/utils/utils.functions';


@Component({
  selector: 'pre-offer-history',
  templateUrl: './pre-offer-history.component.html',
  styleUrls: ['./pre-offer-history.component.scss']
})
// tslint:disable-next-line:component-class-suffix
export class PreOfferHistory implements OnInit {
  @Input()
  private _processId: number;
  public get processId(): number {
    return this._processId;
  }
  public set processId(value: number) {
    this._processId = value;
  }

  editForm: FormGroup = this.fb.group({
    preOfferDate: [new Date()],
    salaryPreOffer: [0, [Validators.required]],
    vacationDays: [0, [Validators.required]],
    healthInsurance: [0, [Validators.required]]
  });

  addPreOfferButton = {
    enabled: false,
    processInvalid: false,
    preOfferNotConcluded: false
  }

  panelControl: any = {
    active: false,
    name: 'Add New Pre-Offer',
    arrow: false
  }

  healthInsuranceList: any[];
  preOfferStatusList: any[];
  preOffers: PreOffer[] = [];

  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {
    this.preOfferStatusList = globals.preOfferStatusList;
    this.healthInsuranceList = globals.healthInsuranceList;
  }

  ngOnInit() {
    this.getPreOffers();
  }

  getPreOffers() {
    this.facade.preOfferService.get()
      .subscribe(res => {
        this.preOffers = res.filter(x => x.processId === this.processId);
        this.validateAddPreOffers();
        this.getHealthInsuranceNames();
      },
        err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        });
  }

  getHealthInsuranceNames(){
    this.preOffers.forEach((preOffer, index) => {
      preOffer.healthInsuranceText = HealthInsuranceEnum[preOffer.healthInsurance];
    })
  }

  showModal(modalContent: TemplateRef<{}>) {
    const modal = this.facade.modalService.create({
      nzTitle: 'Pre-Offer history',
      nzContent: modalContent,
      nzClosable: false,
      nzWrapClassName: 'vertical-center-modal',
      nzWidth: '60%',
      nzFooter: null
    });
  }

  private validateEditForm() {
    for (const i in this.editForm.controls) {
      if (this.editForm.controls.hasOwnProperty(i)) {
        this.editForm.controls[i].markAsDirty();
        this.editForm.controls[i].updateValueAndValidity();
        
        if (this.editForm.controls[i].invalid) {
          return false;
        }
      }
    }
    return true;
  }

  addPreOffer() {
    if (this.validateEditForm()) {
      const editForm: PreOffer = new PreOffer();
      editForm.id = 0;
      editForm.preOfferDate = this.editForm.controls['preOfferDate'].value;
      editForm.salary = this.editForm.controls['salaryPreOffer'].value;
      editForm.vacationDays = this.editForm.controls['vacationDays'].value;
      editForm.healthInsurance = this.editForm.controls['healthInsurance'].value;
      editForm.status = preOfferStatusEnum.PendingReply;
      editForm.processId = this.processId;
      console.log({ editForm: editForm });

      this.facade.preOfferService.add(editForm)
        .subscribe(() => {
          this.getPreOffers();
          this.facade.toastrService.success('Pre-Offer was successfuly created!');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        });
      this.resetEditForm();
    }
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
      salaryPreOffer: [null, [Validators.required]],
      vacationDays: [null, [Validators.required]],
      healthInsurance: [0, [Validators.required]]
    });
  }

  getStatusColor(status: number): string {
    const statusName = this.getStatusName(status);
    switch (statusName) {
      case 'Pending Reply': return 'processing';
      case 'Accepted': return 'success';
      case 'Declined': return 'error';
    }
  }

  validateReasons() {
    this.facade.modalService.openModals[1].destroy();
  }

  validateAddPreOffers() {
    if (this.preOffers.length > 0) {
      var isLastPreOfferDeclined = this.preOffers[this.preOffers.length - 1].status === preOfferStatusEnum.Declined;
      this.addPreOfferButton.preOfferNotConcluded = !isLastPreOfferDeclined;
    } else {
      this.addPreOfferButton.preOfferNotConcluded = false;
    }

    this.addPreOfferButton.processInvalid = this.processId === 0;
    this.addPreOfferButton.enabled = !this.addPreOfferButton.processInvalid && (this.preOffers.length === 0 || !this.addPreOfferButton.preOfferNotConcluded);
  }


  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.editForm);
  }

}
