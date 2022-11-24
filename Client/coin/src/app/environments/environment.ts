export const environment = {
    production: false,
    apiUrl: 'http://localhost:5000',
    notificationContext: {
        unreadMessageCount: "http://localhost:5000/api/notifications/unreadMessageCount",
        unreadMessage: "http://localhost:5000/api/notifications/unreadMessage",
    },
    kecloak: {
        config: {
            url: 'http://localhost:8080/auth',
            realm: 'SCM',
            clientId: 'AC-SCM-ITEM-MANAGEMENT-WEB',
        },
        loadUserProfileAtStartUp: false,
        initOptions: {
            onLoad: 'login-required',
            checkLoginIframe: false,
            pkceMethod: 'S256'
        },
        bearerExcludedUrls: []
    },
    resourceServiceId: 'AC-SCM-ITEM-MANAGEMENT-SERVICE'
};
