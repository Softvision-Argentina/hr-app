import { OfferStatusEnum } from '../enums/offer-status.enum';

export interface Offer {
    id: number;
    offerDate: Date;
    salary: number;
    rejectionReason: string;
    status: OfferStatusEnum;
    processId: number;
}
