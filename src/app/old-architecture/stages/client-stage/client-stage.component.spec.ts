import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientStageComponent } from './client-stage.component';

describe('ClientStageComponent', () => {
  let component: ClientStageComponent;
  let fixture: ComponentFixture<ClientStageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientStageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
