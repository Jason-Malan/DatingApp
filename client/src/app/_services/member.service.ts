import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { FrontendUser } from '../_models/frontEndUser';
import { PaginatedResult } from '../_models/Pagination';
import { PlatformUser } from '../_models/platformUser';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private baseUrl = environment.apiUrl;
  public members: FrontendUser[] = [];
  public membersCache = new Map();
  private userParams: UserParams;
  private user: PlatformUser;

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user) => {
      this.user = user;
      this.userParams = new UserParams(user);
    });
  }

  set UserParams(params: UserParams) {
    this.userParams = params;
  }

  get UserParams() {
    return this.userParams;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getMembers(userParams: UserParams) {
    console.log(Object.values(userParams).join(' - '));
    const response = this.membersCache.get(Object.values(userParams).join('-'));
    if (response) {
      return of(response);
    }

    let params = this.getPaginationHeaders(
      userParams.pageNumber,
      userParams.pageSize
    );

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginatedResult<FrontendUser[]>(
      this.baseUrl + 'users',
      params
    ).pipe(
      tap((members) => {
        console.log(Object.values(userParams).join(' - '));
        this.membersCache.set(Object.values(userParams).join('-'), members);
      })
    );
  }

  private getPaginatedResult<T>(url, params) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return this.http
      .get<T>(url, {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  private getPaginationHeaders(page: number, itemsPerPage: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());

    return params;
  }

  getMember(username: string) {
    const member = [...this.membersCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member) => member.username === username);

    if (member) {
      return of(member);
    }

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

  setMainPhoto(photoId: number) {
    return this.http.put(
      this.baseUrl + 'users/set-main-photo/' + photoId.toString(),
      { photoId }
    );
  }

  deletePhoto(photoId: number) {
    return this.http.delete(
      this.baseUrl + 'users/delete-photo/' + photoId.toString()
    );
  }

  likeUser(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }

  getUserLikes(predicate: string) {
    return this.http.get<Partial<FrontendUser[]>>(this.baseUrl + 'likes', {
      params: { predicate: predicate },
    });
  }
}
