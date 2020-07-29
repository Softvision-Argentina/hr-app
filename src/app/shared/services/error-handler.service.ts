import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ErrorResponse } from '@shared/models/error-response.model';
@Injectable({
    providedIn: 'root'
})
export class ErrorHandlerService {

    constructor(private toastrService: ToastrService) {}

    showErrorMessage(messages: string[], defaultMessage?: string): void {
        if (!messages){
            this.toastrService.error(defaultMessage);
        }
        else{
            messages.forEach(message => {
                this.toastrService.error(message);
            })
        }
    }
}
