import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { plato } from '../models/plato';

@Injectable({ providedIn: 'root' })

export class MenuApi {

  private http = inject(HttpClient);

  private url = 'http://localhost:5260/platos';

 

  platos() {

    return this.http.get<plato[]>(this.url);

  }

}