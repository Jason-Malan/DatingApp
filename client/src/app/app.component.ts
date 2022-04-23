import { Component, OnInit } from '@angular/core';
import { Storage } from './_models/local_storage';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'The Dating App';

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user = JSON.parse(localStorage.getItem(Storage.APP_USER));
    !!user
      ? this.accountService.setCurrentUser(user)
      : this.accountService.setCurrentUser(null);
  }
}
