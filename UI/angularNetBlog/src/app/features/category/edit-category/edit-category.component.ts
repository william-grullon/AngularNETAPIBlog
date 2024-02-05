import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category.model';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit, OnDestroy {

  id: string | null = null;
  paramsSubscription?: Subscription;
  category?: Category;

  constructor(private route: ActivatedRoute,
    private categoryService: CategoryService) {     
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
    console.log(this.category, 'Category updated successfully');
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
  }

}
