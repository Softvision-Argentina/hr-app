import { FacadeService } from 'src/app/services/facade.service';
import { Component, OnInit } from '@angular/core';
import { GoogleSigninComponent } from './google-signin.component';
import { Router, ActivatedRoute } from '@angular/router';
import { CSoftComponent } from '../login/csoft-signin.component';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [GoogleSigninComponent, CSoftComponent]
})
export class LoginComponent implements OnInit {

  returnUrl = '';

  constructor(
    private google: GoogleSigninComponent,
    private csSoft: CSoftComponent,
    private route: ActivatedRoute,
    private router: Router,
    private facade: FacadeService
  ) {

  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    if (this.google.isUserAuthenticated() || this.csSoft.isUserAuthenticated()) {
      this.router.navigateByUrl(this.returnUrl);
    } else {
      this.facade.appService.stopLoading();
      this.facade.appService.showBgImage();
    }
  }

}
