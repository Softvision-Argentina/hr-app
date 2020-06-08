import { ValidatorFn, FormControl, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { Candidate } from 'src/entities/candidate';
import { Process } from 'src/entities/process';

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

export function UniqueDniValidator(candidates: Candidate[], processId: number): AsyncValidatorFn {
    return({value}: AbstractControl): Observable<ValidationErrors | null> => {
        //const candidateUsed = candidates.find(candidate => candidate.dni === value && value && value !== 0 && candidate.id != processId);
        const candidateUsed = false;
        return new Observable(subscriber => {
            if (!!candidateUsed) {
                subscriber.next({dniExists: true});
            } else {
                subscriber.next(null);
            }
            subscriber.complete();
        });
    }
}