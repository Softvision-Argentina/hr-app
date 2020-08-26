import { FormGroup } from '@angular/forms';

export function validateCandidateForm(form: FormGroup) {

    for (const i in form.controls) {
        if (form.controls[i]) {
          if (form.controls[i] !== form.controls['email']) {
            form.controls[i].markAsDirty();
            form.controls[i].updateValueAndValidity();
          }
        }
      }
    return form.status === 'VALID' ? true : false;
}
