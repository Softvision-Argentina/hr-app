import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
@Injectable({
    providedIn: 'root'
})
export class ErrorHandlerService {

    constructor(private toastrService: ToastrService) {}
    showErrorMessage(err, defaultMessage?: string) {
        if (!!err.message) {
            this.toastrService.error(err.message);
        } else if (!!defaultMessage) {
            this.toastrService.error(defaultMessage);
        } else {
            this.toastrService.error('The service is not available now. Try again later.');
        }
    }
}