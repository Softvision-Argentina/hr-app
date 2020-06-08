import { preOfferStatusEnum } from './enums/pre-offer-status.enum';
import { HealthInsuranceEnum } from './enums/health-insurance.enum';


export class PreOffer {
    id: number;
    preOfferDate: Date;
    salary: number;
    vacationDays: number;
    healthInsurance: HealthInsuranceEnum;
    healthInsuranceText: string;
    status: preOfferStatusEnum;
    processId: number;
}
