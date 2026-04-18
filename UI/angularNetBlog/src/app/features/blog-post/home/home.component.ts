import { Component, OnInit } from '@angular/core';
import { Observable, map } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: false
})
export class HomeComponent implements OnInit {
  posts$?: Observable<BlogPost[]>;

  constructor(private blogPostService: BlogPostService) {}

  ngOnInit(): void {
    this.posts$ = this.blogPostService.getAllBlogPosts().pipe(
      map(posts => posts.filter(post => post.isVisible))
    );
  }
}
