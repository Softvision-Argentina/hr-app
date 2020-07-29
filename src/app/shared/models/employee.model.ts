import { Role } from './role.model';

export interface Employee {
    id: number,
    name: string,
    lastName: string,
    phoneNumber: string,
    emailAddress: string,
    linkedInProfile: string,
    additionalInformation: string,
    status: number,
    userId: number,
    dni: number,
    role: Role,
    roleId: number,
    isReviewer: boolean,
    reviewer: Employee,
    reviewerId: number
}
