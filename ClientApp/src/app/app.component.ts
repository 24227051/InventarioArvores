import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { MenuService } from './services/menu.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  showMenu = true;
  private sub?: Subscription;

  constructor(private menu: MenuService) { }

  ngOnInit(): void {
    this.sub = this.menu.showMenu$.subscribe(v => this.showMenu = v);
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }
}
