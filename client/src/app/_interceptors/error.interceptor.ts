import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error) => {
        switch (error.status) {
          case 400:
            if (error.error.errors) {
              const modalStateErrors = [];
              error.error.errors.array.forEach((key) => {
                modalStateErrors.push(error.error.errors[key]);
              });
              throw modalStateErrors.flat();
            } else {
              this.toastr.error(error.statusText, error.status);
            }
            break;
          case 401:
            this.toastr.error(error.statusText, error.status);
            break;
          case 404:
            this.router.navigateByUrl('/not-found');
            break;
          case 500:
            const navExtras: NavigationExtras = {
              state: { error: error.error },
            };
            this.router.navigate(['/server-error'], navExtras);
            break;
          default:
            this.toastr.error('Something unhandled occured, handle me please!');
            console.log(error);
            break;
        }
        return throwError(error);
      })
    );
  }
}
