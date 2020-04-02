import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, AbstractControl, Validators } from '@angular/forms';
import { Consultant } from 'src/entities/consultant';
import { FacadeService } from 'src/app/services/facade.service';
import { trimValidator } from 'src/app/directives/trim.validator';
import { Process } from 'src/entities/process';
import { Globals } from 'src/app/app-globals/globals';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';

@Component({

  selector: 'app-hire-stage',
  templateUrl: './hire-stage.component.html',
  styleUrls: ['./hire-stage.component.css']
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
    private _consultants: Consultant[];
    public get consultants(): Consultant[] {
        return this._consultants;
    }
    public set consultants(value: Consultant[]) {
        this._consultants = value;
    }

  hireForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    consultantOwnerId: [null, [Validators.required]],
    consultantDelegateId: [null],
    feedback: [null, [trimValidator]]
  });

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
