import { ValidatorFn, FormControl, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { Process } from '@shared/models/process.model';

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

export function UniqueDniValidator(processes: Process[], processId: number): AsyncValidatorFn {
    processes = processes.filter(process => !!process.preOfferStage);
    return({value}: AbstractControl): Observable<ValidationErrors | null> => {
        const dniAlreadyExist = processes.find(process => process.preOfferStage && value && process.preOfferStage.dni === value && value !== 0 && process.id !== processId);
        return new Observable(subscriber => {
            if (!!dniAlreadyExist) {
                subscriber.next({dniExists: true});
            } else {
                subscriber.next(null);
            }
            subscriber.complete();
        });
    }
}