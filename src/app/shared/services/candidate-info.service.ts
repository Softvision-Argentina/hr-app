import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { AppConfig } from '@shared/utils/app.config';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, Subject } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Candidate } from '@shared/models/candidate.model';
import { ICandidate } from '../interfaces/ICandidate.service';

@Injectable()
export class CandidateInfoService{
  emptyReferredInfo: Candidate = null;

  public _candidateInfoSource = new BehaviorSubject<Candidate>(this.emptyReferredInfo);
  _candidateInfo$ = this._candidateInfoSource.asObservable();

  public sendCandidateInfo(info: Candidate) {
    this._candidateInfoSource.next(info);
  }

}
