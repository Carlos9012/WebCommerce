import { Component, OnInit } from '@angular/core';
import { Product, SuggestedProducts } from '../models/models';
import { NavigationService } from '../services/navigation.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  category:string = 'eletronics';
  suggestedProducts: SuggestedProducts[] = [
    {
      banerimage: 'Baner/Baner_Mobile.png',
      category: {
        id: 0,
        category: 'eletronics',
        subCategory: 'mobile',
      },
    },
    {
      banerimage: 'Baner/Baner_Laptop.png',
      category: {
        id: 1,
        category: 'eletronics',
        subCategory: 'laptops',
      },
    },
    {
      banerimage: 'Baner/Baner_Chair.png',
      category: {
        id: 0,
        category: 'furniture',
        subCategory: 'chairs',
      },
    },
  ];

  products: Product[] = [];
  constructor(private navigationService: NavigationService) {}

  ngOnInit(): void {
    this.navigationService.getDistinctProducts()
      .subscribe((res: any[]) => {
      for (let product of res) {
        this.products.push(product);
      }
    });
  }
}
