import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Validators, FormBuilder, FormGroup, NgForm, FormArray } from '@angular/forms';
import { Stage } from 'src/entities/stage';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';

@Component({
  selector: 'app-stage-edit',
  templateUrl: './stage-edit.component.html',
  styleUrls: ['./stage-edit.component.css']
})
export class StageEditComponent implements OnInit {

  isLoading = false;
  users: User[] = [];
  stageForm: FormGroup;
  userForm: FormGroup;
  stage: Stage;
  userOwner: User;
  userDelegate: User;
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

    this.userForm = this.formBuilder.group({
      userName: null
    });

    this.getStageByID(this.route.snapshot.params['id']);
  }

  displayFn(user: User) {
    if (user) { return user.name; }
  }

  getStageByID(id) {
    this.facade.stageService.getByID(id)
      .subscribe(data => {
        this.stage = data;
        this.userOwner = this.users.find(c => c.id === data.userOwnerId);
        this.userDelegate = this.users.find(c => c.id === data.userDelegateId);

        this.stageForm.setValue({
          type: '',
          date: new Date,
          status: data.status,
          stageItems: []
        });
      });
  }

  getAllUsers() {
    this.facade.userService.get()
      .subscribe(res => {
        this.users = res;
        console.log(this.users);
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
