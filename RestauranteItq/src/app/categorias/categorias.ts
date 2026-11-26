import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { CategoriasApi } from '../services/categorias-api';

@Component({
  imports: [RouterLink],
  selector: 'app-categorias',
  styleUrl: './categorias.css',
  templateUrl: './categorias.html',
})
export class Categorias {
  private api = inject(CategoriasApi);

  categorias = toSignal(this.api.categorias(), { initialValue: [] });
}
