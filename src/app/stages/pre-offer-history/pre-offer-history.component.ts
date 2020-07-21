import { Component, OnInit, TemplateRef, Input, ViewChild, OnDestroy } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { PreOffer } from 'src/entities/pre-offer';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { preOfferStatusEnum } from 'src/entities/enums/pre-offer-status.enum';
import { Globals } from 'src/app/app-globals/globals';
import { HealthInsuranceEnum } from 'src/entities/enums/health-insurance.enum';
import { formFieldHasRequiredValidator } from 'src/app/utils/utils.functions';
import { Subscription } from 'rxjs';

@Component({
  selector: 'pre-offer-history',
  templateUrl: './pre-offer-history.component.html',
  styleUrls: ['./pre-offer-history.component.scss']
})
// tslint:disable-next-line:component-class-suffix
export class PreOfferHistory implements OnInit, OnDestroy {
  @Input()
  private _processId: number;
  public get processId(): number {
    return this._processId;
  }
  public set processId(value: number) {
    this._processId = value;
  }
  @Input() preOfferStatus:number;

  editForm: FormGroup = this.fb.group({
    preOfferDate: [new Date()],
    salary: [null, [Validators.required, Validators.min(1)]],
    vacationDays: [null, [Validators.required, Validators.min(0)]],
    healthInsurance: [0, [Validators.required, Validators.min(1)]],
    notes: [null],
    bonus: [null],
    tentativeStartDate: [new Date()], //Check validator
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
  preOfferOperations: { operation: string; data: PreOffer }[] = [];
  processSaveSubscription: Subscription;
  offerCounter:number = 0;
  tomorrow = new Date();
  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {
    this.preOfferStatusList = globals.preOfferStatusList;
    this.healthInsuranceList = globals.healthInsuranceList;
  }

  ngOnInit() {
    this.getPreOffers();
    this.tomorrow.setDate(this.tomorrow.getDate() + 1);
    this.editForm.get('tentativeStartDate').setValue(this.tomorrow);
    this.processSaveSubscription = this.facade.processService.currentId.subscribe(res => this.savePreOfferInDatabase(res));
  }

  getPreOffers() {
    this.facade.preOfferService.get()
      .subscribe(res => {
        if(res.length > 0){
          this.offerCounter = res.reduce((ac, cu) => ac.id > cu.id ? ac : cu).id + 1;
        } else{
          this.offerCounter = 1;
        }
        this.preOffers = res.filter(x => x.processId === this.processId);
        this.validateAddPreOffers();
        this.getHealthInsuranceNames();
      },
        err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        });
  }

  getHealthInsuranceNames() {
    this.preOffers.forEach((preOffer, index) => {
      preOffer.healthInsuranceText = HealthInsuranceEnum[preOffer.healthInsurance];
    });
  }

  showModal(modalContent: TemplateRef<{}>) {
    const modal = this.facade.modalService.create({
      nzTitle: 'Pre-Offer history',
      nzContent: modalContent,
      nzClosable: false,
      nzWrapClassName: 'vertical-center-modal modal-custom',
      nzWidth: '60%',
      nzFooter: null
    });
  }

  disabledDate(current: Date) {
    const tomorrow = new Date();
    tomorrow.setHours(0, 0, 0, 0);
    tomorrow.setDate(tomorrow.getDate() + 1);
    current.setHours(0, 0, 0, 0);
    if(current < tomorrow){
      return true;
    } else{
      return false;
    }
  }

  private validateEditForm() {
    for (const i in this.editForm.controls) {
      if (this.editForm.controls.hasOwnProperty(i)) {
        this.editForm.controls[i].markAsDirty();
        this.editForm.controls[i].updateValueAndValidity();
      }
    }
  }

  addPreOffer() {
    this.validateEditForm();
    if(this.editForm.valid) {
      const currentPreOffer = new PreOffer();
      currentPreOffer.id = this.offerCounter;
      currentPreOffer.preOfferDate = this.editForm.controls['preOfferDate'].value;
      currentPreOffer.salary = this.editForm.controls['salary'].value;
      currentPreOffer.status = preOfferStatusEnum.PendingReply;
      currentPreOffer.healthInsurance = this.editForm.controls['healthInsurance'].value;
      currentPreOffer.notes = this.editForm.controls['notes'].value;
      currentPreOffer.vacationDays = this.editForm.controls['vacationDays'].value;
      if(this.editForm.controls['bonus'].value === null) {
        currentPreOffer.bonus = 0;
      } else {
        currentPreOffer.bonus = this.editForm.controls['bonus'].value;
      }
      currentPreOffer.tentativeStartDate = this.editForm.controls['tentativeStartDate'].value;
      this.preOffers = [...this.preOffers, currentPreOffer];
      this.preOfferOperations.push({
        operation: 'add',
        data: currentPreOffer
      });
      this.offerCounter = this.offerCounter + 1;
      this.resetEditForm();
      this.validateAddPreOffers();
      this.getHealthInsuranceNames();

    }

  }
  savePreOfferInDatabase(processId: number){
    this.preOfferOperations.forEach(preOfferOperation => {
      preOfferOperation.data.processId = processId;
      switch (preOfferOperation.operation) {
        case 'add':
          this.facade.preOfferService.add(preOfferOperation.data)
          .subscribe(() =>{
          }, err => {
            this.facade.errorHandlerService.showErrorMessage(err, 'An error ocurred while adding pre offer');
          });
          break;
        case 'edit':
          this.facade.preOfferService.update(preOfferOperation.data.id, preOfferOperation.data)
          .subscribe(() => {
          }, err =>{
            this.facade.errorHandlerService.showErrorMessage(err, 'An error ocurred while adding pre offer');
          });
          break;
        case 'delete':
          this.facade.preOfferService.delete(preOfferOperation.data.id)
          .subscribe(() => {
          }, err => {
            this.facade.errorHandlerService.showErrorMessage(err, 'An error ocurred while adding pre offer');
          });
          break;
        default:
          break;
      }
    });
  }
  editStatus(status: string, id: number): void {
    const newStatus = status === 'Declined' ? preOfferStatusEnum.Declined : preOfferStatusEnum.Accepted;
    let message;
    if (newStatus === preOfferStatusEnum.Declined) {
      message = 'Do you want to declined the pre offer?';
    } else {
      message = 'Do you want to accept the pre offer?';
    }
    this.facade.modalService.confirm({
      nzTitle: message,
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzWidth: '50%',
      nzOnOk: () => {
        let editedPreOfferOp = this.preOfferOperations.find(preOfferOperation => preOfferOperation.data.id === id);
        const editedPreOffer = this.preOffers.find(preOffer => preOffer.id === id);
        if (editedPreOffer) {
          editedPreOffer.status = newStatus;
        }
        if (editedPreOfferOp) {
          this.preOfferOperations.forEach(preOfferOp => {
            if (preOfferOp.data.id === editedPreOfferOp.data.id) {
              preOfferOp.data.status = newStatus;
            }
          })
        } else{
          editedPreOfferOp = {operation: 'edit', data: this.preOffers.find(preOfferOp => preOfferOp.id === id)};
          editedPreOfferOp.data.status = newStatus;
          this.preOfferOperations.push(editedPreOfferOp);
        }
        this.validateAddPreOffers();
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
      nzOnOk: () => {

        const preOfferOp = this.preOfferOperations.find(po => po.data.id === id);
        if (preOfferOp) {
          this.preOfferOperations = this.preOfferOperations.filter(preOfferOperation => preOfferOperation.data.id !== id);
        } else {
          const deletedPreOffer = this.preOffers.find(preOffer => preOffer.id === id);
          this.preOfferOperations.push({
            operation: 'delete',
            data: deletedPreOffer
          });
        }
        this.preOffers = this.preOffers.filter(preOffer => preOffer.id !== id );
        this.validateAddPreOffers();
      }
    });
  }

  resetEditForm() {

    this.editForm.reset();
    this.editForm.controls['healthInsurance'].setValue(0);
    this.editForm.controls['preOfferDate'].setValue(new Date());
    this.editForm.controls['tentativeStartDate'].setValue(this.tomorrow);
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
      let isLastPreOfferDeclined = this.preOffers[this.preOffers.length - 1].status === preOfferStatusEnum.Declined;
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

  ngOnDestroy() {
    if (this.processSaveSubscription) {
      this.processSaveSubscription.unsubscribe();
    }

  }

}
