import { Component, inject, OnInit } from '@angular/core';
import { ArvoreService } from '../../services/arvore.service';
import { Arvore, ArvoreDetalhada } from '../../models/tree.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface ArvoreListavel extends ArvoreDetalhada {
  selecionada?: boolean;
}

@Component({
  selector: 'app-arvore-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './arvore-list.component.html',
  styleUrl: './arvore-list.component.css'
})
export class ArvoreListComponent implements OnInit {
  private readonly arvoreService = inject(ArvoreService);
  private router = inject(Router);

  arvores: ArvoreListavel[] = [];

  ngOnInit(): void {
    this.loadArvores();
  }

  loadArvores(): void {
    this.arvoreService.obterTodas().subscribe(arvores => {
      this.arvores = arvores;
      this.arvores = this.arvores.map(arvore => {
        return {
          ...arvore,
          dataRegistro: arvore.dataRegistro?.slice(0, 10) ?? ''
        };
      });
    });
  }

  isAllSelected(): boolean {
    return this.arvores.length > 0 && this.arvores.every(a => a.selecionada);
  }

  toggleAll(event: any): void {
    const checked = event.target.checked;
    this.arvores.forEach(a => a.selecionada = checked);
  }

  getSelectedIds(): string[] {
    return this.arvores.filter(a => a.selecionada).map(a => a.id);
  }

  // Redireciona enviando os IDs como parâmetro de consulta (ex: ?ids=1,2,3)
  goToGerarQrCodes(): void {
    const ids = this.getSelectedIds();
    if (ids.length > 0) {
      this.router.navigate(['/arvores-qrcode'], {
        queryParams: { ids: ids.join(',') }
      });
    }
  }

  goToAddArvore(): void {
    this.router.navigate(['/add-arvore']);
  }

  goToEditArvore(id: string): void {
    this.router.navigate([`/edit-arvore/${id}`]);
  }

  goToViewArvore(id: string): void {
    // Navega para a rota protegida que exibe o menu (MenuVisibilityGuard controla exibição)
    this.router.navigate(['/arvores', id]);
  }

  deleteArvore(id: string): void {
    if (confirm('Tem certeza de que deseja excluir esta árvore?')) {
      this.arvoreService.deletar(id).subscribe({
        next: () => {
          this.loadArvores();
        },
        error: (error) => {
          console.error('Erro ao deletar árvore:', error);
          alert('Erro ao excluir a árvore');
        }
      });
    }
  }
}
