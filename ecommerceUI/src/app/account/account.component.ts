import { Component, OnInit } from '@angular/core';
import { NavigationService } from '../services/navigation.service';
import { UtilityService } from '../services/utility.service';
import { User } from '../models/models';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  account?: User;
  user!: User;

  constructor(
    private navigationService: NavigationService,
    public utilityService: UtilityService
  ) {}

  ngOnInit() {
    this.user = this.utilityService.getUser();
    this.navigationService
      .getUser(this.user.id)
      .subscribe((res: any) => {
        this.account = res;
      })
  }
}
