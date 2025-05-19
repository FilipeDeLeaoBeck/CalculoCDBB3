import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CdbResult } from "./CdbResult";

@Injectable({
    providedIn: 'root'
})
export class CdbService {

    constructor(private http: HttpClient) { }

    calculateCdb(initialAmount: number, months: number) {
        // definir como variável de ambiente para case real
        const url = `https://localhost:7192/cdb/calculate?initialValue=${initialAmount}&months=${months}`;
        return this.http.get<CdbResult>(url);
    }
}