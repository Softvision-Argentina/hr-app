import { preOfferStatusEnum } from './enums/pre-offer-status.enum';
import { HealthInsuranceEnum } from './enums/health-insurance.enum';
import { TestBed } from '@angular/core/testing';


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


// export class PreOffer {
//     id: number;
//     preOfferDate: Date;
//     grossSalary: number;
//     vacationDays: number;
//     healthInsurance: HealthInsuranceEnum;
//     healthInsuranceText: string;
//     
//     condition: preOfferStatusEnum;
//     bonus: text;
//     processId: number;
// }
