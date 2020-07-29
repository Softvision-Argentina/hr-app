import { Component, OnInit } from '@angular/core';
import { FacadeService } from '@shared/services/facade.service';

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
