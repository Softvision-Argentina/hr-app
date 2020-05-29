import { ValidatorFn, FormControl, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { Candidate } from 'src/entities/candidate';
import { map } from 'rxjs/operators';

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

export function UniqueDniValidator(CandidateObservable: Observable<Candidate[]>): AsyncValidatorFn {
    return({value}: AbstractControl): Observable<ValidationErrors | null> => {
        return CandidateObservable.pipe(map(candidates => {
            if (candidates.find(candidate => candidate.dni === value && value !== 0)) {
                return {
                    dniExists: true
                };
            }
            return null;
        }));
    };
}