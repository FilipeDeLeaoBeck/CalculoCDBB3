export interface CdbResult {
  errors: string[];
  success: boolean;
  // Valor bruto
  grossAmount: number;
  // Valor líquido
  netAmount: number;
}
