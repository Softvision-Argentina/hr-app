import { ValidatorFn, FormControl } from '@angular/forms';

/// This directive does not allow a date from the past. Should be greater than Date now()
export const dateValidator: ValidatorFn = (control: FormControl) => {
    const now = new Date();
    now.setHours(0); now.setMinutes(0); now.setSeconds(0);

    if (control.value !== null) {
        if (control.value.toISOString() < now.toISOString()) {
            return {
                'pastDateError': true
            };
        }
    }
    return null;
};
