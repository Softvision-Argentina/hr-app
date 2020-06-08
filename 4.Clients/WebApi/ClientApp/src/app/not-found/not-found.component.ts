import { FacadeService } from './../services/facade.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss'],
})
export class NotFoundComponent implements OnInit {

  constructor(private facade: FacadeService) { }

  ngOnInit() {
    this.facade.appService.showBgImage();
  }

}
