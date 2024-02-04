import { Injectable } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-resquest.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }
  
  addCategory(model: AddCategoryRequest): Observable<void> {
    return this.http.post<void>('http://localhost:5201/api/categories', model);
  }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>('http://localhost:5201/api/categories');
  }
}
