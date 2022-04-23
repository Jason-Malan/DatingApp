import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Storage } from '../_models/local_storage';
import { AppUser } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private baseUrl = 'https://localhost:5000/api/';
  private currentUser = new BehaviorSubject<AppUser>(null);
  public currentUser$ = this.currentUser.asObservable();

  constructor(private http: HttpClient) {}

  register(model: any) {
    return this.http
      .post<AppUser>(this.baseUrl + 'account/register', model)
      .pipe(
        tap((user) => {
          if (user) {
            localStorage.setItem(Storage.APP_USER, JSON.stringify(user));
            this.currentUser.next(user);
          }
        })
      );
  }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: AppUser) => {
        const user = response;
        if (user) {
          localStorage.setItem('appUser', JSON.stringify(user));
          this.currentUser.next(user);
        }
      })
    );
  }

  setCurrentUser(user: AppUser) {
    this.currentUser.next(user);
  }

  logout() {
    this.currentUser.next(null);
    localStorage.removeItem(Storage.APP_USER);
  }

  getUsers() {
    return this.http.get<any[]>('https://localhost:5000/api/users');
  }
}
