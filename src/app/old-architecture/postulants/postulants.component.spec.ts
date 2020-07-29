/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PostulantsComponent } from './postulants.component';

describe('PostulantsComponent', () => {
  let component: PostulantsComponent;
  let fixture: ComponentFixture<PostulantsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PostulantsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostulantsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
