import { TestBed } from '@angular/core/testing';

import { LibrarianUserAuthenticationGuard } from './librarian-user-authentication.guard';

describe('LibrarianUserAuthenticationGuard', () => {
  let guard: LibrarianUserAuthenticationGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(LibrarianUserAuthenticationGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
