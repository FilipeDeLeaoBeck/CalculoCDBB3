import { Component } from '@angular/core';
import { CdbResult } from './CDB/CdbResult';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { CdbService } from './CDB/cdb.service';

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
    private formBuilder: FormBuilder,
    private cdbService: CdbService
  ) {
    this.cdbForm = this.formBuilder.group({
      initialAmount: [0, [Validators.required, this.cdbFormInitialAmountValidator]],
      months: [0, [Validators.required, this.cdbFormMonthsValidator]]
    })
  }

  onSubmit() {
    const formData = this.cdbForm.value;

    this.cdbService.calculateCdb(formData.initialAmount, formData.months).subscribe({
      next: (res) => {
        this.result = res;
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
