import { Component, OnInit } from '@angular/core';
import { FrontendUser } from '../_models/frontendUser';
import { MemberService } from '../_services/member.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
})
export class ListsComponent implements OnInit {
  members: Partial<FrontendUser[]>;
  predicate = 'liked';

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.memberService.getUserLikes(this.predicate).subscribe((response) => {
      this.members = response;
      console.log('members', this.members);
    });
  }
}
