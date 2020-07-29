import { ValidationError } from './validation-error.model';

export interface ErrorResponse {
    additionalInfo: any;
    errorCode: number;
    exceptionMessage: string;
    httpStatusCode: number;
    innerExceptionMessage: string;
    validationErrors: ValidationError[];
}
