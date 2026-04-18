import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { CreateBlogPostRequest } from '../models/create-blog-post-request.model';

@Component({
    selector: 'app-add-post',
    templateUrl: './add-post.component.html',
    styleUrls: ['./add-post.component.css'],
    standalone: false
})
export class AddPostComponent implements OnDestroy {
  model: CreateBlogPostRequest;
  private addPostSubscription?: Subscription;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private blogPostService: BlogPostService
  ) {
    this.model = this.createEmptyModel();
  }

  onFormSubmit(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.isSubmitting = true;

    this.addPostSubscription = this.blogPostService.addBlogPost(this.model).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.successMessage = 'Post created successfully. You can add another post or go back to the list.';
        this.model = this.createEmptyModel();
      },
      error: (error) => {
        console.error('Error creating blog post', error);
        const apiMessage =
          error?.error?.message ||
          error?.error?.title ||
          error?.message ||
          'The API rejected the request.';
        this.errorMessage = `We could not save the post. ${apiMessage}`;
        this.isSubmitting = false;
      }
    });
  }

  ngOnDestroy(): void {
    this.addPostSubscription?.unsubscribe();
  }

  private createEmptyModel(): CreateBlogPostRequest {
    const localDateTime = new Date();
    const offsetMinutes = localDateTime.getTimezoneOffset();
    const localDate = new Date(localDateTime.getTime() - offsetMinutes * 60_000);

    return {
      title: '',
      shortDescription: '',
      content: '',
      featureImageUrl: '',
      urlHandle: '',
      publishedDate: localDate.toISOString().slice(0, 16),
      author: 'William Grullon',
      isVisible: true
    };
  }
}
