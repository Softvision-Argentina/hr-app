import { ValidatorFn, FormControl } from '@angular/forms';

export const dniValidator: ValidatorFn = (control: FormControl) => {
    if (control.value != null) {
        if (control.value > 99999999) {
            return {
                'dniTooLongError': true
            };
        }
        if (control.value < 0) {
            return {
                'dniTooShortError': true
            };
        }
    }
    return null;
};
