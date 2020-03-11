import { Component, OnInit } from '@angular/core';
import { FacadeService } from '../services/facade.service';
import { User } from 'src/entities/user';
import { Dashboard } from 'src/entities/dashboard';
import { UserDashboard } from 'src/entities/userDashboard';
import { variable } from '@angular/compiler/src/output/output_ast';

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
    let variable: Dashboard = new Dashboard();
    if(addOrDelete) {
      var userToAdd: UserDashboard = new UserDashboard();
      userToAdd.userId = this.currentUser.ID;
      userToAdd.dashboardId = dashboard.id;     
      dashboard.userDashboards = dashboard.userDashboards.filter(x => x.userId === this.currentUser.ID);
      dashboard.userDashboards.push(userToAdd);
    }
    else {
      var indexUserToDelete: number;
      indexUserToDelete = dashboard.userDashboards.findIndex(x => x.userId === this.currentUser.ID);
      dashboard.userDashboards.splice(indexUserToDelete, 1);
    }

    this.facade.dashboardService
      .update(dashboard.id, dashboard)
      .subscribe(
        res => {
        },
        error => {
          console.log(error);
        }
      );
      this.facade.dashboardService.changePreference(this.dashboards);
  }

  getDashboards() {
    this.facade.dashboardService.get().subscribe(
      res => {
        res.forEach(dash => {
          this.dashboards.push(dash);
        });
        this.fillStatus();
      },
      error => {
        console.log(error);
      }
    );
  }

  fillStatus() {
    for (let counter = 0; counter < this.dashboards.length; counter++){
      if ( this.dashboards[counter].userDashboards.some( x => x.userId === this.currentUser.ID)) {
        this.dashboardStatus.push(true);
      } else {
        this.dashboardStatus.push(false);
      }
    }
  }

}
