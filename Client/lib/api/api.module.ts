import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ErrorHandlerInterceptorService } from './error-handler-interceptor.service';

@NgModule({
    imports: [CommonModule, HttpClientModule],
    providers: [ErrorHandlerInterceptorService]
})
export class ApiModule {}
