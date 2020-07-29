import { FormGroup, AbstractControl } from '@angular/forms'
import { StageStatusEnum } from '@shared/enums/stage-status.enum';

export function formFieldHasRequiredValidator(field: string, form: FormGroup): boolean {
    const form_field = form.get(field);
    
    if (!form_field.validator) {
        return false;
    }

    const validator = form_field.validator({ 'value': null } as AbstractControl);
    return (validator && validator.required);
  }

export function CanShowReaddressPossibility(currentStageStatus) {
    let validStatus = [StageStatusEnum.Declined, StageStatusEnum.Rejected, StageStatusEnum.Pipeline];
    return (validStatus.indexOf(currentStageStatus) > -1);
  }