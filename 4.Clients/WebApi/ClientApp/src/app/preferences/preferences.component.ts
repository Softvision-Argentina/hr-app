import { Component, OnInit } from '@angular/core';
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
  currentUser: User = new User(null,null,null);
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
      userToAdd.userId = this.currentUser.id;
      userToAdd.dashboardId = dashboard.id;    
      dashboard.userDashboards.push(userToAdd);
    }
    else {
      var indexUserToDelete: number;
      indexUserToDelete = dashboard.userDashboards.findIndex(x => x.userId === this.currentUser.id);
      dashboard.userDashboards.splice(indexUserToDelete, 1);
    }

    this.facade.dashboardService
      .update(dashboard.id, dashboard)
      .subscribe(
        res => {
          this.facade.dashboardService.changePreference(this.dashboards);
          this.facade.toastrService.success('Preferences were successfully edited !');
        },
        error => {
          this.facade.toastrService.error('The service is not available now. Try again later.');
        }
      );
      
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
      if ( this.dashboards[counter].userDashboards.some( x => x.userId === this.currentUser.id)) {
        this.dashboardStatus.push(true);
      } else {
        this.dashboardStatus.push(false);
      }
    }
  }

}
