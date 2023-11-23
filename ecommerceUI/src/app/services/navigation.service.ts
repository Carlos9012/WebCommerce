import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs';
import { Category, Order, Payment, PaymentMethod, User, } from '../models/models';
@Injectable({
  providedIn: 'root'
})
export class NavigationService {
  baseUrl = "https://localhost:7108/api/Shopping/";

  constructor(private http: HttpClient) { }

  getCategoryList() {
    let url = this.baseUrl + 'GetCategoryList';
    return this.http.get<any[]>(url).pipe(
      map((categories) => 
      categories.map((category) => {
        let mappedCategory: Category = {
          id: category.id,
          category: category.category,
          subCategory: category.subCategory
        };
        return mappedCategory;
      })
      )
    )
  }

  getProducts(category: string, subcategory: string, count: number) {
    return this.http.get<any[]>(this.baseUrl + 'GetProducts', {
      params: new HttpParams()
        .set('category', category)
        .set('subcategory', subcategory)
        .set('count', count),
    });

  }

  getProduct(id: number) {
    let url = this.baseUrl + "GetProduct/" + id;
    console.log(url);
    return this.http.get(url);
  }

  registerUser(user: User) {
    let url = this.baseUrl + "RegisterUser";
    return this.http.post(url, user, {responseType: 'text'});
  }

  loginUser(email: String, password: String) {
    let url = this.baseUrl +  "LoginUser";
    return this.http.post(
      url, 
      { Email: email, Password: password },
      { responseType: 'text' }
    );
  }

  submitReview(userid: number, productid: number, review: string, category: string, subcategory: string, nota: number) {
    let obj: any = {
      User: {
        Id: userid,
      },
      Product: {
        Id: productid,
        ProductCategory: {  // Incluindo o objeto ProductCategory
          Category: category,
          SubCategory: subcategory
        }
      },
      Value: review,
      nota: nota,
    };

    let url = this.baseUrl + 'InsertReview';
    return this.http.post(url, obj, { responseType: 'text' });
  }

  getAllReviewsOfProduct(productId: number) {
    let url = this.baseUrl + 'GetproductReview/' + productId;
    return this.http.get(url);
  }

  addToCart(userid: number, productid: number) {
    let url = 
      this.baseUrl + 'InsertCartItem/' + userid + '/' + productid;
    return this.http.post(url, null, { responseType: 'text' });
  }

  getActiveCartOfUser(userid: number) {
    let url = this.baseUrl + 'GetActiveCartOfUser/' + userid;
    return this.http.get(url);
  }

  getAllPreviousCarts(userid: number) {
    let url = this.baseUrl + 'GetAllPreviousCartsOfUser/' + userid;
    return this.http.get(url);
  }

  getPaymentMethods() {
    let url = this.baseUrl + "GetPaymentMethods";
    return this.http.get<PaymentMethod[]>(url);
  }

  insertPayment(payment: Payment) {
    return this.http.post(this.baseUrl + "InsertPayment", payment, {
      responseType: 'text',
    });
  }

  insertOrder(order: Order) {
    return this.http.post(this.baseUrl + 'InsertOrder', order );
  }

  getUser(userId: number) {
    let url = this.baseUrl + 'GetUser?id=' + userId;
    return this.http.get(url);
  }
 }
