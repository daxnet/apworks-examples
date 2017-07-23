import { TestBed, inject } from '@angular/core/testing';

import { GlobalEventsManagerService } from './global-events-manager.service';

describe('GlobalEventsManagerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GlobalEventsManagerService]
    });
  });

  it('should ...', inject([GlobalEventsManagerService], (service: GlobalEventsManagerService) => {
    expect(service).toBeTruthy();
  }));
});
