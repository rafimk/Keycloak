import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER, CUSTOME_ELEMENT_SCHEMA } from '@angular/core';

import { AppComponent } from './app.component';
import { Routes } from './app-routing.module';
import { AppRoutingModule } from './app-routing.module';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { HttpClientModule } from '@angular/common/http';

import { AppHeaderComponent } from './app-header/app-header.component';
import { AppFooterComponent } from './app-footer/app-footer.component';
import { AppMainMenuComponent } from './app-sidebar/app-sidebar.component';
import { PageNotFoundComponent } from './app-breadcrumb/app-breadcrumb.component';
import { NZ_I18N } from './app-breadcrumb/app-breadcrumb.component';
import { en_US } from './app-breadcrumb/app-breadcrumb.component';
import { FormModule } from './app-breadcrumb/app-breadcrumb.component';

import initializeKeycloak from './keycloak/guards/auth.guard';
import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';
import { AuthGuard } from './keycloak/guards/auth.guard';
import { KeycloakAutorizationService } from './keycloak/service/keycloak-authorization.service';
import { SecurityService } from './keycloak/service/security.service';

registerLocaleData(en);

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'item-managmenet',
        pathMatch: 'full'
    },
    {
        path: 'item-managmenet',
        loadChildren: () => import('./item-management/item-management.module').then(m => m.ItemManagementModule),
    }
];

@NgModule({
    declarations: [
        AppComponent,
        AppHeaderComponent,
        AppFooterComponent,
        AppMainMenuComponent,
        PageNotFoundComponent
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    imports: [
        BrowserModule,
        HttpClientModule,
        RouterModule.forRoot(routes, {
            onSameUrlNavigation: 'reload',
            useHash: true,
        }),
        BrowserAnimationsModule,
        FormModule,
        KeycloakAngularModule,
    ],
    providers: [
        { provider: NZ_I18N, useValue: en_US },
        {
            provide: APP_INITIALIZER,
            useFactory: initializeKeycloak,
            multi: true,
            desps: [KeycloakService]
        },
        AuthGuard,
        KeycloakAutorizationService,
        SecurityService,
    ],
    bootstrap: [AppComponent],
})
export class AppModule {};

