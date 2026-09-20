import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EspecieFormComponent } from './especie-form.component';

describe('EspecieFormComponent', () => {
  let component: EspecieFormComponent;
  let fixture: ComponentFixture<EspecieFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EspecieFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EspecieFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
