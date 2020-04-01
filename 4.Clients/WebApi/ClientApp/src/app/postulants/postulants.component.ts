import { Component, OnInit } from '@angular/core';
import { Community } from 'src/entities/community';
import { CandidateProfile } from 'src/entities/Candidate-Profile';
import { ActivatedRoute, Params } from '@angular/router';
import { FacadeService } from '../services/facade.service';
import { AppComponent } from '../app.component';
import { PostulantsService } from '../services/postulants.service';
import { Postulant } from 'src/entities/postulant';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-postulants',
  templateUrl: './postulants.component.html',
  styleUrls: ['./postulants.component.css']
})
export class PostulantsComponent implements OnInit {

  listOfDisplayData: Postulant [] = [];
  constructor(private facade: FacadeService, private fb: FormBuilder, private app: AppComponent) { }

  ngOnInit() {
    this.app.showLoading();
    this.app.removeBgImage();
    this.getPostulants();
  }

  getPostulants() {
    this.facade.postulantService.get().subscribe(res => {
      this.listOfDisplayData = res;
      console.log(res);
    }, err => {
      console.log(err);
    });
  }
}
