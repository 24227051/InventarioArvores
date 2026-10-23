import { Component, HostListener, Inject, Renderer2 } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-header-acessibilidade',
  standalone: true,
  imports: [],
  templateUrl: './header-acessibilidade.component.html',
  styleUrls: ['./header-acessibilidade.component.css']
})
export class HeaderAcessibilidadeComponent {
  private isAltoContraste = false;

  constructor(
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document
  ) { }

  // Alterna o modo de Alto Contraste injetando a classe no <body>
  toggleAltoContraste(event?: Event): void {
    if (event) event.preventDefault();
    this.isAltoContraste = !this.isAltoContraste;

    if (this.isAltoContraste) {
      this.renderer.addClass(this.document.body, 'alto-contraste');
    } else {
      this.renderer.removeClass(this.document.body, 'alto-contraste');
    }
  }

  // Captura os atalhos de teclado oficiais do eMAG (Alt + número)
  @HostListener('window:keydown', ['$event'])
  handleKeyboardEvents(event: KeyboardEvent): void {
    if (event.altKey) {
      switch (event.key) {
        case '1':
          this.irPara('conteudo-principal');
          break;
        case '2':
          this.irPara('menu-principal');
          break;
        case '3':
          this.irPara('campo-busca');
          break;
      }
    }
  }

  // Função auxiliar para mover o foco e a tela para o elemento correto
  irPara(elementId: string): void {
    const element = this.document.getElementById(elementId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
      element.focus();
    }
  }
}
