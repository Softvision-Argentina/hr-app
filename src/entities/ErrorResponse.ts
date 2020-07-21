export class ValidationError{
    name: string;
    description: string;
}

export class ErrorResponse {
    additionalInfo: any;
    errorCode: number;
    exceptionMessage: string;
    httpStatusCode: number;
    innerExceptionMessage: string;
    validationErrors: ValidationError[];
}
