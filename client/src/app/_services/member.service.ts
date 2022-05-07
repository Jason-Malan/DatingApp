import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { FrontendUser } from '../_models/frontEndUser';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getMembers() {
    return this.http.get<FrontendUser[]>(this.baseUrl + 'users');
  }

  getMember(username: string) {
    return this.http.get<FrontendUser>(
      this.baseUrl + 'users/' + username
    );
  }
}
