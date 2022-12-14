import { Injectable } from "@Angular/core";
import { SecurityService } from "../../../../../../apps/coin/src/app/keycloak/services/security.service";

@Injectable({
    providedIn: 'root'
})
export class ItemSecurityService {
    private readonly resource = "Item_Management";

    private readonly SCOPES = {
        VIEW: "VIEW",
        CREATE: "CREATE",
        UPDATE: "UPDATE",
        DELETE: "DELETE",
        PUBLISH: "PUBLISH"
    }

    constructor(public securityService SecurityService) {}

    public canViewItem$: Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPES.VIEW);
    public canCreateItem$: Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPES.CREATE);
    public canUpdateItem$: Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPES.UPDATE);
    public canDeleteItem$: Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPES.DELETE);
    public canPublishItem$: Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPES.PUBLISH);
}