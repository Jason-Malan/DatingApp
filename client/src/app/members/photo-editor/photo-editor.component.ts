import { Component, Input, OnInit } from '@angular/core';
import { FrontendUser } from 'src/app/_models/frontendUser';
import { PlatformUser } from 'src/app/_models/platformUser';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: FrontendUser;
  constructor() {}

  ngOnInit(): void {}
}
