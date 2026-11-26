import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { categorias } from '../models/categorias';
import { plato } from '../models/plato';

@Injectable({ providedIn: 'root' })
export class CategoriasApi {
  private http = inject(HttpClient);
  private url = 'http://localhost:5260/categorias';

  categorias() {
    return this.http.get<categorias[]>(this.url);
  }

  platosPorCategoria(id: number) {
    return this.http.get<plato[]>(`${this.url}/${id}/platos`);
  }
}