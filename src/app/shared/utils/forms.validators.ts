import { AbstractControl, ValidationErrors } from '@angular/forms';
import { CheckedEnum } from '@shared/enums/checked.enum';
export function customEmailAndPhoneNumberValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.get('email').value) {
        if (control.get('phoneNumber').value === null || control.get('phoneNumber').value.length === 0 || control.get('phoneNumber').invalid) {
            control.get('email').setErrors({ emailAndPhoneEmpty: true });
        } else {
            control.get('email').setErrors(null)
        }
    }

    if (!control.get('phoneNumber').value) {
        if (control.get('email').value === null || control.get('email').value.length === 0 || control.get('email').invalid) {
            control.get('phoneNumber').setErrors({ emailAndPhoneEmpty: true });
        } else {
            control.get('phoneNumber').setErrors(null);
        }
    }

    return null;
}

export function customCvAndLinkedInValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.get('link').value) {
        if (control.get('file').value === null || control.get('file').value.length === 0 || control.get('file').invalid) {
            control.get('link').setErrors({ cvAndLinkedInEmpty: true });
        } else {
            control.get('link').setErrors(null)
        }
    }

    if (!control.get('file').value) {
        if (control.get('link').value === null || control.get('link').value.length === 0 || control.get('link').invalid) {
            control.get('file').setErrors({ cvAndLinkedInEmpty: true });
        } else {
            control.get('file').setErrors(null);
        }
    }

    return null;
}

export function checkDateIsnotEmpty(control: AbstractControl): ValidationErrors | null {
    if (control.get('done').value !== CheckedEnum.NA){
        if (!control.get('date').value) {
            return { invalidDate: true };
        }
    }
    return null;
}