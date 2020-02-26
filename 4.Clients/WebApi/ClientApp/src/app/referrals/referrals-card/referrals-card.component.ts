import { Component, OnInit, Input } from '@angular/core';
import { Candidate } from 'src/entities/candidate';

@Component({
  selector: 'app-referrals-card',
  templateUrl: './referrals-card.component.html',
  styleUrls: ['./referrals-card.component.css']
})
export class ReferralsCardComponent implements OnInit {
@Input() cand: Candidate;

  constructor() { }

  ngOnInit() {
  }
}
