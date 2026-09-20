import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EspecieService } from '../../services/especie.service'
import { Especie } from '../../models/tree.model';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-especie-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './especie-form.component.html',
  styleUrl: './especie-form.component.css'
})
export class EspecieFormComponent implements OnInit {
  especie: Especie = { id: '', nomeCientifico: '', nomePopular: '', familia: '', origem: '' };
  isEditMode: boolean = false;

  constructor(
    private especieService: EspecieService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.especieService.obterPorId(id).subscribe(data => {
        this.especie = data;
      });
    }
  }

  saveEspecie(): void {
    if (this.isEditMode) {
      this.especieService.atualizar(this.especie.id, this.especie).subscribe(() => {
        this.router.navigate(['/especies']);
      });
    } else {
      this.especieService.criar(this.especie).subscribe(() => {
        this.router.navigate(['/especies']);
      });
    }
  }
}
