import { Component, OnInit } from '@angular/core';
import { FacadeService } from '@shared/services/facade.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  constructor(private facade: FacadeService) { }

  ngOnInit() {
    this.facade.appService.startLoading();
    this.facade.appService.removeBgImage();
    this.facade.appService.stopLoading();
  }
}

