import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators'; // Importante para capturar o erro
import { Router } from '@angular/router';   // Importante para poder redirecionar
import { AuthService } from '../services/auth.service';

@Injectable() export class AuthInterceptor implements HttpInterceptor {
  // Injetamos o Router aqui no construtor junto com o seu AuthService
  constructor(
    private auth: AuthService,
    private router: Router
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const url = (req.url || '').toLowerCase();

    // Não adicionar Authorization em chamadas de login/register
    if (url.endsWith('/login') || url.endsWith('/register') || url.includes('/identity/')) {
      return next.handle(req);
    }

    const token = this.auth.getToken();
    let requestToHandle = req;

    if (token) {
      requestToHandle = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    // Enviamos a requisição e escutamos a resposta do Azure usando o pipe(catchError)
    return next.handle(requestToHandle).pipe(
      catchError((error: HttpErrorResponse) => {
        // Se o Azure responder 401 Unauthorized, o token expirou
        if (error.status === 401) {
          console.warn('Sessão expirada. Limpando dados e redirecionando para o login...');

          // Executa o logout no seu serviço (limpa localStorage, zera variáveis, etc.)
          if (typeof this.auth.logout === 'function') {
            this.auth.logout();
          } else {
            // Caso seu AuthService não tenha a função logout criada ainda, limpa direto:
            localStorage.clear();
          }

          // Redireciona o usuário para a página de login do Angular
          this.router.navigate(['/login']);
        }

        // Repassa o erro adiante para não quebrar o fluxo do Angular
        return throwError(() => error);
      })
    );
  }
}
