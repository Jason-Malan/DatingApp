import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FrontendUser } from 'src/app/_models/frontendUser';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent implements OnInit {
  @Input() member: Partial<FrontendUser>;

  constructor(
    private memberService: MemberService,
    private toastr: ToastrService
  ) {}

  likeUser(member: Partial<FrontendUser>) {
    this.memberService.likeUser(member.username).subscribe(() => {
      this.toastr.success('You have liked ' + member.knownAs);
    });
  }

  ngOnInit(): void {}
}
