import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, AbstractControl, Validators, FormControl } from '@angular/forms';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';
import { trimValidator } from 'src/app/directives/trim.validator';
import { Process } from 'src/entities/process';
import { Globals } from 'src/app/app-globals/globals';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';

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
    feedback: [null, [trimValidator]]
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
