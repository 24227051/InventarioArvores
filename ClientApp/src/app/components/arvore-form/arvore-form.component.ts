import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EspecieService } from '../../services/especie.service';
import { ArvoreService } from '../../services/arvore.service'
import {
  ArvoreDetalhada,
  Especie,
  FotoInfo,
  LaudoTecnicoInfo
} from '../../models/tree.model';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-arvore-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './arvore-form.component.html',
  styleUrl: './arvore-form.component.css'
})
export class ArvoreFormComponent implements OnInit {
  arvore: ArvoreDetalhada = {
    id: '',
    especieId: '',
    dataRegistro: '',
    statusViva: true,
    localizacao: {
      type: 'Point',
      coordinates: [0, 0],
      enderecoAproximado: ''
    },
    dendrometria: {
      cap: 0,
      alturaTotal: 0,
      alturaComercial: 0,
      diametroCopaNs: 0,
      diametroCopaLo: 0
    },
    fotos: [],
    laudosTecnicos: []
  };
  isEditMode: boolean = false;
  especies: Especie[] = [];
  constructor(
    private arvoreService: ArvoreService,
    private especieService: EspecieService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.loadEspecies();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.arvoreService.obterPorId(id).subscribe(data => {
        this.arvore = {
          ...data,
          dataRegistro: data.dataRegistro?.slice(0, 10) ?? '',
          fotos: (data.fotos ?? []).map(foto => ({
            ...foto,
            dataRegistro: foto.dataRegistro?.slice(0, 10) ?? ''
          })),
          laudosTecnicos: data.laudosTecnicos ?? []
        };
      });
    }
  }

  loadEspecies(): void {
    this.especieService.obterTodas().subscribe(especies => {
      this.especies = especies;
    });
  }

  addFoto(): void {
    this.arvore.fotos?.push({
      idFoto: 0,
      urlFoto: '',
      dataRegistro: '',
      tipoFoto: ''
    });
  }

  removeFoto(index: number): void {
    this.arvore.fotos?.splice(index, 1);
  }

  addLaudo(): void {
    this.arvore.laudosTecnicos?.push({
      idLaudo: 0,
      dataInspecao: '',
      responsavelTecnico: '',
      condicaoSanitaria: '',
      riscoQueda: '',
      recomendacaoManejo: ''
    });
  }

  removeLaudo(index: number): void {
    this.arvore.laudosTecnicos?.splice(index, 1);
  }

  saveArvore(): void {
    if (this.isEditMode) {
      this.arvoreService.atualizar(this.arvore.id, this.arvore).subscribe(() => {
        this.router.navigate(['/arvores']);
      });
    } else {
      this.arvoreService.criar(this.arvore).subscribe(() => {
        this.router.navigate(['/arvores']);
      });
    }
  }
}
