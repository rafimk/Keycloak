import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { KeycloakAuthGuard, KeycloakService } from 'keycloak-angular';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard extends KeycloakAuthGurd {
    constructor(protected router: Router, protected keycloak: KeycloakService) {
        super(router, keycloak);
    }

    public isAccessAllowed(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
        return new Promise(async (resolve, reject) => {
            if (!this.authenticated) {
                this.keycloak.login({
                    redirectUri: window.location.origin + state.url,
                });
                return;
            }

            const requiredRoles = route.data.roles;

            if (!requiredRoles || requiredRoles.length === 0) {
                resolve(true);
            } else if (!this.roles || this.roles.length === 0) {
                resolve(false);
            } else {
                resolve(requiredRoles.some(role => this.roles.indexOf(role) > -1));
            }
        });
    }
}