import { ValidatorFn, FormControl, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { Candidate } from 'src/entities/candidate';
import { delay } from 'rxjs/operators';

export const dniValidator: ValidatorFn = (control: FormControl): { [key: string]: any } | null => {
    let shortNumber = /^\d{1,7}$/,
        firstDigit0 = /^[0]{1}/;

    if (control.value == null) {
        return {
            'emptyInputError': true
        };
    }
    if (firstDigit0.test(control.value)) {
        return {
            'dniFirstDigitError': true
        };
    }
    if (shortNumber.test(control.value)) {
        return {
            'dniTooShortError': true
        };
    }

    return null;
};

export function UniqueDniValidator(candidates: Candidate[]): AsyncValidatorFn {

    return({value}: AbstractControl): Observable<ValidationErrors | null> => {
        const dniUsed = candidates.find(candidate => candidate.dni === value && value && value !== 0);
        return new Observable(subscriber => {
            if (dniUsed) {
                subscriber.next({dniExists: true});
            } else {
                subscriber.next(null);
            }
            subscriber.complete();
        });
    }
}