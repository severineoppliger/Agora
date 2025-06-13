import {Component, inject, OnInit} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './layout/header/header'
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {

  baseApiUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  protected title = 'Agora';
  posts: any[] = []

  ngOnInit(): void {
    this.http.get<any>(this.baseApiUrl + 'posts').subscribe({
      next: response => this.posts = response.data,
      error: error => console.log(error),
      complete: () => console.log('complete')
    })
  }
}
