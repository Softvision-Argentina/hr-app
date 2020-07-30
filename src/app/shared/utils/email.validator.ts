import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { CandidateService } from '../services/candidate.service';
import { map } from 'rxjs/operators';

export function UniqueEmailValidator(candidateService: CandidateService): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    if (control.value) {
      return candidateService.exists(control.value).pipe(       
          map(result => result.body.exists ? {emailExists: true} : null)
        );
    } else {
      return new Observable(null);
    }
  };
}