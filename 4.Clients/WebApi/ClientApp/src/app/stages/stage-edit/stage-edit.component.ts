import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Validators, FormBuilder, FormGroup, NgForm, FormArray } from '@angular/forms';
import { Stage } from 'src/entities/stage';
import { Consultant } from 'src/entities/consultant';
import { FacadeService } from 'src/app/services/facade.service';

@Component({
  selector: 'app-stage-edit',
  templateUrl: './stage-edit.component.html',
  styleUrls: ['./stage-edit.component.css']
})
export class StageEditComponent implements OnInit {

  isLoading = false;
  consultants: Consultant[] = [];
  stageForm: FormGroup;
  consultantForm: FormGroup;
  stage: Stage;
  consultantOwner: Consultant;
  consultantDelegate: Consultant;
  selected = 0;

  constructor(private facade: FacadeService, private route: ActivatedRoute, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.stageForm = this.formBuilder.group({
      title: [null, Validators.required],
      description: [null, Validators.required],
      startDate: [null, Validators.required],
      endDate: [null, Validators.required],
      status: [null, Validators.required],
      stageItems: this.formBuilder.array([])
    });

    this.consultantForm = this.formBuilder.group({
      consultantName: null
    });

    this.getStageByID(this.route.snapshot.params['id']);
  }

  displayFn(consultant: Consultant) {
    if (consultant) { return consultant.name; }
  }

  getStageByID(id) {
    this.facade.stageService.getByID(id)
      .subscribe(data => {
        this.stage = data;
        this.consultantOwner = this.consultants.find(c => c.id === data.consultantOwnerId);
        this.consultantDelegate = this.consultants.find(c => c.id === data.consultantDelegateId);

        this.stageForm.setValue({
          type: '',
          date: new Date,
          status: data.status,
          stageItems: []
        });
      });
  }

  getAllConsultants() {
    this.facade.consultantService.get()
      .subscribe(res => {
        this.consultants = res;
        console.log(this.consultants);
      }, err => {
        console.log(err);
      });
  }

  addNewItem() {
    const control = <FormArray>this.stageForm.controls.stageItems;
    control.push(
      this.formBuilder.group({
        itemId: [this.selected], itemDescription: [''], associatedContent: ['']
      })
    );
  }

  deleteItem(index) {
    const control = <FormArray>this.stageForm.controls.stageItems;
    control.removeAt(index);
  }
}
