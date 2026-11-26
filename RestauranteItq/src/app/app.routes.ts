import { Routes } from '@angular/router';
import { CartaP } from './carta-p/carta-p';
import { Inicio } from './inicio';
import { Pedido } from './pedido/pedido';
import { Categorias } from './categorias/categorias';
import { Reporte } from './reporte/reporte';
import { Login } from './login/login';
import { rolGuard } from './guards/rol-guard';

export const routes: Routes = [
    { path: '', component: Inicio },
    { path: 'login', component: Login },
    { path: 'carta', component: CartaP },
    { path: 'carta/:categoriaId', component: CartaP },
    { path: 'categorias', component: Categorias },
    { path: 'pedido', component: Pedido },
    {
        path: 'reporte',
        component: Reporte,
        canActivate: [rolGuard('Administrador')]
    },
];
