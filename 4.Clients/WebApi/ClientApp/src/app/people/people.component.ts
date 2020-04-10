import { Component, OnInit } from '@angular/core';
import { AppComponent } from '../app.component';
import { GoogleSigninComponent } from '../login/google-signin.component';
import { User } from 'src/entities/user';

@Component({
  selector: 'app-people',
  templateUrl: './people.component.html',
  styleUrls: ['./people.component.css']
})
export class PeopleComponent implements OnInit {

  currentUser: User;

  constructor(private app: AppComponent, private google: GoogleSigninComponent) { }

  ngOnInit() {
    this.app.removeBgImage();
  }

  isUserAuthenticated() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    return this.google.isUserAuthenticated();
  }


  isUserRole(roles: string[]): boolean {
    return this.app.isUserRole(roles);
  }
}
