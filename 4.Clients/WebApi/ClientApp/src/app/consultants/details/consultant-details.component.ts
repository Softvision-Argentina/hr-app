import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { Consultant } from 'src/entities/consultant';
import { FacadeService } from 'src/app/services/facade.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'consultant-details',
  templateUrl: './consultant-details.component.html',
  styleUrls: ['./consultant-details.component.css']
})
export class ConsultantDetailsComponent implements OnInit {

  @Input()
  private _detailedConsultant: Consultant;
  public get detailedConsultant(): Consultant {
    return this._detailedConsultant;
  }
  public set detailedConsultant(value: Consultant) {
    this._detailedConsultant = value;
  }

  constructor(private facade: FacadeService) { }

  ngOnInit() {
  }

  showModal(modalContent: TemplateRef <{}>, fullName: string) {
    this.facade.modalService.create({
        // tslint:disable-next-line: quotemark
        nzTitle: fullName + "'s details",
        nzContent: modalContent,
        nzClosable: true,
        nzWrapClassName: 'vertical-center-modal',
        nzFooter: null
    });
}

}
