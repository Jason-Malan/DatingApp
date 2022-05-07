import { Component, Input, OnInit } from '@angular/core';
import { FrontendUser } from 'src/app/_models/frontendUser';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member: FrontendUser;

  constructor() { }

  ngOnInit(): void {
  }

}
