import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CdbService } from './cdb.service';
import { CdbResult } from './CdbResult';

describe('CdbService', () => {
    let service: CdbService;
    let httpMock: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
            providers: [CdbService]
        });

        service = TestBed.inject(CdbService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
        httpMock.verify();
    });

    it('deve fazer a requisição com sucesso', () => {
        const mockResult: CdbResult = {
            success: true,
            grossAmount: 1000,
            netAmount: 1050,
            errors: []
        };

        service.calculateCdb(1000, 6).subscribe(res => {
            expect(res).toEqual(mockResult);
        });

        const req = httpMock.expectOne('https://localhost:7192/cdb/calculate?initialValue=1000&months=6');
        expect(req.request.method).toBe('GET');

        req.flush(mockResult);
    });

    it('deve processar uma requisição com erro', () => {
        const mockError = { success: false, grossAmount: 0, netAmount: 0, errors: ['Erro'] };

        service.calculateCdb(1000, 6).subscribe({
            next: () => fail('Deveria falhar'),
            error: (error) => {
                expect(error.error).toEqual(mockError);
            }
        });

        const req = httpMock.expectOne('https://localhost:7192/cdb/calculate?initialValue=1000&months=6');
        req.flush(mockError, { status: 400, statusText: 'Bad Request' });
    });
});