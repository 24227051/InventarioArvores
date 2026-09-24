import { CommonModule } from '@angular/common';
import { Component, OnInit  } from '@angular/core';
import { NavigationEnd, Router, RouterModule, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  showMenu = true;

  constructor(private router: Router) {}

  ngOnInit(): void {
    const updateMenu = (): void => {
      const isArvoreDetalhe = this.router.url.startsWith('/arvores/');
      const origem = window.history.state?.origem;

      this.showMenu = !isArvoreDetalhe || origem === 'lista';
    };

    updateMenu();

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => updateMenu());
  }
}
