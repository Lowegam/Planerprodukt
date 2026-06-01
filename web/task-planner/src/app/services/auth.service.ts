import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5056/api/Auth';
  constructor(private http: HttpClient) {}
  register(u: string, e: string, p: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, { username: u, email: e, password: p });
  }
  login(u: string, p: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { username: u, password: p }).pipe(
      tap((r: any) => localStorage.setItem('token', r.token))
    );
  }
  logout() { localStorage.removeItem('token'); }
  getToken() { return localStorage.getItem('token'); }
  isLoggedIn() { return !!this.getToken(); }
}