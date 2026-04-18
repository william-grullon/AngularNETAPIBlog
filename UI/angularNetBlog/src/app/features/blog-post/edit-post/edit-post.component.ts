import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { BlogPostService } from '../services/blog-post.service';
import { UpdateBlogPostRequest } from '../models/update-blog-post-request.model';

@Component({
  selector: 'app-edit-post',
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.css']
})
export class EditPostComponent implements OnInit, OnDestroy {
  id: string | null = null;
  post?: BlogPost;
  private routeSubscription?: Subscription;
  private postSubscription?: Subscription;
  private updatePostSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private blogPostService: BlogPostService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe(params => {
      this.id = params.get('id');

      if (!this.id) {
        return;
      }

      this.postSubscription?.unsubscribe();
      this.postSubscription = this.blogPostService.getBlogPostById(this.id).subscribe({
        next: post => {
          this.post = {
            ...post,
            publishedDate: this.toDatetimeLocalValue(post.publishedDate)
          };
        }
      });
    });
  }

  onFormSubmit(): void {
    if (!this.id || !this.post) {
      return;
    }

    const request: UpdateBlogPostRequest = {
      title: this.post.title,
      shortDescription: this.post.shortDescription,
      content: this.post.content,
      featureImageUrl: this.post.featureImageUrl,
      urlHandle: this.post.urlHandle,
      publishedDate: this.post.publishedDate,
      author: this.post.author,
      isVisible: this.post.isVisible
    };

    this.updatePostSubscription = this.blogPostService.updateBlogPost(this.id, request).subscribe({
      next: () => {
        this.router.navigateByUrl('/admin/posts');
      }
    });
  }

  onDelete(): void {
    if (!this.id) {
      return;
    }

    this.blogPostService.deleteBlogPost(this.id).subscribe({
      next: () => {
        this.router.navigateByUrl('/admin/posts');
      }
    });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.postSubscription?.unsubscribe();
    this.updatePostSubscription?.unsubscribe();
  }

  private toDatetimeLocalValue(value: string): string {
    const date = new Date(value);
    const pad = (input: number) => String(input).padStart(2, '0');
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
  }
}
