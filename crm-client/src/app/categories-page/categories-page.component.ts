import { Component, OnInit } from '@angular/core';
import { CategoriesService } from '../shared/services/categories.service';
import { ICategory } from '../shared/interfaces';

@Component({
  selector: 'app-categories-page',
  templateUrl: './categories-page.component.html',
  styleUrls: ['./categories-page.component.scss'],
})
export class CategoriesPageComponent implements OnInit {
  public loading: boolean = false;
  public categories: ICategory[] = [];

  constructor(private categoriesService: CategoriesService) {}

  ngOnInit(): void {
    this.loading = true;
    this.categoriesService.getAll().subscribe((categories) => {
      this.loading = false;
      this.categories = categories;
    });
  }
}
