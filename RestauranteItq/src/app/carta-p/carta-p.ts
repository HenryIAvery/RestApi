import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { CurrencyPipe } from '@angular/common';
import { switchMap } from 'rxjs';
import { MenuApi } from '../services/menu-api';
import { CategoriasApi } from '../services/categorias-api';

@Component({
  imports: [RouterLink, CurrencyPipe],
  selector: 'app-carta-p',
  styleUrl: './carta-p.css',
  templateUrl: './carta-p.html',
})
export class CartaP {
  private menuApi = inject(MenuApi);
  private categoriasApi = inject(CategoriasApi);
  private route = inject(ActivatedRoute);

  platos = toSignal(
    this.route.paramMap.pipe(
      switchMap((params) => {
        const id = params.get('categoriaId');
        return id ? this.categoriasApi.platosPorCategoria(Number(id)) : this.menuApi.platos();
      })
    ),
    { initialValue: [] }
  );
}
