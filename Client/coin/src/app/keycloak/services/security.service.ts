import {Inectable} from '@angular/core';
import {KeycloakService} from 'keycloak-angular';
import { KeycloakAuthorizationService } from './keycloak-authorization.service';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
});

export class SecurityService {

    public readonly resources: environment.resourceServerId;

    constructor(private keycloak: KeycloakService, 
        private keycloakAuthorization: KeycloakAuthorizationService) {}

    public async isUserLoggedIn() {
        return await this.keycloak.isLoggedIn();
    }

    public isInGroup() {
        this.keycloak.loadUserProfile().then((profile) => {
            console.log(profile);
        });
    }

    public getUserName() {
        this.keycloak.getUserName();
    }

    public getTokenParsed(): any {
        this.keycloak.getKeycloakInstance().tokenParsed;
    }

    public getToken(): string {
        this.keycloak.getKeycloakInstance().token;
    }

    public getEmployeeId(): string {
        return this.getTokenParsed().prefferd_username;
    }

    public async hasPermission(resourcesId: string, scope: string): Promise<boolean> {
        if (!this.keycloak.isLoggedIn()) {
            return false;
        }

        try {
            return await this.keycloakAuthorization.hasEntitlement(this.resources, resourcesId, scope);
        } catch (error) {
            return false;
        }
    }

    public async getAllAuthorizeResourcesAndScopes() {
        return await this.keycloakAuthorization.getAllAuthorizeResourcesAndScopes(this.resources);
    }
}