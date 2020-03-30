import { OfferStatusEnum } from './enums/offer-status.enum';


export class Offer {
    id: number;
    offerDate: Date;
    salary: number;
    rejectionReason: string;
    status: OfferStatusEnum;
    processId: number;
}
