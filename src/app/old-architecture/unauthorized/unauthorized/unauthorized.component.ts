import { Component, OnInit } from '@angular/core';
import { FacadeService } from '@shared/services/facade.service';

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
