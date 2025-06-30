import {Component, inject, OnInit} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './layout/header/header'
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { PostSummaryDto, PostsApiResponse } from './interfaces/post.interface';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {

  baseApiUrl = 'http://localhost:5000/api/';
  private http = inject(HttpClient);
  protected title = 'Agora';
  posts: PostSummaryDto[] = [];
  loading = false;
  error: string | null = null;

  ngOnInit(): void {
    this.loadPosts();
  }

  private loadPosts(): void {
    this.loading = true;
    this.error = null;

    console.log('Load posts from ', this.baseApiUrl + 'posts/catalog');

    this.http.get<PostsApiResponse>(this.baseApiUrl + 'posts/catalog').subscribe({
      next: (response) => {
        console.log('Received response:', response);
        this.posts = response.data || (response as any); // If structure is different
        this.loading = false;
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error while loading posts:', error);
        this.error = `Error: ${error.status} - ${error.message}`;
        this.loading = false;
      },
      complete: () => {
        console.log('Post loading completed');
      }
    });
  }

  // Badges for CSS classes

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active': return 'status-active';
      case 'inactive': return 'status-inactive';
      case 'deleted': return 'status-deleted';
      default: return 'status-default';
    }
  }

  getTypeClass(type: string): string {
    switch (type.toLowerCase()) {
      case 'offer': return 'type-offer';
      case 'request': return 'type-request';
      default: return 'type-default';
    }
  }

  getCategoryClass(category: string): string {
    const colors = [
      'category-blue', 'category-green', 'category-purple',
      'category-orange', 'category-pink'
    ];
    const index = category.length % colors.length;
    return colors[index];
  }
}
