import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArvoreViewComponent } from './arvore-view.component';

describe('ArvoreViewComponent', () => {
  let component: ArvoreViewComponent;
  let fixture: ComponentFixture<ArvoreViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArvoreViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArvoreViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
