import { Role } from './role';

export class Employee {
    constructor
        (public id: number, public name: string,
            public lastName: string, public phoneNumber: string,
            public emailAddress: string, public linkedInProfile: string,
            public additionalInformation: string, public status: number,
            public userId: number, public dni: number,
            public role: Role, public roleId: number,
            public isReviewer: boolean, public reviewer: Employee, public reviewerId: number) { }
}
