import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { CdbResult } from './CDB/CdbResult';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  cdbForm!: FormGroup;
  initialAmount!: number;
  months!: number;
  showResult = false;
  showErrors = false;
  result!: CdbResult;

  initialAmountError: boolean = false;
  monthsError: boolean = false;

  constructor(
    private readonly http: HttpClient,
    private formBuilder: FormBuilder
  ) {
    this.cdbForm = this.formBuilder.group({
      initialAmount: [0, [Validators.required, this.cdbFormInitialAmountValidator]],
      months: [0, [Validators.required, this.cdbFormMonthsValidator]]
    })
  }

  onSubmit() {
    const formData = this.cdbForm.value;

    // definir como variável de ambiente para case real
    const url = `https://localhost:7192/cdb/calculate?initialValue=${formData.initialAmount}&months=${formData.months}`;

    this.http.get<CdbResult>(url).subscribe({
      next: (res) => {
        console.log(`Resposta: ${JSON.stringify(res)}`);
        this.result = res;

        console.log(`CdbResult: ${JSON.stringify(this.result)}`);

        this.showResult = true;
        this.showErrors = false;
      },
      error: (err) => {

        if (err.error && err.status == 0) {
          const errorText = 'Houve um erro na requisição:' + err.name;
          this.result = {
            "errors": [errorText],
            "success": false,
            "grossAmount": 0,
            "netAmount": 0
          }
        }
        else {
          this.result = err.error;
        }

        this.showErrors = true;
        this.showResult = false;
      },
    });
  }

  // Valida se o valor inicial informado é válido
  validateInitialAmount() {
    const initialAmountValue = this.cdbForm.get('initialAmount')?.value;
    this.initialAmountError = initialAmountValue <= 0;
  }

  // Valida se o valor informado para os meses é válido
  validateMonths() {
    const monthsValue = this.cdbForm.get('months')?.value;
    this.monthsError = !(Number.isInteger(monthsValue) && monthsValue > 1);
  }

  cdbFormInitialAmountValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    return value > 0 ? null : { isValid: false };
  }

  cdbFormMonthsValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    return Number.isInteger(value) && value > 1 ? null : { isValid: false };
  }
}
