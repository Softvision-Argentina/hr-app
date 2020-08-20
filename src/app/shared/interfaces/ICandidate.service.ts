import { Observable } from 'rxjs';

export interface ICandidate {
    exists(email: string, id: number): Observable<any>;
    data: any;
}
