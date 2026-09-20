import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Arvore, ArvoreDetalhada } from '../models/tree.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ArvoreService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/arvores`;

  criar(arvore: Arvore): Observable<Arvore> {
    return this.http.post<Arvore>(this.apiUrl, arvore);
  }

  atualizar(id: string, arvore: Arvore): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, arvore);
  }

  deletar(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Retorna o DTO estendido com os dados populados da Espécie via \$lookup do MongoDB
  obterTodas(): Observable<ArvoreDetalhada[]> {
    return this.http.get<ArvoreDetalhada[]>(this.apiUrl);
  }

  obterPorId(id: string): Observable<ArvoreDetalhada> {
    return this.http.get<ArvoreDetalhada>(`${this.apiUrl}/${id}`);
  }
}
