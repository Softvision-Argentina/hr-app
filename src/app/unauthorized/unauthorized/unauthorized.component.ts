import { FacadeService } from './../../services/facade.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.scss'],
})
export class UnauthorizedComponent implements OnInit {

  constructor(private facade: FacadeService) { }

  ngOnInit() {
    this.facade.appService.showBgImage();
    this.facade.appService.stopLoading();
  }

}
