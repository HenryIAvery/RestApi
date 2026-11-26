import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-inicio',
    imports: [RouterLink],
    template:`
     <h1>Hola, mundo </h1>
     <p>Estudiantes bienvenidos</p>
     <button type="button" routerLink="/carta">Ver carta</button>
     <button type="button" routerLink="/categorias">Ver categorías</button>
     <button type="button" routerLink="/pedido">Ver pedidos</button>
     <button type="button" routerLink="/reporte">Reporte del día</button>
     `,
})
export class Inicio {}
