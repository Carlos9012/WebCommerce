import { Component, OnInit } from '@angular/core';
import { UtilityService } from '../services/utility.service';
import { ActivatedRoute } from '@angular/router';
import { NavigationService } from '../services/navigation.service';
import { Product } from '../models/models';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  view: 'grid' | 'list' = 'list';
  sortBy: 'default' | 'htl' | 'lth' = 'default';
  products: Product[] = [];

  constructor(
    private ActivatedRoute: ActivatedRoute,
    private NavigationService: NavigationService,
    private utilityService: UtilityService
  ) {}
  ngOnInit(): void {
    this.ActivatedRoute.queryParams.subscribe((params: any) => {
      let category = params.category;
      let subcategory = params.subcategory;

      if(category && subcategory)
        this.NavigationService
         .getProducts(category, subcategory, 10)
         .subscribe((res: any) => {
          this.products = res;
         });
      }
    );
  }

  sortByPrice(sotkey: string){
    this.products.sort((a, b) =>{
      if (sotkey === 'default') {
        return a.id > b.id ? 1 : -1;
      }
      if (sotkey === 'htl') {
        return this.utilityService.applyDiscount(a.price, a.offer.discount) > 
        this.utilityService.applyDiscount(b.price, b.offer.discount)
          ? -1
          : 1;
      }
      if (sotkey === 'lth') {
        return this.utilityService.applyDiscount(a.price, a.offer.discount) > 
        this.utilityService.applyDiscount(b.price, b.offer.discount)
          ? 1
          : -1;
      }
      return 0;
    });
  }
}
