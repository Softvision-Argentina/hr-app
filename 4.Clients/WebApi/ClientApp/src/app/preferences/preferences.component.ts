import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-preferences',
  templateUrl: './preferences.component.html',
  styleUrls: ['./preferences.component.css']
})
export class PreferencesComponent implements OnInit {
  switchProcesses = false;
  switchSkills = false;
  switchProgress = false;
  switchCompleted = false;
  switchProjection = false;
  switchCasualties = false;
  switchTimeToFill1 = false;
  switchTimeToFill2 = false;
  
  constructor() { }

  ngOnInit() {
  }

}
