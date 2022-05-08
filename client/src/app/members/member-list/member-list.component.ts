import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { FrontendUser } from 'src/app/_models/frontendUser';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  public members: Observable<FrontendUser[]>;

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.members = this.memberService.getMembers();
  }
}
