import { Component, inject, OnInit } from '@angular/core';
import { EspecieService } from '../../services/especie.service';
import { Especie } from '../../models/tree.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-especie-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './especie-list.component.html',
  styleUrl: './especie-list.component.css'
})
export class EspecieListComponent implements OnInit {
  private readonly especieService = inject(EspecieService);

  especies: Especie[] = [];
  constructor(private router: Router) { }

  ngOnInit(): void {
    this.loadEspecies();
  }

  loadEspecies(): void {
    this.especieService.obterTodas().subscribe(especies => {
      this.especies = especies;
    });
  }

  goToAddEspecie(): void {
    this.router.navigate(['/add-especie']);
  }

  goToEditEspecie(id: string): void {
    this.router.navigate([`/edit-especie/${id}`]);
  }

  deleteEspecie(id: string): void {
    if (confirm('Tem certeza de que deseja excluir esta espécie?')) {
      this.especieService.deletar(id).subscribe({
        next: () => {
          this.loadEspecies();
        },
        error: (error) => {
          console.error('Erro ao deletar espécie:', error);
          alert('Erro ao excluir a espécie');
        }
      });
    }
  }
}
