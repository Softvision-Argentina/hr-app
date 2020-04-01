import { FormGroup } from '@angular/forms';
export function validateCandidateForm(form: FormGroup) {
    let isValid = true;
    if (form.get('isReferred')) {
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
        for (const i in form.controls) {
            form.controls[i].markAsDirty();
            form.controls[i].updateValueAndValidity();
            isValid = false;
            break;
        }
    }
    return isValid;
}
