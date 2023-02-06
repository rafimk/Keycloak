Import { Injectable } from '@angular/core';
import { SecurityService } from 'coin/src/app/keycloak/services/security.service';
@Injectable({
  providedIn: 'root'
})
export class CapSecurityService {

  private readonly resource = "Cap_Managment"

  private readonly SCOPES = {
    VIEW: "VIEW",
    CREATE: "CREATE",
    UPDATE: "UPDATE",
    DELETE: "DELETE",
    PUBLISH: "PUBLISH"
  }

constructure (public securityService: SecurityService) {}

public canViewCAP$ = Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPE.VIEW)
public canCreateCAP$ = Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPE.CREATE)
public canUpdateCAP$ = Promise<boolean> = this.securityService.hasPermission(this.resource, this.SCOPE.UPDATE)

}
