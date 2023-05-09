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
            duration: duration || type === 'error' ? 6000 : 4000,
            panelClass: type,
            data: {
                message: message,
                type: type
            }
        };
        this.snackbar.openFromComponent(SnackbarComponent, config);
    }

}

const template: string = 
    `<span style="display: flex; align-items: center;">
        <mat-icon>
            {{
                data.type === 'success'
                    ? 'check_circle'
                    : data.type === 'error'
                        ? 'error_outline'
                        : 'warning'
            }}
        </mat-icon>
        {{data.message}}
    </span>`;

@Component({ template })
export class SnackbarComponent {
    constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any) { }
}