import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { ICandidate } from '../interfaces/ICandidate.service';

export function UniqueEmailValidator(service: ICandidate, id = 0): AsyncValidatorFn {
  let referrals = service.data.value;
  if (id && referrals) {
    referrals = service.data.value.filter(referral => referral.id !== id)
  }
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    if (control.value !== null && control.value !== '') {
      const emailExists = referrals?.some(referral => referral.emailAddress === control.value);
      if (emailExists) {
        return of({ emailExists: true });
      } else {
        return service.exists(control.value, id)
          .pipe(
            map(result => result.body.exists ?
              { emailExists: true } : null)
          );
      }
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