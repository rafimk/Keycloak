import { environment } from '../../environments/environment';
import { KeycloakService, KeycloakOption } from 'keycloak-angular';

export default function initializeKeycloak(keycloak: KeycloakService): () => Promise<void> {
    return async () => {
        await keycloak.init(environment.kecloak as KeycloakOption);
    };
}