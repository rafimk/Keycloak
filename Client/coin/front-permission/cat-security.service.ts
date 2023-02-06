import { Injectable } from '@angular/core';
import { SecurityService } from '../app/coins/app/keycloak/service/security.service';

@Injectable({
    providedIn: 'root'
})
export class CatSecurityService {
    private readonly resource = "CAT_MANAGEMENT";

    private readonly SCOPES = {
        VIEW: "VIEW",
        CREATE: "CREATE",
        UPDATE: "UPDATE",
        DELETE: "DELETE",
        PUBLISH: "PUBLISH"
    }

    constructor(public securityService: SecurityService) {}

    public caanViewCAT$: Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPES.VIEW);
    public caanCreateCAT$: Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPES.CREATE);
}
