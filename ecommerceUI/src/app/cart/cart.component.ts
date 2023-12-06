import { Component, OnInit } from '@angular/core';
import { NavigationService } from '../services/navigation.service';
import { UtilityService } from '../services/utility.service';
import { Cart } from '../models/models';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  previousPrice: number = 0;
  usersCart: Cart = {
    id: 0,
    user: this.utilityService.getUser(),
    cartItems: [],
    ordered: false,
    orderedOn: '',
  };

  usersPaymentInfo: any = {
    id: 0,
    user: this.utilityService.getUser(),
    paymetMethod: {
      id: 0,
      type: '',
      provider: '',
      available: false,
      reason: '',
    },
    totalAmount: 0,
    shipingCharges: 0,
    amountReduced: 0,
    amountPaid: 0,
    createdAt: '',
  };

  usersPreviousCart: Cart[] = [];
  constructor(
    public utilityService: UtilityService,
    private navigationService: NavigationService
  ) {}

  ngOnInit(): void {
    //get cart
    this.navigationService
      .getActiveCartOfUser(this.utilityService.getUser().id)
      .subscribe((res: any) => {
        this.usersCart = res;
        console.log(res);
        // get previous card value
        this.navigationService.getPreviousValue(this.usersCart.id).subscribe((res: any) => {
          this.previousPrice = res
        })
        //calcule payment
        this.utilityService.calculatePayment(
          this.usersCart,
          this.usersPaymentInfo
        );
      });    

    // get previous carts
    this.navigationService
      .getAllPreviousCarts(this.utilityService.getUser().id)
      .subscribe((res: any) => {
        console.log(res);
        this.usersPreviousCart = res;
      })
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
