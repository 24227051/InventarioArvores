import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EspecieListComponent } from './especie-list.component';

describe('EspecieListComponent', () => {
  let component: EspecieListComponent;
  let fixture: ComponentFixture<EspecieListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EspecieListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EspecieListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
