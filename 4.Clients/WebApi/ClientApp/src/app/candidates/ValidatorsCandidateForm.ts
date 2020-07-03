import { FormGroup, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { Candidate } from 'src/entities/candidate';
import { delay } from 'rxjs/operators';
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

export function UniqueEmailValidator(candidates: Candidate[]): AsyncValidatorFn {

    return({value}: AbstractControl): Observable<ValidationErrors | null> => {
        const emailUsed = candidates.find(candidate => candidate.emailAddress === value && value && value.length > 0);
        return new Observable(subscriber =>{
            if(emailUsed) {
                subscriber.next({emailExists: true})
            } else{
                subscriber.next(null);
            }
            subscriber.complete();
        });
    };
}

