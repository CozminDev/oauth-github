import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { filter, map, mergeMap, throwError } from 'rxjs';
import { SigninService } from '../services/signin.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  user: User | null = null;

  constructor(
    private route: ActivatedRoute,
    private signInService: SigninService
  ) {}

  ngOnInit(): void {
    this.route.data
      .pipe(
        filter((d) => d['success']),
        mergeMap(() => {
          return this.route.queryParamMap.pipe(
            map((params) => params.get('code'))
          );
        }),
        mergeMap((code) => {
          if (!code) return throwError(() => new Error(`Invalid code ${code}`));

          return this.signInService.signInGithub<User>(code);
        })
      )
      .subscribe((user) => {
        this.user = user;
      });
  }

  signIn() {
    window.location.href =
      'https://github.com/login/oauth/authorize?client_id=fb946a02836c5a4c7416';
  }
}

export interface User {
  login: string;
}
