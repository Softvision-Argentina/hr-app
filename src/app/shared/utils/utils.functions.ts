import { FormGroup, AbstractControl } from '@angular/forms'
import { StageStatusEnum } from '@shared/enums/stage-status.enum';
import { ErrorResponse } from '@shared/models/error-response.model';
import { Observable } from 'rxjs';

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

export function handleError(httpErrorResponse, cb ) : Observable<any> | string[]{
  const errorObject: ErrorResponse = httpErrorResponse.error;
  const defaultServerErrorMessage = 'The service is not available now. Try again later.';
  if (httpErrorResponse.status === 500){
    return cb([defaultServerErrorMessage]);
  }

  else if (httpErrorResponse.status === 400){
    if (errorObject.errorCode === 200){
      // back-end validation error from fluent validation
      const errorArray = [];
      errorObject.validationErrors.forEach(error => {
        errorArray.push(error.description);
      })
      return cb(errorArray);
    }
    else{
      // back-end business validations such as uniqueness of names and others
      return cb([errorObject.exceptionMessage]);
    }
  }

  else {
    return cb([defaultServerErrorMessage]);
  }
}