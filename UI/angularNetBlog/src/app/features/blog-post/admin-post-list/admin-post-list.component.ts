import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { BlogPostService } from '../services/blog-post.service';

@Component({
    selector: 'app-admin-post-list',
    templateUrl: './admin-post-list.component.html',
    styleUrls: ['./admin-post-list.component.css'],
    standalone: false
})
export class AdminPostListComponent implements OnInit {
  posts$?: Observable<BlogPost[]>;

  constructor(private blogPostService: BlogPostService) {}

  ngOnInit(): void {
    this.posts$ = this.blogPostService.getAllBlogPosts();
  }

  deletePost(id: string): void {
    this.blogPostService.deleteBlogPost(id).subscribe({
      next: () => {
        this.posts$ = this.blogPostService.getAllBlogPosts();
      }
    });
  }
}
