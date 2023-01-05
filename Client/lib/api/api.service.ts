import { Inectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpHeaders, HttpParms, HttpRequest } from '@angular/common/http';
import { environment } from '@env/environment';

@Injectable({
    providedIn: 'root'
})
export class ApiService {
    constructor(private http: HttpClient) {

    }

    openServerUrl(url: string) {
        window.open(`${environment.apiUrl}`);
    }

    get(url: string, parms: HttpParms = new HttpParms()): Observable<any> {
        return this.http.get(`${environment.apiUrl}${url}`, {
            headers: this.headers,
            withCredentials: true,
            parms
        });
    }

    post(url: string, data: Object = {}): Observable<any> {
        return this.http.post(`${environment.apiUrl}${url}`, data,  {
            headers: this.headers,
            withCredentials: true,
        })
    }

    postWithImage(url: string, data: FromData): Observable<any> {
        return this.http.request(new HttpRequest(
            'POST',
            `${environment.apiUrl}${url}`,
            data,
            {
                reportProgress: true
            }
        ));
    } 

    put(url: string, data: Object = {}): Observable<any> {
        return this.http.put(`${environment.apiUrl}${url}`, {
            headers: this.headers,
            withCredentials: true,
        });
    }

    delete(url: string): Observable<any> {
        return this.http.delete(`${environment.apiUrl}${url}`, {
            headers: this.headers,
            withCredentials: true,
        });
    }

    get headers(): HttpHeaders {
        const headersConfig = {
            'Content-Type': 'application/json',
            Accept: 'application/json',
            Pragma: 'no-cache'
        };

        return new HttpHeaders(headersConfig);
    }
    
    getWithImage(url: string): Observable<HttpEvent<any>> {
        return this.http.request(new HttpRequest(
            'GET',
            `${environment.apiUrl}${url}`,
            null,
            {
                reportProgress: true,
                responseType: 'blob'
            }
        ));
    }

    getAsImage(url: string) {
        return this.http.get(`${environment.apiUrl}${url}`)
    }

    getImage(url: string): Observable<any> {
        return this.http.get(`${environment.apiUrl}${url}`);
    }
}