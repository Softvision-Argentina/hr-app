import { LoadingState } from '@shared/enums/loadingState.enum';
import { ErrorState } from './errorState.model';

export type CallState = LoadingState | ErrorState;
