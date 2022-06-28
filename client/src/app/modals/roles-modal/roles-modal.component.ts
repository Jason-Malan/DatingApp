import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css'],
})
export class RolesModalComponent implements OnInit {
  updateSelectedRoles: Subject<any[]> = new Subject();
  user: User;
  roles: any[] = [];

  title?: string;
  closeBtnName?: string;

  constructor(public bsModalRef: BsModalRef) {}

  ngOnInit() {}

  updateRoles() {
    this.updateSelectedRoles.next(this.roles);
    this.bsModalRef.hide();
  }
}
