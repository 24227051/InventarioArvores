export interface Especie {
  id: string;
  nomeCientifico: string;
  nomePopular: string;
  familia: string;
  origem: string;
}

export interface LocalizacaoGeoJson {
  type: string;
  coordinates: [number, number];
  enderecoAproximado: string;
}

export interface DendrometriaInfo {
  cap: number;
  alturaTotal: number;
  alturaComercial: number;
  diametroCopaNs: number;
  diametroCopaLo: number;
}

export interface FotoInfo {
  idFoto: number;
  urlFoto: string;
  dataRegistro: string;
  tipoFoto: string;
}

export interface LaudoTecnicoInfo {
  idLaudo: number;
  dataInspecao: string;
  responsavelTecnico: string;
  condicaoSanitaria: string;
  riscoQueda: string;
  recomendacaoManejo: string;
}

export interface Arvore {
  id: string;
  especieId: string;
  dataRegistro: string;
  statusViva: boolean;
  localizacao: LocalizacaoGeoJson;
  dendrometria: DendrometriaInfo;
  fotos?: FotoInfo[];
  laudosTecnicos?: LaudoTecnicoInfo[];
}

export interface ArvoreDetalhada extends Arvore {
  especie?: Especie;
}

