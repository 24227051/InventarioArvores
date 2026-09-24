import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GoogleMapsModule } from '@angular/google-maps';
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
  selector: 'app-arvore-view',
  standalone: true,
  imports: [CommonModule, GoogleMapsModule],
  templateUrl: './arvore-view.component.html',
  styleUrl: './arvore-view.component.css'
})
export class ArvoreViewComponent implements OnInit {
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

  especies: Especie[] = [];
  constructor(
    private arvoreService: ArvoreService,
    private especieService: EspecieService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  // Configurações padrão do mapa
  mapZoom = 17;
  mapOptions: google.maps.MapOptions = {
    mapTypeId: 'hybrid', // Visão de satélite com rótulos (excelente para árvores urbanas/florestais)
    streetViewControl: true,
    mapTypeControl: true
  };

  ngOnInit(): void {
    this.loadEspecies();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
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

  // Converte a estrutura [lng, lat] do GeoJSON para { lat, lng } exigida pelo Google Maps
  getCoordinates(): google.maps.LatLngLiteral {
    if (this.arvore?.localizacao?.coordinates) {
      const [lng, lat] = this.arvore.localizacao.coordinates;
      return { lat, lng };
    }
    return { lat: 0, lng: 0 };
  }

  loadEspecies(): void {
    this.especieService.obterTodas().subscribe(especies => {
      this.especies = especies;
    });
  }

}
