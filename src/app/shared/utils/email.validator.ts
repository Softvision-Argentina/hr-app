import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { CandidateService } from '../services/candidate.service';
import { map } from 'rxjs/operators';

export function UniqueEmailValidator(candidateService: CandidateService): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return candidateService.exists(control.value).pipe(       
        map(result => result.body.exists ? {emailExists: true} : null)
      );
    };
}