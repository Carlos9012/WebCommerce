import { Directive, HostListener, Input } from '@angular/core';
import { Router } from '@angular/router';

@Directive({
  selector: '[OpenProductDetails]'
})
export class OpenProductsDetailsDirective {
  @Input() productId: number = 0;
  @HostListener('click') openProductsDetails() {
    window.scroll(0, 0);
    this.router.navigate(['/product-details'], {
      queryParams: {
        id: this.productId,
      }
    });
  }
  constructor(private router: Router) { }

}
