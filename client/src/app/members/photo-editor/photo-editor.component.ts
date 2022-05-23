import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { FrontendUser } from 'src/app/_models/frontendUser';
import { Photo } from 'src/app/_models/photo';
import { PlatformUser } from 'src/app/_models/platformUser';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: FrontendUser;
  private user: PlatformUser;
  public hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  public uploader: FileUploader;

  constructor(
    private accountService: AccountService,
    private memberService: MemberService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe((response) => {
      this.user = response;
    });
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
      this.member.photoUrl = photo.url;
      this.member.photos.forEach((p) => {
        if (p.isMain) {
          p.isMain = 0;
        }
        if (p.id === photo.id) {
          p.isMain = 1;
        }
      });
    });
  }

  deletePhoto(photo: Photo) {
    this.memberService.deletePhoto(photo.id).subscribe(() => {
      this.member.photos = this.member.photos.filter((p) => p.id !== photo.id);
      // this.member.photos.forEach(p => {
      //   if (p.id === photo.id) {
      //     this.member.photos.splice(this.member.photos.indexOf(p), 1);
      //   }
      // });
    });
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.member.photos.push(photo);
        if (photo.isMain) {
          this.user.photoUrl = photo.url;
          this.member.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
        }
      }
    };
  }
}
