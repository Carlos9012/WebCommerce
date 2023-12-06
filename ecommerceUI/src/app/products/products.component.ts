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
  destaque: Product = {
    id: 1,
    title: '',
    description_: '',
    price: 0,
    quantity: 0,
    productCategory: {
      id: 1,
      category: '',
      subCategory: '',
    },
    offer: {
      id: 1,
      title: '',
      discount: 0,
    },
    imageName: '',
    nota: 0
  };

  BestOffer: Product = {
    id: 1,
    title: '',
    description_: '',
    price: 0,
    quantity: 0,
    productCategory: {
      id: 1,
      category: '',
      subCategory: '',
    },
    offer: {
      id: 1,
      title: '',
      discount: 0,
    },
    imageName: '',
    nota: 0
  };

  constructor(
    private ActivatedRoute: ActivatedRoute,
    private NavigationService: NavigationService,
    public utilityService: UtilityService
  ) {}
  ngOnInit(): void {
    this.ActivatedRoute.queryParams.subscribe((params: any) => {
      let category = params.category;
      let subcategory = params.subcategory;

      //get Min Price Product
      this.NavigationService.getMinProduct(category, subcategory).subscribe(
        (res: any) => {
          console.log(res);
          this.destaque = res;
        }
      )

      //get Max Offer 
      this.NavigationService.getMaxOpffer(category, subcategory).subscribe(
        (res: any) => {
          console.log(res);
          this.BestOffer = res;
        }
      )

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
