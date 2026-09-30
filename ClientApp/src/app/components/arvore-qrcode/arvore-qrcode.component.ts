import { Component, inject, OnInit, ElementRef, ViewChildren, QueryList, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ArvoreService } from '../../services/arvore.service';
import * as QRCode from 'qrcode';

interface QrCodeItem {
  id: string;
  especie: string;
  url: string;
}

@Component({
  selector: 'app-arvore-qrcode',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './arvore-qrcode.component.html',
  styleUrl: './arvore-qrcode.component.css'
})
export class ArvoreQrcodeComponent implements OnInit, AfterViewInit {
  private route = inject(ActivatedRoute);
  private arvoreService = inject(ArvoreService);

  @ViewChildren('qrCanvas') qrCanvases!: QueryList<ElementRef<HTMLCanvasElement>>;

  qrCodesList: QrCodeItem[] = [];
  carregando = true;

  ngOnInit(): void {
    // 1. Captura os IDs que vieram na URL (?ids=1,2,3)
    this.route.queryParams.subscribe(params => {
      const idsParam = params['ids'];
      if (idsParam) {
        const idsArray = idsParam.split(',');
        this.buscarDadosEGerarUrls(idsArray);
      } else {
        this.carregando = false;
      }
    });
  }

  ngAfterViewInit(): void {
    // Escuta quando a lista de elementos canvas mudar no HTML para renderizar os QRs
    this.qrCanvases.changes.subscribe(() => {
      this.renderizarQrCodes();
    });
  }

  private buscarDadosEGerarUrls(ids: string[]): void {
    this.carregando = true;
    const baseHref = window.location.origin;

    // Buscando os dados de todas as árvores selecionadas
    // Obs: Se sua API tiver um endpoint de busca em lote, use-o. 
    // Caso contrário, buscamos todas e filtramos no front para poupar requisições individuais.
    this.arvoreService.obterTodas().subscribe({
      next: (todasAsArvores) => {
        const filtradas = todasAsArvores.filter(a => ids.includes(a.id));

        this.qrCodesList = filtradas.map(arvore => ({
          id: arvore.id,
          especie: arvore.especie?.nomePopular ?? 'Sem espécie',
          url: `${baseHref}/arvores/${arvore.id}`
        }));

        this.carregando = false;
      },
      error: (err) => {
        console.error('Erro ao carregar dados para QR Code', err);
        this.carregando = false;
      }
    });
  }

  private renderizarQrCodes(): void {
    if (!this.qrCanvases || this.qrCanvases.length === 0) return;
    const canvasArray = this.qrCanvases.toArray();

    this.qrCodesList.forEach((item, index) => {
      const canvasElement = canvasArray[index]?.nativeElement;
      if (canvasElement) {
        QRCode.toCanvas(canvasElement, item.url, { width: 180, margin: 2 }, (error) => {
          if (error) console.error(error);
        });
      }
    });
  }

  imprimir(): void { window.print(); }
}
