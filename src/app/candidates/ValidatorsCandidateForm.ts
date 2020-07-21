import { FormGroup, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { Candidate } from 'src/entities/candidate';
import { delay, map } from 'rxjs/operators';
import { CandidateService } from 'src/app/services/candidate.service';

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

export function UniqueEmailValidator(candidateService: CandidateService): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return candidateService.exists(control.value).pipe(       
        map(result => result.body.exists ? {emailExists: true} : null)
      );
    };
  }
