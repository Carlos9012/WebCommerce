import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NavigationService } from '../services/navigation.service';
import { UtilityService } from '../services/utility.service';
import { Product, Review, SuggestedProducts } from '../models/models';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  imageIndex: number = 1;
  product !: Product;
  reviewControl!: FormGroup;
  showError = false;
  reviewSaved = false;
  otherReviews: Review[] = [];

  constructor(
    private ActivatedRoute: ActivatedRoute,
    private NavigationService: NavigationService,
    public utilityService: UtilityService,
    private fb: FormBuilder
  ) {}
  
  ngOnInit(): void {
    this.reviewControl = this.fb.group({
      review: ['', Validators.required],
      nota: [0, Validators.required]
    })

    this.ActivatedRoute.queryParams.subscribe((params: any) => {
      let id = params.id;
      this.NavigationService.getProduct(id).subscribe((res: any) => {
        this.product = res;
        this.fetchAllReviews();
      })
    });
  }

  submitReview() {
    let review = this.reviewControl.get('review')?.value;
    let nota = this.reviewControl.get('nota')?.value;

    if (review === '' || review === null) {
      this.showError = true;
      return;
    }
  
      let userid = this.utilityService.getUser().id;
      let productid = this.product.id;
      let productCategory = this.product.productCategory.category;
      let productSubcategory= this.product.productCategory.subCategory;
      this.NavigationService
        .submitReview(userid, productid, review, productCategory, productSubcategory, nota)
        .subscribe((res) => {
          this.reviewSaved = true;
          this.fetchAllReviews();
          this.reviewControl.patchValue({
            review: '',
            nota: 0
          });
        });
    }

    fetchAllReviews() {
      this.otherReviews = [];
      this.NavigationService
        .getAllReviewsOfProduct(this.product.id)
        .subscribe((res: any) => {
          console.log(res)
          for (let review of res) {
            this.otherReviews.push(review);
          }
        });
    }

    formatDate(dateString: string): string {
      const months: {[key: string]: string} = {
        'jan.': '01', 'fev.': '02', 'mar.': '03', 'abr.': '04',
        'mai.': '05', 'jun.': '06', 'jul.': '07', 'ago.': '08',
        'set.': '09', 'out.': '10', 'nov.': '11', 'dez.': '12'
      };
    
      const parts = dateString.split(' ');
      const day = parts[2];
      const month = months[parts[1].toLowerCase()];
      const year = parts[3];
    
      return `${day}/${month}/${year}`;
    }
  }
