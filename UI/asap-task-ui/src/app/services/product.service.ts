import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
}

export interface ProductListResponse {
  result: {
    items: Product[];
    totalCount: number;
  };
}

@Injectable({
  providedIn: 'root',
})


export class ProductService {

  private apiUrl = 'https://localhost:7033/api';

  constructor(private http: HttpClient) {}

  ggetAllProducts(page: number, pageSize: number): Observable<ProductListResponse> {
    // Convert to numbers explicitly
    const pageNumber = page || 1;
    const pageSizeNumber = pageSize || 10;

    // Validate numbers
    if (isNaN(pageNumber) || isNaN(pageSizeNumber)) {
      throw new Error('Invalid pagination parameters');
    }

    const params = new HttpParams()
      .set('page', Math.max(1, pageNumber).toString())
      .set('pageSize', Math.max(1, Math.min(100, pageSizeNumber)).toString());

    return this.http.get<ProductListResponse>(this.apiUrl+"/Products", { params });
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl+"/Products"}/${id}`);
  }


  createProduct(product: Omit<Product, 'id'>): Observable<Product> {
    return this.http.post<Product>(this.apiUrl+"/Products", product);
  }

  updateProduct(product: Product): Observable<void> {
    return this.http.put<void>(`${this.apiUrl+"/Products"}/${product.id}`, product);
  }

}
