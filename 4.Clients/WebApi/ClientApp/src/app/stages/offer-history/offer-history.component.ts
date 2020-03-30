import { Component, OnInit, TemplateRef, Input, ViewChild, OnDestroy } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { Offer } from 'src/entities/offer';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { OfferStatusEnum } from 'src/entities/enums/offer-status.enum';
import { Globals } from 'src/app/app-globals/globals';


@Component({
  selector: 'offer-history',
  templateUrl: './offer-history.component.html',
  styleUrls: ['./offer-history.component.css']
})
export class OfferHistory implements OnInit {
  @Input() 
  private _processId : number;
  public get processId():  number {
    return this._processId;
  }
  public set processId(value: number) {
      this._processId = value;
  }

  editForm: FormGroup;
  offerStatusList: any[];
  offers : Offer[] = [];

  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {    
    this.offerStatusList = globals.offerStatusList;    
  }

  ngOnInit() {
    this.getOffers();
  }

  getOffers(){
    this.facade.offerService.get()
      .subscribe(res => {
        this.offers = res.filter(x=> x.processId == this.processId);            
      }, err => {
        console.log(err);
      });
  }
  
  showModal(modalContent: TemplateRef <{}>){    
    const modal = this.facade.modalService.create({
        nzTitle: "Offer history",
        nzContent: modalContent,
        nzClosable: false,
        nzWrapClassName: 'vertical-center-modal',
        nzWidth: 1000,
        nzFooter: null,
        nzMaskClosable: false,
        nzKeyboard: false    
    })
  }

  addOffer(modalContent: TemplateRef<{}>){
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
            let isCompleted: boolean = true;
            for (const i in this.editForm.controls) {
              this.editForm.controls[i].markAsDirty();
              this.editForm.controls[i].updateValueAndValidity();
              if (!this.editForm.controls[i].valid) isCompleted = false;
            }
            if (isCompleted) {
              let toAdd : Offer = new Offer();  
              toAdd.id = 0;
              toAdd.offerDate = this.editForm.controls['offerDate'].value;
              toAdd.salary = this.editForm.controls['salaryOffer'].value;
              toAdd.rejectionReason = '';
              toAdd.status = OfferStatusEnum.Pending;
              toAdd.processId = this.processId;
              this.facade.offerService.add(toAdd)
                .subscribe(res => {
                  this.getOffers();
                  this.facade.toastrService.success("Offer was successfuly created !");                                           
                  modal.destroy();
                }, err => {
                  this.facade.errorHandlerService.showErrorMessage(err);
              })                            
            }              
            else modal.nzFooter[1].loading = false;
          }
        }],
    });
  }

  editStatus(status : string, id: number): void {     
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to change the status?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzWidth: '50%',
      nzOnOk: () => {
        let editedOffer : Offer = this.offers.filter(offer => offer.id == id)[0];                             
        if (status == 'Declined'){
          editedOffer.status = OfferStatusEnum.Declined;      
        }else{
          editedOffer.status = OfferStatusEnum.Accepted;      
        }    
        this.facade.offerService.update(id, editedOffer)
        .subscribe(res => {
          this.getOffers();                
          this.facade.toastrService.success('Offer was successfully edited !');                
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
      }
    });                                                
  }

  editRejectionReason(id: number, reason :string){
    let editedOffer : Offer = this.offers.filter(offer => offer.id == id)[0];                             
    editedOffer.rejectionReason = reason;
    this.facade.offerService.update(id, editedOffer)
    .subscribe(res => {
      this.getOffers();                
      this.facade.toastrService.success('Offer was successfully edited !');                
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    })   
  }

  getStatusName(status: number): string {
    return this.offerStatusList.find(st => st.id === status).name;
  }

  showDeleteConfirm(id: number): void {    
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to delete?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',  
      nzWidth: '50%',                      
      nzOnOk: () => this.facade.offerService.delete(id)
      .subscribe(res => {
        this.getOffers();
        this.facade.toastrService.success('Offer was deleted !');
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      })
    });    
  }

  resetEditForm() { 
    this.editForm = this.fb.group({
      offerDate: [new Date(), [Validators.required]],
      salaryOffer: [null, [Validators.required]],            
    });
  }

  offerConcluded(): boolean{
    if (this.offers.length == 0){
      return false;
    }else{
      return !(this.offers[this.offers.length-1].status == OfferStatusEnum.Declined);
    }    
  }

  getStatusColor(status: number): string {
    let statusName = this.getStatusName(status);
    switch (statusName) {            
      case 'Declined': return 'error';
      case 'Accepted': return 'success';
      case 'Pending': return 'processing';
    }
  }

  validateReasons(){  
    if (this.offers.length == 0){
      this.facade.modalService.openModals[1].destroy();
    }else if((this.offers.filter(x=> x.rejectionReason == '' && x.status == OfferStatusEnum.Declined)).length == 0){
      this.facade.modalService.openModals[1].destroy();
    }        
  }
}
