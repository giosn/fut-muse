import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { Player, PlayerDTO } from 'src/app/models/player.model';
import { Achievement, AchievementDTO } from 'src/app/models/achievement.model';
import { mergeMap } from 'rxjs';

@Component({
    selector: 'app-home',
    templateUrl: './home.page.html',
    styleUrls: ['./home.page.scss']
})
export class HomePage implements OnInit {

    constructor(private api: ApiService) { }

    playerId: number;
    isSearching: boolean;
    player: Player | null;
    achievements: Achievement[];

    ngOnInit(): void {
    }

    getPlayer() {
        this.isSearching = true;
        this.api.getPlayerProfile(this.playerId)
            .pipe(
                mergeMap((playerDTO: PlayerDTO) => {
                    const player: Player = Player.adapt(playerDTO);
                    this.player = player;
                    return this.api.getAchievements(this.playerId);
                })
            )
            .subscribe({
                next: (achievementDTOs: AchievementDTO[]) => {
                    const achievements: Achievement[] = achievementDTOs.map(a => Achievement.adapt(a));
                    this.achievements = achievements;
                },
                error: () => {
                    this.player = null;
                    this.achievements = [];
                }
            })
            .add(() => this.isSearching = false);
    }

}
