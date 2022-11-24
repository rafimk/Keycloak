import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';

export interface IResource {
    rsid: string;
    rsname: string;
    scopes: string[];
}

@Injectable({
    providerIn: 'toot'
});

export class KeycloakAuthorizationService {
    private config: any;
    private clientId: string = '';
    constructor(private keycloakService: KeycloakService, private httpClient: HttpClient) {
        this.getUMA2Config();
    }

    private getUMA2Config() {
        if (!this.config) {
            const keycloak = this.keycloakService.getKeycloakInstance();
            this.clientId = keycloak.clientId || '';

            this.config = await this.httpClient.get(`${keycloak.authServerUrl}/realms/${keycloak.realm}/.well-known/uma2-configuration`).toPromise();
        }
    }

    public async hasEntitlement(resource: string, resourceId: string, scope: string): Promise<boolean> {
        if (!this.config) {
            await this.wait();
            return this.hasEntitlement(resource, resourceId, scope);
        }

        const body = new URLSearchParams();
        body.set('grant_type', 'urn:ietf:params:oauth:grant-type:uma-ticket');
        body.set('client_id', this.clientId);
        body.set('permission', `${resourceId}#${scope}`);
        body.set('audience', this.clientId);
        body.set('response_mode', 'decision');

        try {
            const response = await this.httpClient.post(`${this.config.token_endpoint}`, body.toString(), {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }).toPromise();

            return response.result;
        } catch (error) {
            return false;
        }
        
    }
    public async getAuthorizationResourcesAnd(resource: string): Promise<IResource[]> {
        if(!this.config) {
            await this.wait();
            return this.getAuthorizationResourcesAnd(resource);
        }

        const body = new URLSearchParams();
        body.set('grant_type', 'urn:ietf:params:oauth:grant-type:uma-ticket');
        body.set('client_id', this.clientId);
        body.set('audience', resource);
        body.set('response_mode', 'permissions');

        try {
            const response = await this.httpClient.post(`${this.config.token_endpoint}`, body.toString(), {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }).toPromise();

            return response.permissions;
        } catch (error) {
            return [];
        }
    }

    public await(ms = 500) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}