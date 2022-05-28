import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { FrontendUser } from 'src/app/_models/frontendUser';
import { Pagination } from 'src/app/_models/Pagination';
import { PlatformUser } from 'src/app/_models/platformUser';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  public members: FrontendUser[];
  public pagination: Pagination;
  public userParams: UserParams;
  public user: PlatformUser;
  public genders = [
    { value: 'male', displayName: 'Males' },
    { value: 'female', displayName: 'Females' },
  ];

  constructor(private memberService: MemberService) {
    this.userParams = this.memberService.UserParams;
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.UserParams = this.userParams;
    this.memberService.getMembers(this.userParams).subscribe((response) => {
      this.members = response.result;
      this.pagination = response.pagination;
    });
  }

  resetFilterParams() {
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.memberService.UserParams = this.userParams;
    this.loadMembers();
  }

  onGenderChanged() {
    this.userParams.gender =
      this.userParams.gender === 'male' ? 'female' : 'male';
  }
}
