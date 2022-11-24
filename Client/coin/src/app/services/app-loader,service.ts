import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
});

export class AppLoaderService {
    public appIsLoading = new BehaviorSubject(false);

    public show() {
        this.appIsLoading.next(true);
    }

    public hide() {
        this.appIsLoading.next(false);
    }
}