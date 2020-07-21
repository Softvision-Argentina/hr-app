import { TestBed, inject } from '@angular/core/testing';

import { CandidateProviderService } from './candidate-provider.service';

describe('CandidateProviderService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CandidateProviderService]
    });
  });

  it('should be created', inject([CandidateProviderService], (service: CandidateProviderService) => {
    expect(service).toBeTruthy();
  }));
});
