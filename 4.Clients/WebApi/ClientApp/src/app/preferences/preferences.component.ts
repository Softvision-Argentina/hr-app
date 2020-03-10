import { Component, OnInit } from '@angular/core';
import { Preference } from 'src/entities/preference';
import { FacadeService } from '../services/facade.service';
import { User } from 'src/entities/user';
import { Dashboard } from 'src/entities/dashboard';
import { UserDashboard } from 'src/entities/userDashboard';

@Component({
  selector: 'app-preferences',
  templateUrl: './preferences.component.html',
  styleUrls: ['./preferences.component.css']
})
export class PreferencesComponent implements OnInit {
  dashboards: Dashboard[] = [];
  currentUser: User = new User();
  dashboardStatus: boolean[] = new Array()

  constructor(private facade: FacadeService) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));    
  }

  ngOnInit() {
    this.getDashboards();
  }

  updatePreferences(dashboard: Dashboard, addOrDelete: boolean) {   
    if(addOrDelete) {
      var userToAdd: UserDashboard = new UserDashboard();
      userToAdd.userId = this.currentUser.ID;
      dashboard.userDashboards.push(userToAdd);
    }
    else {
      var indexUserToDelete: number;
      indexUserToDelete = dashboard.userDashboards.findIndex(x => x.userId === this.currentUser.ID);
      dashboard.userDashboards.splice(indexUserToDelete);
    }

    this.facade.dashboardService
      .update(dashboard.id, dashboard)
      .subscribe(
        error => {
          console.log(error);
        }
      );
  }

  getDashboards() {
    this.facade.dashboardService.get().subscribe(
      res => {
        res.forEach(dash => {
          this.dashboards.push(dash);
        })
        this.fillStatus();
      },
      error => {
        console.log(error);
      }
    );
  }

  fillStatus() {
    for(let counter = 0; counter < this.dashboards.length; counter++){
      if( this.dashboards[counter].userDashboards.some( x => x.userId === this.currentUser.ID)){
        this.dashboardStatus.push(true);
      }
      else{
        this.dashboardStatus.push(false);
      }
    }
  }

}
