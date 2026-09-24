import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private tokenKey = 'auth_token';
  constructor(private http: HttpClient) { }

  login(model: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/login`, model).pipe(
      map((res) => {
        const token = res?.token ?? res?.accessToken ?? res?.jwt;
        if (token) {
          localStorage.setItem(this.tokenKey, token);
        }
        return res;
      })
    );
  }

  register(model: { email: string; password: string }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/register`, model);
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }
}
