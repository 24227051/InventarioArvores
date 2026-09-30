import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArvoreQrcodeComponent } from './arvore-qrcode.component';

describe('ArvoreQrcodeComponent', () => {
  let component: ArvoreQrcodeComponent;
  let fixture: ComponentFixture<ArvoreQrcodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArvoreQrcodeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArvoreQrcodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
