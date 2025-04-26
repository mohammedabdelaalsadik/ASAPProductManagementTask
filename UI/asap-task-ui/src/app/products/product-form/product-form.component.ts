// product-form.component.ts
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Product } from '../../services/product.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.scss']
})
export class ProductFormComponent {
  @Input() product?: Product;
  @Input() mode?: string;

  @Output() formSubmit = new EventEmitter<Product>();
  @Output() cancel = new EventEmitter<void>();

  productForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(0)]]
    });
  }
  ngOnInit() {
    if (this.mode === 'edit' && this.product) {
      this.productForm.patchValue(this.product);
    }
  }


  onSubmit() {
    if (this.productForm.valid) {
      const formValue = this.productForm.value;
      const productData: Product = {
        ...this.product, // Keep existing id if editing
        ...formValue
      };
      this.formSubmit.emit(productData);
    }
  }

  onCancel() {
    this.cancel.emit();
  }
}
