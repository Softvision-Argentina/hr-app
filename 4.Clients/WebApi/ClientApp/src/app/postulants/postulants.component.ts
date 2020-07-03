import { Component, OnInit } from '@angular/core';
import { FacadeService } from '../services/facade.service';
import { Postulant } from 'src/entities/postulant';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-postulants',
  templateUrl: './postulants.component.html',
  styleUrls: ['./postulants.component.scss']
})
export class PostulantsComponent implements OnInit {

  listOfDisplayData: Postulant [] = [];
  constructor(private facade: FacadeService, private fb: FormBuilder) { }

  ngOnInit() {
    this.facade.appService.startLoading();
    this.facade.appService.removeBgImage();
    this.getPostulants();
  }

  getPostulants(){
    this.facade.postulantService.get().subscribe(res => {
      this.listOfDisplayData = res;
      this.facade.appService.stopLoading();
    }, err => {
      console.log(err);
    });
  }
}
