import { Component, inject, signal, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ReporteApi, ReporteDia } from '../services/reporte-api';
import { AuthApi } from '../services/auth-api';

@Component({
  selector: 'app-reporte',
  imports: [CurrencyPipe, FormsModule, RouterLink],
  templateUrl: './reporte.html',
  styleUrl: './reporte.css',
})
export class Reporte implements OnInit {
  private api = inject(ReporteApi);
  auth = inject(AuthApi);

  fecha = this.hoyLocal();
  datos = signal<ReporteDia | null>(null);
  cargando = signal(false);
  error = signal('');

  ngOnInit() {
    this.cargar();
  }

  private hoyLocal(): string {
    const d = new Date();
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  cargar() {
    this.cargando.set(true);
    this.error.set('');
    this.api.delDia(this.fecha).subscribe({
      next: (r) => {
        this.datos.set(r);
        this.cargando.set(false);
      },
      error: (err) => {
        this.cargando.set(false);
        this.error.set(
          err?.error?.error ??
            `No se pudo cargar el reporte (código ${err.status || 'sin conexión'}).`
        );
      },
    });
  }

  porcentaje(cantidad: number): number {
    const total = this.datos()?.totalPedidos ?? 0;
    return total === 0 ? 0 : Math.round((cantidad * 100) / total);
  }
}
