import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PlatformUser } from './_models/platformUser';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'The Dating app';
  users: any;

  constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: PlatformUser = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }
}
