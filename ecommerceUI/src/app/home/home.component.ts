import { Component, OnInit } from '@angular/core';
import { SuggestedProducts } from '../models/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
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


  ngOnInit(): void {}
}
