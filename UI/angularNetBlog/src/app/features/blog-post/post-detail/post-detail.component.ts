import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { BlogPostService } from '../services/blog-post.service';

@Component({
    selector: 'app-post-detail',
    templateUrl: './post-detail.component.html',
    styleUrls: ['./post-detail.component.css'],
    standalone: false
})
export class PostDetailComponent implements OnInit, OnDestroy {
  post?: BlogPost;
  private routeSubscription?: Subscription;
  private postSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private blogPostService: BlogPostService
  ) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe(params => {
      const urlHandle = params.get('urlHandle');

      if (!urlHandle) {
        return;
      }

      this.postSubscription?.unsubscribe();
      this.postSubscription = this.blogPostService.getBlogPostByUrlHandle(urlHandle).subscribe({
        next: post => {
          this.post = post;
        },
        error: () => {
          this.post = undefined;
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.postSubscription?.unsubscribe();
  }
}
