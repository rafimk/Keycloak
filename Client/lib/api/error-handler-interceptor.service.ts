import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operator';

@Injectable()
export class ErrorHandlerInterceptorService implements HttpInterceptor {

    constructor() {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError((error: HttpErrorResponse) => {
            if (error.status === 401) {
                // Unauthorized
            }

            if (error.status === 404) {
                // Resource not found
            }

            if (error.status === 500) {
                // Server side error
            }

            return throwError(error);
        }));
    }
}