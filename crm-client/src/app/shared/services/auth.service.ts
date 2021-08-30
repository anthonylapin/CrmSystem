import { Injectable } from '@angular/core';
import { IUser } from '../interfaces';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

const localStorageAuthTokenKey = 'auth-token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private token: string | null;

  constructor(private http: HttpClient) {
    this.token = localStorage.getItem(localStorageAuthTokenKey);
  }

  setToken(token: string | null) {
    this.token = token;
  }

  getToken(): string {
    return this.token ?? '';
  }

  isAuthenticated(): boolean {
    return Boolean(this.token);
  }

  logout() {
    this.setToken(null);
    localStorage.clear();
  }

  login(user: IUser): Observable<{ token: string }> {
    return this.http.post<{ token: string }>('/api/auth/login', user).pipe(
      tap(({ token }) => {
        localStorage.setItem(localStorageAuthTokenKey, token);
        this.setToken(token);
      })
    );
  }

  register(user: IUser): Observable<void> {
    return this.http.post<void>('/api/auth/register', user);
  }
}
