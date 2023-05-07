import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(private snackbar: SnackbarService) {}

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        return next.handle(request)
            .pipe(
                catchError((response: HttpErrorResponse) => {
                    console.error(response.message);
                    console.error(response.error);
                    this.handleError(request, response);
                    return throwError(() => response);
                })
            );
    }

    handleError(request: HttpRequest<unknown>, response: HttpErrorResponse) {
        switch (response.status) {
            case 403:
                this.snackbar.show(
                    'Exceeded number of TM requests',
                    'error'
                );
                break;
            case 404: break;
            default:
                this.snackbar.show(
                    request.url.includes('/home/')
                        ? 'Could not search players'
                        : 'Could not get player profile',
                    'error');
                break;
        }
    }
}
