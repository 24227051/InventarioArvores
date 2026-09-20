import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArvoreFormComponent } from './arvore-form.component';

describe('ArvoreFormComponent', () => {
  let component: ArvoreFormComponent;
  let fixture: ComponentFixture<ArvoreFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArvoreFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArvoreFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
