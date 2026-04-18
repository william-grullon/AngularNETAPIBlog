import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryListComponent } from './features/category/category-list/category-list.component';
import { AddCategoryComponent } from './features/category/add-category/add-category.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category.component';
import { HomeComponent } from './features/blog-post/home/home.component';
import { PostDetailComponent } from './features/blog-post/post-detail/post-detail.component';
import { AdminPostListComponent } from './features/blog-post/admin-post-list/admin-post-list.component';
import { AddPostComponent } from './features/blog-post/add-post/add-post.component';
import { EditPostComponent } from './features/blog-post/edit-post/edit-post.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: HomeComponent
  },
  {
    path: 'blog/:urlHandle',
    component: PostDetailComponent
  },
  {
    path: 'admin/categories',
    component: CategoryListComponent
  },
  {
    path: 'admin/categories/add',
    component: AddCategoryComponent
  },
  {
    path: 'admin/categories/edit/:id',
    component: EditCategoryComponent
  },
  {
    path: 'admin/posts',
    component: AdminPostListComponent
  },
  {
    path: 'admin/posts/add',
    component: AddPostComponent
  },
  {
    path: 'admin/posts/edit/:id',
    component: EditPostComponent
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
