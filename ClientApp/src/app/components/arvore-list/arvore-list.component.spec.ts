import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArvoreListComponent } from './arvore-list.component';

describe('ArvoreListComponent', () => {
  let component: ArvoreListComponent;
  let fixture: ComponentFixture<ArvoreListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArvoreListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArvoreListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
