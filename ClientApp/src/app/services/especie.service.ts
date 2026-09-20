import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Especie } from '../models/tree.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EspecieService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/especies`; // Ajuste a URL no seu environment

  obterTodas(): Observable<Especie[]> {
    return this.http.get<Especie[]>(this.apiUrl);
  }

  obterPorId(id: string): Observable<Especie> {
    return this.http.get<Especie>(`${this.apiUrl}/${id}`);
  }

  criar(especie: Especie): Observable<Especie> {
    return this.http.post<Especie>(this.apiUrl, especie);
  }

  atualizar(id: string, especie: Especie): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, especie);
  }

  deletar(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
