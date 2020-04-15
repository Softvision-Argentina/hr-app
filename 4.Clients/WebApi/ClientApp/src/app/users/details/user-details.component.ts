import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';

@Component({
  selector: 'user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {

  @Input()
  private _detailedUser: User;
  public get detailedUser(): User {
    return this._detailedUser;
  }
  public set detailedUser(value: User) {
    this._detailedUser = value;
  }

  constructor(private facade: FacadeService) { }

  ngOnInit() {
  }

  showModal(modalContent: TemplateRef <{}>, fullName: string){
    this.facade.modalService.create({
        nzTitle: fullName + "'s details",
        nzContent: modalContent,
        nzClosable: true,
        nzWrapClassName: 'vertical-center-modal',
        nzFooter: null
    });
}

}
