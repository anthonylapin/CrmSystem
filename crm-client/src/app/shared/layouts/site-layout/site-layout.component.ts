import {AfterViewInit, Component, ElementRef, ViewChild} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-site-layout',
  templateUrl: './site-layout.component.html',
  styleUrls: ['./site-layout.component.scss'],
})
export class SiteLayoutComponent implements AfterViewInit {
  @ViewChild('floating') floatingRef : ElementRef;

  public showSideBar: boolean = false;
  public sideBarLinks = [
    {url: '/overview', name: 'Overview'},
    {url: '/analytics', name: 'Analytics'},
    {url: '/history', name: 'History'},
    {url: '/add-order', name: 'Add New Order'},
    {url: '/categories', name: 'Assortment'},
  ]

  constructor(private authService: AuthService, private router: Router) {}

  ngAfterViewInit(): void {

  }

  onSideNavToggle() {
    this.showSideBar = !this.showSideBar;
  }

  async logout(event: Event) {
    event.preventDefault();
    this.authService.logout();
    await this.router.navigate(['/login']);
  }
}
