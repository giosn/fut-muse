import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription, mergeMap } from 'rxjs';
import { Achievement, AchievementDTO } from 'src/app/models/achievement.model';
import { Player, PlayerDTO } from 'src/app/models/player.model';
import { ApiService } from 'src/app/services/api.service';

@Component({
    selector: 'app-player',
    templateUrl: './player.page.html',
    styleUrls: ['./player.page.scss']
})
export class PlayerPage implements OnInit {

    constructor(
        private title: Title,
        private route: ActivatedRoute,
        private api: ApiService
    ) {
        this.title.setTitle('Fut Muse | Player');
        route.params.subscribe(params => this.params = params);
        this.id = this.params['id'];
    }

    params: Params;
    id: number;
    player: Player | null;
    achievements: Achievement[];
    isLoading = true;
    playerSub: Subscription;

    ngOnInit(): void {
        this.getPlayer();
    }

    ngOnDestroy(): void {
        this.playerSub.unsubscribe();
    }

    getPlayer() {
        this.playerSub = this.api.getPlayerProfile(this.id)
            .pipe(
                mergeMap((playerDTO: PlayerDTO) => {
                    const player: Player = Player.adapt(playerDTO);
                    this.player = player;
                    return this.api.getAchievements(this.id);
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
            });
        this.playerSub.add(() => {
            this.isLoading = false;
            if (this.player) {
                this.title.setTitle(`${this.title.getTitle()} | ${this.player.name}`);
            }
        });
    }
}
