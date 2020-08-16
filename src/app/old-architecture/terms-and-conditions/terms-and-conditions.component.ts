import { Component, OnInit } from '@angular/core';
import { FacadeService } from '@app/shared/services/facade.service';

@Component({
  selector: 'app-terms-and-conditions',
  templateUrl: './terms-and-conditions.component.html',
  styleUrls: ['./terms-and-conditions.component.scss']
})
export class TermsAndConditionsComponent implements OnInit {

  constructor(private facade: FacadeService) { }

  ngOnInit(): void {
    this.facade.appService.stopLoading();
  }

}
