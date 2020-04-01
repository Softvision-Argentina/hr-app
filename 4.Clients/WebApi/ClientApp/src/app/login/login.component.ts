import { Component, OnInit } from '@angular/core';
import { GoogleSigninComponent } from './google-signin.component';
import { Router, ActivatedRoute } from '@angular/router';
import { AppComponent } from '../app.component';
import { CSoftComponent } from '../login/csoft-signin.component';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [GoogleSigninComponent, AppComponent, CSoftComponent]
})
export class LoginComponent implements OnInit {

  returnUrl = '';

  constructor(
    private google: GoogleSigninComponent,
    private csSoft: CSoftComponent,
    private route: ActivatedRoute,
    private router: Router, private app: AppComponent
  ) {

  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    if (this.google.isUserAuthenticated() || this.csSoft.isUserAuthenticated()) {
      this.router.navigateByUrl(this.returnUrl);
    } else {
      this.app.renderBgImage();
    }
  }

}
