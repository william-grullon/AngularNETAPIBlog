import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})

export class EditCategoryComponent implements OnInit, OnDestroy {

  id: string | null = null;
  paramsSubscription?: Subscription;  
  updateCategorySubscription?: Subscription;
  category?: Category;

  constructor(private route: ActivatedRoute,
    private categoryService: CategoryService,
    private router: Router) {     
  }

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if (this.id) {
          // Fetch category by id
          this.categoryService.getCategoryById(this.id).subscribe({
            next: (reponse) => {
              this.category = reponse;
            },
            error: (error) => {
              console.error('Error fetching category by id', error);
            }
          });
        }
      }
    });
  }

  onFormSubmit(): void {
    const UpdateCategoryRequest: UpdateCategoryRequest = { 
      name: this.category?.name || '',
      urlHandle: this.category?.urlHandle || ''
    };

    // pass the UpdateCategoryRequest to the service
    if (this.id) {
      this.updateCategorySubscription = this.categoryService.updateCategory(this.id, UpdateCategoryRequest).subscribe({
        next: (response) => {
          console.log('Category updated successfully', response);
          this.router.navigateByUrl('/admin/categories');
        },
        error: (error) => {
          console.error('Error updating category', error);
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.updateCategorySubscription?.unsubscribe();
  }

}
