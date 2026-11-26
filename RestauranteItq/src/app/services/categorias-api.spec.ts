import { TestBed } from '@angular/core/testing';
import { CategoriasApi } from './categorias-api';

describe('CategoriasApi', () => {
  let service: CategoriasApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategoriasApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
