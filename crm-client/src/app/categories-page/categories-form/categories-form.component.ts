import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { CategoriesService } from '../../shared/services/categories.service';
import { UiService } from '../../shared/utilities/ui.service';
import { ICategory } from '../../shared/interfaces';
import { ToastService } from '../../shared/services/toast.service';

@Component({
  selector: 'app-categories-form',
  templateUrl: './categories-form.component.html',
  styleUrls: ['./categories-form.component.scss'],
})
export class CategoriesFormComponent implements OnInit {
  public isNew: boolean = true;
  public categoryId: number | null;
  public form: FormGroup;
  private image?: File;
  public imagePreview: string | ArrayBuffer | null = '';

  constructor(
    private route: ActivatedRoute,
    private categoriesService: CategoriesService,
    private router: Router,
    private toastService: ToastService
  ) {}

  private initializeForm(category: ICategory | null) {
    if (category) {
      this.categoryId = category.id;

      this.form.patchValue({
        name: category.name,
      });

      this.imagePreview = category.imageSource ?? null;
    }
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(null, Validators.required),
    });

    this.form.disable();

    this.route.params
      .pipe(
        switchMap((params: Params) => {
          this.isNew = !Boolean(params['id']);
          return this.isNew ? of(null) : this.categoriesService.getById(params['id']);
        })
      )
      .subscribe(
        (category) => {
          this.initializeForm(category);
          this.form.enable();
        },
        (error) => this.toastService.showDanger(error.error.message)
      );
  }

  onFileUpload(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files) {
      this.image = target.files[0];

      const reader = new FileReader();

      reader.onload = () => {
        this.imagePreview = reader.result;
      };

      reader.readAsDataURL(this.image);
    }
  }

  onSubmit() {
    let obs$;
    this.form.disable();

    if (this.categoryId) {
      obs$ = this.categoriesService.update(this.categoryId, this.form.value.name, this.image);
    } else {
      obs$ = this.categoriesService.create(this.form.value.name, this.image);
    }

    obs$.subscribe(
      async (category) => {
        if (this.isNew) {
          this.toastService.show('Category was created successfully!!');
          await this.router.navigate([`/categories/${category.id}`]);
        } else {
          this.initializeForm(category);
          this.form.enable();
          this.toastService.show('Changes were saved successfully!');
        }
      },
      (error) => {
        this.toastService.showDanger(error.error.message);
        this.form.enable();
      }
    );
  }

  deleteCategory() {
    if (UiService.confirmMessage('Are you sure you want to delete this category?') && this.categoryId) {
      this.categoriesService.delete(this.categoryId).subscribe(
        () => {
          this.toastService.show('Category was deleted successfully!');
        },
        (error) => {
          this.toastService.showDanger(error.error.message);
        },
        () => this.router.navigate(['/categories'])
      );
    }
  }
}
