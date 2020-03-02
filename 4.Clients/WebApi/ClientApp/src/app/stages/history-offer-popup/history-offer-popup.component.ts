import { Component, OnInit, TemplateRef, Input } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { Offer } from 'src/entities/offer';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { OfferStatusEnum } from 'src/entities/enums/offer-status.enum';
import { Globals } from 'src/app/app-globals/globals';


@Component({
  selector: 'history-offer-popup',
  templateUrl: './history-offer-popup.component.html',
  styleUrls: ['./history-offer-popup.component.css']
})
export class HistoryOfferPopupComponent implements OnInit {
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
  ofertas : Offer[];
  listOfDisplayData : Offer[];
  sortValue = null;
  sortName = null;
  allowAddOffer : boolean;
    
  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {    
    this.offerStatusList = globals.offerStatusList;    
  }

  ngOnInit() {
    this.getOffers();        
  }

  getOffers(){
    this.facade.offerService.get<Offer>()
      .subscribe(res => {
        this.ofertas = res.filter(x=> x.processId == this.processId);    
        this.listOfDisplayData = res.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
      }, err => {
        console.log(err);
      });
  }
  
  showModal(modalContent: TemplateRef <{}>){    
    this.facade.modalService.create({
        nzTitle: "Offer history",
        nzContent: modalContent,
        nzClosable: true,
        nzWrapClassName: 'vertical-center-modal',
        nzFooter: null ,     
        nzWidth : 1000
    })
  }

  doSomething(){        
    console.log(this.ofertas);
    console.log(!(this.ofertas[this.ofertas.length-1].status == OfferStatusEnum.Declined));
  }

  doSomething2(){        
    let toAdd : Offer = new Offer();  
    toAdd.id = 0;
    toAdd.offerDate = new Date();
    toAdd.salary = 123;
    toAdd.rejectionReason = "pinto";
    toAdd.status = OfferStatusEnum.Pending;
    toAdd.processId = this.processId;
     this.facade.offerService.add<Offer>(toAdd)
      .subscribe(res => {
        this.getOffers();        
        this.facade.toastrService.success("Offer was successfuly created !");        
      }, err => {        
        if(err.message != undefined) this.facade.toastrService.error(err.message);
        else this.facade.toastrService.error("The service is not available now. Try again later.");
    })
  }

  addOffer(modalContent: TemplateRef<{}>){
    this.resetEditForm();     
    const modal = this.facade.modalService.create({
      nzTitle: 'Add Offer',
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
              this.facade.offerService.add<Offer>(toAdd)
                .subscribe(res => {
                  this.getOffers();        
                  this.facade.toastrService.success("Offer was successfuly created !");      
                  this.allowAddOffer = false;                    
                  modal.destroy();
                }, err => {        
                  if(err.message != undefined) this.facade.toastrService.error(err.message);
                  else this.facade.toastrService.error("The service is not available now. Try again later.");
              })                            
            }              
            else modal.nzFooter[1].loading = false;
          }
        }],
    });
  }

  editStatus(status : string, id: number): void {             
    let editedOffer : Offer = this.ofertas.filter(offer => offer.id == id)[0];                             
    if (status == 'Declined'){
      editedOffer.status = OfferStatusEnum.Declined;
      this.allowAddOffer = true;      
    }else{
      editedOffer.status = OfferStatusEnum.Accepted;
      this.allowAddOffer = false;
    }    
    this.facade.offerService.update<Offer>(id, editedOffer)
    .subscribe(res => {
      this.getOffers();                
      this.facade.toastrService.success('Offer was successfully edited !');                
    }, err => {                                
      if (err.message != undefined) this.facade.toastrService.error(err.message);
      else this.facade.toastrService.error('The service is not available now. Try again later.');
    })                                                    
  }

  getStatus(status: number): string {
    return this.offerStatusList.find(st => st.id === status).name;
  }

  showDeleteConfirm(id: number): void {    
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to delete?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',                        
      nzOnOk: () => this.facade.offerService.delete<Offer>(id)
      .subscribe(res => {
        this.getOffers();
        this.facade.toastrService.success('Offer was deleted !');
      }, err => {
        if(err.message != undefined) this.facade.toastrService.error(err.message);
        else this.facade.toastrService.error("The service is not available now. Try again later.");
      })
    });
    this.lastOfferConcluded();
  }

  resetEditForm() { 
    this.editForm = this.fb.group({
      offerDate: [new Date(), [Validators.required]],
      salaryOffer: [null, [Validators.required]],            
    });
  }

  lastOfferConcluded(){
    if (this.ofertas.length == 0){
      this.allowAddOffer = false;
    }else{
      this.allowAddOffer = this.ofertas[this.ofertas.length-1].status == OfferStatusEnum.Declined;
    }    
  }
}
