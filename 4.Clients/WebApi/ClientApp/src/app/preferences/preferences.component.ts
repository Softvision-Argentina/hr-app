import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-preferences',
  templateUrl: './preferences.component.html',
  styleUrls: ['./preferences.component.css']
})
export class PreferencesComponent implements OnInit {
  switchProcesses = true;
  switchSkills = true;
  switchProgress = true;
  switchCompleted = true;
  switchProjection = true;
  switchCasualties = true;
  switchTimeToFill1 = true;
  switchTimeToFill2 = true;
  
  constructor() { }

  ngOnInit() {
  }

}
