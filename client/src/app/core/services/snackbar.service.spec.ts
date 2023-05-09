import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SnackbarComponent, SnackbarService } from './snackbar.service';
import { MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';
import { SharedModule } from 'src/app/shared/modules/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('SnackbarService', () => {
    let component: SnackbarComponent;
    let fixture: ComponentFixture<SnackbarComponent>;
    let service: SnackbarService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [
                BrowserAnimationsModule,
                SharedModule
            ],
            declarations: [
                SnackbarComponent
            ],
            providers: [
                SnackbarService,
                { provide: MAT_SNACK_BAR_DATA, useValue: {} },
            ]
        })
        .compileComponents();

        fixture = TestBed.createComponent(SnackbarComponent);
        component = fixture.componentInstance;
        service = TestBed.inject(SnackbarService);
    });

    it('should render snackbar component on show()', () => {
        service.show('test', 'success');
        fixture.detectChanges();
        expect(component).toBeTruthy();
    });
});
