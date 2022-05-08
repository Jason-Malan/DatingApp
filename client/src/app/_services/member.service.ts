import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { FrontendUser } from '../_models/frontEndUser';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private baseUrl = environment.apiUrl;
  public members: FrontendUser[] = [];

  constructor(private http: HttpClient) {}

  getMembers() {
    if (this.members.length > 0) return of(this.members);
    return this.http.get<FrontendUser[]>(this.baseUrl + 'users').pipe(
      tap((members) => {
        this.members = members;
      })
    );
  }

  getMember(username: string) {
    const foundMember = this.members.find((x) => x.userName === username);
    if (foundMember !== undefined) return of(foundMember);
    return this.http.get<FrontendUser>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: FrontendUser) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      tap(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
}
