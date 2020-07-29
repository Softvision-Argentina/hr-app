import { Component, OnInit } from '@angular/core';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { GoogleSigninComponent } from '../login/google-signin.component';

@Component({
  selector: 'app-people',
  templateUrl: './people.component.html',
  styleUrls: ['./people.component.scss']
})
export class PeopleComponent implements OnInit {

  currentUser: User;

  constructor(private facade: FacadeService, private google: GoogleSigninComponent) { }

  ngOnInit() {
    this.facade.appService.removeBgImage();
  }

  isUserAuthenticated() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    return this.google.isUserAuthenticated();
  }

  isUserRole(roles: string[]): boolean {
    return this.facade.appService.isUserRole(roles);
  }
}
