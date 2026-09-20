import { Component, inject, OnInit } from '@angular/core';
import { ArvoreService } from '../../services/arvore.service';
import { Arvore, ArvoreDetalhada } from '../../models/tree.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-arvore-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './arvore-list.component.html',
  styleUrl: './arvore-list.component.css'
})
export class ArvoreListComponent implements OnInit {
  private readonly arvoreService = inject(ArvoreService);

  arvores: ArvoreDetalhada[] = [];
  constructor(private router: Router) { }

  ngOnInit(): void {
    this.loadArvores();
  }

  loadArvores(): void {
    this.arvoreService.obterTodas().subscribe(arvores => {
      this.arvores = arvores;
    });
  }

  goToAddArvore(): void {
    this.router.navigate(['/add-arvore']);
  }

  goToEditArvore(id: string): void {
    this.router.navigate([`/edit-arvore/${id}`]);
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
