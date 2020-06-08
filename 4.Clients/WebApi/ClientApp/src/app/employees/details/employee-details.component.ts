import { Component, OnInit, TemplateRef, Input, ContentChild } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { Employee } from 'src/entities/employee';


@Component({
  selector: 'employee-details',
  templateUrl: './employee-details.component.html',
  styleUrls: ['./employee-details.component.scss'],
})


export class EmployeeDetailsComponent implements OnInit {

  @Input()
  private _detailedEmployee: Employee;
  public get detailedEmployee(): Employee {
    return this._detailedEmployee;
  }
  public set detailedEmployee(value: Employee) {
    this._detailedEmployee = value;
  }

  userName: string;

  constructor(private facade: FacadeService) { }

  ngOnInit() {
    this.getRecruiterName();
  }

  getRecruiterName() {
    this.facade.userService.get()
      .subscribe(res => {
        this.userName = res.filter(r => r.id === this._detailedEmployee.userId)[0].firstName
          + res.filter(r => r.id === this._detailedEmployee.userId)[0].lastName;
      }, err => {
        console.log(err);
      })
  }

  showModal(modalContent: TemplateRef<{}>, fullName: string) {
    let title = fullName + '\'s details';
    this.facade.modalService.create({
      nzTitle: title,
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: null
    });
  }
}
