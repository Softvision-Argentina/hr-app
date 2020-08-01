import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { CandidateService } from '../services/candidate.service';
import { map } from 'rxjs/operators';

export function UniqueEmailValidator(candidateService: CandidateService): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    if (control.value !== null && control.value !== '') {
      return candidateService.exists(control.value).pipe(
        map(result => result.body.exists ? { emailExists: true } : null)
      );
    } else {
      return new Observable(null);
    }
  };
}

export function checkIfEmailAndPhoneNulll(c: AbstractControl): ValidationErrors | null {
  if ((c.get('email').value === null || c.get('email').value.length === 0)
    && (c.get('phoneNumber').value === null || c.get('phoneNumber').value.length === 0)) {
    c.get('email').setErrors({ emailAndPhoneValidator: true });
    c.get('phoneNumber').setErrors({ emailAndPhoneValidator: true });
    return {
      emailAndPhoneValidator: true
    };
  };

  return null;
}