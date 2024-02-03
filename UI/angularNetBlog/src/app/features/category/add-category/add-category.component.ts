import { Component, OnDestroy } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-resquest.model';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnDestroy {

  model: AddCategoryRequest;
  private addCategorySubscription?: Subscription

  constructor(private categoryService: CategoryService) {
    this.model = {
      name: '',
      urlHandle: ''
    };
  }

  onFormSubmit() {
    this.addCategorySubscription = this.categoryService.addCategory(this.model)
      .subscribe({
        next: (response) => {
          console.log('Category added');
        },
        error: (error) => {
          console.error('There was an error!', error);
        }
      });
    };
    
    ngOnDestroy(): void {
      this.addCategorySubscription?.unsubscribe();
    }
}
