import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { Player, PlayerDTO } from 'src/app/models/player.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SnackbarService } from 'src/app/services/snackbar.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.page.html',
    styleUrls: ['./home.page.scss']
})
export class HomePage implements OnInit {

    constructor(
        private api: ApiService,
        private snackbar: SnackbarService
    ) { }

    playerId: number;
    isSearching: boolean;
    player: Player | null;

    ngOnInit(): void {
    }

    getPlayer() {
        this.isSearching = true;
        this.api.getPlayerProfile(this.playerId)
            .subscribe({
                next: (playerDTO: PlayerDTO) => {
                    const player: Player = Player.adapt(playerDTO);
                    this.player = player;
                },
                error: (response: HttpErrorResponse) => {
                    this.player = null;
                    console.error(response.message);
                    console.error(response.error);
                    this.snackbar.show(
                        `Could not find player ${response.status === 404 ? `with TM id ${this.playerId}` : ''}`,
                        'error'
                    );
                }
            })
            .add(() => this.isSearching = false);
    }

}
