import {Injectable} from "@angular/core";
import {IUser} from "../interfaces";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {tap} from "rxjs/operators";



@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private token : string | null = null;

  constructor(private http: HttpClient) {}

  setToken(token: string | null) {
    this.token = token;
  }

  getToken() : string | null {
    return this.token;
  }

  isAuthenticated() : boolean {
    return Boolean(this.token);
  }

  logout() {
    this.setToken(null);
    localStorage.clear();
  }

  login(user: IUser) : Observable<{token: string}> {
     return this.http.post<{token: string}>('/api/auth/login', user)
       .pipe(
         tap(
           ({token}) => {
             localStorage.setItem('auth-token', token);
             this.setToken(token);
           }
         )
       );
  }

  register() {

  }
}