import { FormGroup, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { Candidate } from 'src/entities/candidate';
import { map } from 'rxjs/operators';
export function validateCandidateForm(form: FormGroup) {
    let isValid = true;
    if(form.get('isReferred')) {
        if (form.controls['isReferred'].value === true && form.controls['referredBy'].invalid) {
            isValid = false;
        }
    }
    if (form.get('phoneNumberPrefix')) {
        // tslint:disable-next-line: forin
        for (const i in form.controls) {
            form.controls[i].markAsDirty();
            form.controls[i].updateValueAndValidity();
            if ((!form.controls[i].valid) && (form.controls[i] !== form.controls['phoneNumberPrefix'])) {
                isValid = false;
                break;
            }
        }
    } else {
        // tslint:disable-next-line: forin
        for (const i in form.controls){
            form.controls[i].markAsDirty();
            form.controls[i].updateValueAndValidity();
            isValid = false;
            break;
        }
    }
    return isValid;
}
export function UniqueEmailValidator(CandidateObservable: Observable<Candidate[]>): AsyncValidatorFn {
    return({value}: AbstractControl): Observable<ValidationErrors | null> => {
        return CandidateObservable.pipe(map(candidates => {
            if (candidates.find(candidate => candidate.emailAddress === value)) {
                return {
                    emailExists: true
                };
            }
            return null;
        }));
    };
}

