import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface PlatoVendido {
  plato: string;
  cantidad: number;
  total: number;
}

export interface EstadoConteo {
  estado: string;
  cantidad: number;
}

export interface ReporteDia {
  fecha: string;
  totalPedidos: number;
  totalVendido: number;
  ticketPromedio: number;
  topPlatos: PlatoVendido[];
  porEstado: EstadoConteo[];
}

@Injectable({ providedIn: 'root' })
export class ReporteApi {
  private http = inject(HttpClient);
  private url = 'http://localhost:5260/reportes';

  delDia(fecha?: string) {
    const q = fecha ? `?fecha=${fecha}` : '';
    return this.http.get<ReporteDia>(`${this.url}/dia${q}`);
  }
}
