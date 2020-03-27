export class Consultant {
    constructor(public id: number, public name: string, public lastName: string) {}

    emailAddress: string;
    phoneNumber: string;
    additionalInformation: string;
}

export interface IConsultantResponse {
    total: number;
    results: Consultant[];
}
