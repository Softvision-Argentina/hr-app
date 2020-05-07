import { FormGroup, AbstractControl } from '@angular/forms'

export function formFieldHasRequiredValidator(field: string, form: FormGroup): boolean {
    const form_field = form.get(field);
    
    if (!form_field.validator) {
        return false;
    }

    const validator = form_field.validator({ 'value': null } as AbstractControl);
    return (validator && validator.required);
  }