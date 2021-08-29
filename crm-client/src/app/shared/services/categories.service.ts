import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ICategory } from '../interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<ICategory[]> {
    return this.http.get<ICategory[]>('/api/categories');
  }

  getById(id: number): Observable<ICategory> {
    return this.http.get<ICategory>(`/api/categories/${id}`);
  }

  private getModifyCategoryFormDataObject(name: string, image?: File) : FormData {
    const formData = new FormData();

    formData.append('name', name);

    if (image) {
      formData.append('image', image, image.name);
    }

    return formData;
  }

  create(name: string, image?: File) : Observable<ICategory> {
    return this.http.post<ICategory>('/api/categories', this.getModifyCategoryFormDataObject(name, image));
  }

  update(id: number, name: string, image?: File) : Observable<ICategory> {
    return this.http.put<ICategory>(`/api/categories/${id}`, this.getModifyCategoryFormDataObject(name, image));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`/api/categories/${id}`);
  }
}
