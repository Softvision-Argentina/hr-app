import { Component, OnInit } from '@angular/core';
import { Preference } from 'src/entities/preference';
import { FacadeService } from '../services/facade.service';
import { User } from 'src/entities/user';

@Component({
  selector: 'app-preferences',
  templateUrl: './preferences.component.html',
  styleUrls: ['./preferences.component.css']
})
export class PreferencesComponent implements OnInit {
  preference: Preference = new Preference();

  constructor(private facade: FacadeService) {}

  ngOnInit() {
    this.getPreferences();   
  }

  updatePreferences() {
    this.facade.preferenceService
      .update(this.preference.id, this.preference)
      .subscribe(
        error => {
          console.log(error);
        }
      );
      this.facade.preferenceService.changePreference(this.preference);
  }

  getPreferences() {
    const currentUser: User = JSON.parse(localStorage.getItem('currentUser'));

    this.facade.preferenceService.get().subscribe(
      res => {
        this.preference = res.filter(x => x.userId === currentUser.ID)[0];
      },
      error => {
        console.log(error);
      }
    );
  }
}
