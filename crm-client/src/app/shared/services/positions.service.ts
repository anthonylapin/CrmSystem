import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPosition } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class PositionsService {
  constructor(private http: HttpClient) {}

  getAll(categoryId: number): Observable<IPosition[]> {
    return this.http.get<IPosition[]>(`/api/categories/${categoryId}/positions`);
  }

  create(position: IPosition): Observable<IPosition> {
    return this.http.post<IPosition>(`/api/categories/${position.categoryId}/positions`, position);
  }

  update(position: IPosition): Observable<IPosition> {
    return this.http.put<IPosition>(`/api/categories/${position.categoryId}/positions/${position.id}`, position);
  }

  delete(position: IPosition): Observable<void> {
    return this.http.delete<void>(`/api/categories/${position.categoryId}/positions/${position.id}`);
  }
}
