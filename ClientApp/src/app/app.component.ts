import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy, Inject, PLATFORM_ID } from '@angular/core'; // Verifique se o OnDestroy está importado aqui
import { RouterOutlet, RouterLink, Router, NavigationEnd } from '@angular/router';
import { MenuService } from './services/menu.service';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { HeaderAcessibilidadeComponent } from './components/header-acessibilidade/header-acessibilidade.component';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, HeaderAcessibilidadeComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
// Garanta o "implements OnDestroy" aqui na classe
export class AppComponent implements OnInit, OnDestroy { 
  title = 'ClientApp';
  showMenu = true;
  private sub?: Subscription;
  private routerSub?: Subscription; // Inscrição do roteador controlada

  constructor(
    private menu: MenuService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) { }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.inicializarVLibras();
    }

    this.sub = this.menu.showMenu$.subscribe(v => this.showMenu = v);

    this.routerSub = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      setTimeout(() => {
        const mainContent = document.querySelector('main') || document.getElementById('conteudo');
        if (mainContent) {
          (mainContent as HTMLElement).focus();
        } else {
          document.body.focus();
        }
      }, 50); // Pequeno atraso para garantir o DOM pronto
    });
  }

  // Método essencial para evitar vazamento de memória e bugs de foco repetido
  ngOnDestroy(): void {
    this.sub?.unsubscribe();
    this.routerSub?.unsubscribe(); // Limpa a escuta do roteador com segurança
  }

  private inicializarVLibras() {
    if (!document.querySelector('[vw]')) {
      const vwDiv = document.createElement('div');
      vwDiv.setAttribute('vw', '');
      vwDiv.classList.add('enabled');
      vwDiv.innerHTML = `
      <div vw-access-button class="active"></div>
      <div vw-plugin-wrapper>
        <div class="vw-plugin-top-wrapper"></div>
      </div>
    `;
      document.body.appendChild(vwDiv);
    }

    if (!(window as any).VLibras) {
      const script = document.createElement('script');
      script.src = 'https://vlibras.gov.br/app/vlibras-plugin.js';
      script.type = 'text/javascript';
      script.async = true;
      script.defer = true;

      script.onload = () => {
        if ((window as any).VLibras?.Widget) {
          new (window as any).VLibras.Widget('https://vlibras.gov.br/app');
        }
      };

      document.body.appendChild(script);
    } else {
      new (window as any).VLibras.Widget('https://vlibras.gov.br/app');
    }
  }
}
