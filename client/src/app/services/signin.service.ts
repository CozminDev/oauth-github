import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SigninService {

  constructor(private http: HttpClient) { }

  signInGithub<T>(code: string){
    return this.http.get<T>('http://127.0.0.1:5075/api/signin/authorize', { params: { code: code }});
  }
}
