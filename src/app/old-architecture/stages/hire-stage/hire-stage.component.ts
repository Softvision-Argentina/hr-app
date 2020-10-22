import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Process } from '@shared/models/process.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { Globals } from '@shared/utils/globals';

@Component({
  selector: 'hire-stage',
  templateUrl: './hire-stage.component.html',
  styleUrls: ['./hire-stage.component.scss']
})
export class HireStageComponent implements OnInit {

  @Input()
    private _process: Process;
    public get process(): Process {
        return this._process;
    }
    public set process(value: Process) {
        this._process = value;
    }

    @Input()
    private _users: User[];
    public get users(): User[] {
        return this._users.sort((a,b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
    }
    public set users(value: User[]) {
        this._users = value;
    }

  hireForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    userOwnerId: [null, [Validators.required]],
    userDelegateId: [null],
    feedback: [null, [Validators.maxLength(10000)]]
  });

  feedbackContent:string = "";

  statusList: any[];

  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {
    this.statusList = globals.stageStatusList;
   }

  ngOnInit() {
    this.changeFormStatus(false);
  }

  getFormControl(name: string): AbstractControl {
    return this.hireForm.controls[name];
  }

  getFeedbackContent(content: string): void {
    this.feedbackContent = content;
  }

  changeFormStatus(enable: boolean) {
    for (const i in this.hireForm.controls) {
      if (this.hireForm.controls[i] !== this.hireForm.controls['status']) {
        if (enable) { this.hireForm.controls[i].enable(); } else { this.hireForm.controls[i].disable(); }
      }
    }
  }

  statusChanged() {
    if (this.hireForm.controls['status'].value === 1) {
       this.changeFormStatus(true);
       this.hireForm.markAsTouched();
    } else {
       this.changeFormStatus(false);
    }
  }

  getFormData(process: Process): Process {
    // this method will return specific fields from the stage

    return process;
  }
}
