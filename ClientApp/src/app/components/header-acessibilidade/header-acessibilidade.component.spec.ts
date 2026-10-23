import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderAcessibilidadeComponent } from './header-acessibilidade.component';

describe('HeaderAcessibilidadeComponent', () => {
  let component: HeaderAcessibilidadeComponent;
  let fixture: ComponentFixture<HeaderAcessibilidadeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderAcessibilidadeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderAcessibilidadeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
