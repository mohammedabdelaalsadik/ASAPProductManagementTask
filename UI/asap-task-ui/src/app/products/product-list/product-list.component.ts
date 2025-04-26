import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DataStateChangeEvent, GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { DialogModule, DialogService } from '@progress/kendo-angular-dialog';
import { ProductService, Product } from '../../services/product.service';
import { GridModule } from '@progress/kendo-angular-grid';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { LabelModule } from '@progress/kendo-angular-label';
import { ProductFormComponent } from '../product-form/product-form.component';
import { CompositeFilterDescriptor, filterBy, State } from '@progress/kendo-data-query';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    GridModule,
    DialogModule,
    ButtonsModule,
    InputsModule,
    LabelModule,
    ReactiveFormsModule,

  ],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {

  public filter: CompositeFilterDescriptor = { logic: 'and', filters: [] };
  public gridState: State = {
    skip: 0,
    take: 10,
    filter: this.filter
  };

  public gridData: GridDataResult = { data: [], total: 0 };
  public pageSize = 10;
  public skip = 0;

  public productForm: FormGroup;
  public formTitle: string = '';
  public isFormVisible = false;
  public editMode = false;
  public selectedProduct: Product | null = null;

  constructor(
    private productService: ProductService,
    private formBuilder: FormBuilder,
    private dialogService: DialogService,
  ) {
    this.productForm = this.formBuilder.group({
      id: [null],
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0.01)]]
    });
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  public dataStateChange(state: DataStateChangeEvent): void {
    this.gridState = state;
    this.skip = state.skip || 0;

    // Apply client-side filtering
    if (state.filter) {
      this.filter = state.filter;
      this.applyFilter();
    } else {
      this.loadProducts();
    }
  }
  private applyFilter(): void {
    if (this.filter.filters && this.filter.filters.length > 0) {
      this.gridData.data = filterBy(this.gridData.data, this.filter);
    } else {
      this.loadProducts();
    }
  }


  public openCreateForm(): void {
    const dialogRef = this.dialogService.open({
      title: 'Create New Product',
      content: ProductFormComponent,
      width: 600,
      height: 550
    });

    const formComponent = dialogRef.content.instance as ProductFormComponent;


    formComponent.formSubmit.subscribe((product: Product) => {
      this.productService.createProduct(product).subscribe({
        next: () => {
          this.loadProducts();
          dialogRef.close();
        },
        error: (error) => {
          console.error('Error creating product', error);
        }
      });
    });


    formComponent.cancel.subscribe(() => {
      dialogRef.close();
    });
  }
  public loadProducts(): void {
    const page = this.skip / this.pageSize + 1;
    this.productService.ggetAllProducts(page, this.pageSize).subscribe({
      next: (response) => {
        this.gridData = {
          data: response.result.items,
          total: response.result.totalCount
        };
      },
      error: (error) => {
        console.error('Error loading products', error);
      }
    });
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadProducts();
  }


public openEditDialog(product: Product): void {
  const dialogRef = this.dialogService.open({
    //title: Edit Product: ${product.name},
    content: ProductFormComponent,
    width: 600,
    height: 550
  });

  // Get the content component instance
  const contentComponent = dialogRef.content.instance as ProductFormComponent;

  // Set input properties
  contentComponent.mode = 'edit';
  contentComponent.product = product;
  // Handle form submission
  const formSub = contentComponent.formSubmit.subscribe((updatedProduct: Product) => {
    this.productService.updateProduct(updatedProduct).subscribe({
      next: () => {
        this.loadProducts();
        dialogRef.close();
      },
      error: (error) => {
        console.error('Error updating product', error);
      }
    });
  });

  // Handle cancel
  const cancelSub = contentComponent.cancel.subscribe(() => {
    dialogRef.close();
  });

  // Clean up subscriptions when dialog closes
  dialogRef.result.subscribe(() => {
    formSub.unsubscribe();
    cancelSub.unsubscribe();
  });
}

  

  public confirmDelete(id: number): void {
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.loadProducts();
      },
      error: (error) => {
        console.error('Error deleting product', error);
      }
    });
  }

  public cancelForm(): void {
    this.isFormVisible = false;
    this.productForm.reset();
    this.selectedProduct = null;
  }




 }
