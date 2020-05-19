import { preOfferStatusEnum } from './enums/pre-offer-status.enum';


export class PreOffer {
    id: number;
    offerDate: Date;
    salary: number;
    rejectionReason: string;
    status: preOfferStatusEnum;
    processId: number;
}
