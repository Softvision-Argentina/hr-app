import { preOfferStatusEnum } from '../enums/pre-offer-status.enum';
import { HealthInsuranceEnum } from '../enums/health-insurance.enum';

export class PreOffer {
    id: number;
    preOfferDate: Date;
    salary: number;
    status: preOfferStatusEnum;
    processId: number;
    healthInsurance: HealthInsuranceEnum;
    vacationDays: number;
    bonus: number;
    notes: String;
    tentativeStartDate: Date;
    healthInsuranceText: string; // ??
}