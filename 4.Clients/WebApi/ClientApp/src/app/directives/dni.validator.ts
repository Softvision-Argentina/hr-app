import { ValidatorFn, FormControl, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { Candidate } from 'src/entities/candidate';
import { map } from 'rxjs/operators';
export const dniValidator: ValidatorFn = (control: FormControl) =>{
    if (control.value!=null) {
        if (control.value>99999999) {
            return {
                'dniTooLongError': true
            };
        }
        if (control.value<0) {
            return {
                'dniTooShortError': true
            };
        }
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