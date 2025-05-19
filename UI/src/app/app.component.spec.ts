import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      imports: [
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientTestingModule
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);

    fixture.detectChanges();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve criar o componente', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('formulário deve ser válido quando valorInicial > 0 e meses > 1', () => {
    component.cdbForm.setValue({
      initialAmount: 1000,
      months: 12
    });
    expect(component.cdbForm.valid).toBeTrue();
  });

  it('deve ser inválido se valorInicial for menor ou igual a 0', () => {
    component.cdbForm.setValue({
      initialAmount: 0,
      months: 12
    });
    expect(component.cdbForm.valid).toBeFalse();
  });

  it('deve ser inválido se o prazo em meses for menor ou igual a 1', () => {
    component.cdbForm.setValue({
      initialAmount: 1000,
      months: 1
    });
    expect(component.cdbForm.valid).toBeFalse();
  });

  it('deve fazer a requisição com sucesso', () => {
    component.cdbForm.setValue({ initialAmount: 1000, months: 6 });

    const mockResponse = {
      success: true,
      grossAmount: 1100,
      netAmount: 1050,
      errors: []
    };

    component.onSubmit();

    const req = httpMock.expectOne(
      'https://localhost:7192/cdb/calculate?initialValue=1000&months=6'
    );
    expect(req.request.method).toBe('GET');

    req.flush(mockResponse);

    expect(component.result).toEqual(mockResponse);
    expect(component.showResult).toBeTrue();
    expect(component.showErrors).toBeFalse();
  });

  it('deve fazer a requisição e tratar erro 400', () => {
    component.cdbForm.setValue({ initialAmount: 1000, months: 6 });

    const mockErrorResponse = {
      success: false,
      grossAmount: 0,
      netAmount: 0,
      errors: ['Valor inválido']
    };

    component.onSubmit();

    const req = httpMock.expectOne(
      'https://localhost:7192/cdb/calculate?initialValue=1000&months=6'
    );

    req.flush(mockErrorResponse, { status: 400, statusText: 'Bad Request' });

    expect(component.result).toEqual(mockErrorResponse);
    expect(component.showErrors).toBeTrue();
    expect(component.showResult).toBeFalse();
  });
});
