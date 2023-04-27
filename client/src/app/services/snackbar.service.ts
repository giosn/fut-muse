import { Injectable, Component, Inject } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig, MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

type SnackbarType = 'success' | 'error' | 'warn';

@Injectable({
    providedIn: 'root'
})
export class SnackbarService {

    constructor(private snackbar: MatSnackBar) { }

    show(message: string, type: SnackbarType, duration?: number) {
        const config: MatSnackBarConfig = {
            duration:   duration ? duration : type === 'error' ? 6000 : 4000,
            panelClass: type,
            data: {
                message: message,
                type: type
            }
        };
        this.snackbar.openFromComponent(SnackbarComponent, config);
    }

}

@Component({
    template:
        `<span style="display: flex; align-items: center;">
            <mat-icon>
                {{data.type === 'success' ? 'check_circle' : data.type === 'error' ? 'error_outline' : 'warning'}}
            </mat-icon>
            {{data.message}}
        </span>`,
})
export class SnackbarComponent {

    constructor(
        public snackBarRef: MatSnackBarRef<SnackbarComponent>,
        @Inject(MAT_SNACK_BAR_DATA) public data: any
    ) { }

}