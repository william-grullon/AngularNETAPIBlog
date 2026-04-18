import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { CreateBlogPostRequest } from '../models/create-blog-post-request.model';

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html',
  styleUrls: ['./add-post.component.css']
})
export class AddPostComponent implements OnDestroy {
  model: CreateBlogPostRequest;
  private addPostSubscription?: Subscription;

  constructor(
    private blogPostService: BlogPostService,
    private router: Router
  ) {
    this.model = {
      title: '',
      shortDescription: '',
      content: '',
      featureImageUrl: '',
      urlHandle: '',
      publishedDate: new Date().toISOString().slice(0, 16),
      author: 'William Grullon',
      isVisible: true
    };
  }

  onFormSubmit(): void {
    this.addPostSubscription = this.blogPostService.addBlogPost(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/admin/posts');
      }
    });
  }

  ngOnDestroy(): void {
    this.addPostSubscription?.unsubscribe();
  }
}
