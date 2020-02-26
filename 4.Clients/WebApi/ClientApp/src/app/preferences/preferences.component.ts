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
  preference: Preference;

  constructor(private facade: FacadeService) {}

  ngOnInit() {
    this.getPreferences();
  }

  updatePreferences() {
    this.facade.preferenceService
      .update<Preference>(this.preference.id, this.preference)
      .subscribe(
        res => {
          console.log(res);
        },
        error => {
          console.log(error);
        }
      );
  }

  getPreferences() {
    const currentUser: User = JSON.parse(localStorage.getItem('currentUser'));

    this.facade.preferenceService.get<Preference>().subscribe(
      res => {
        this.preference = res.filter(x => x.userId === currentUser.ID)[0];
      },
      error => {
        console.log(error);
      }
    );
  }
}
