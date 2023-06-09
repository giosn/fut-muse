import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription, mergeMap } from 'rxjs';
import { Achievement, AchievementDTO } from 'src/app/core/models/achievement.model';
import { Player, PlayerDTO } from 'src/app/core/models/player.model';
import { ApiService } from 'src/app/shared/services/api/api.service';

@Component({
    selector: 'app-player',
    templateUrl: './player.page.html',
    styleUrls: ['./player.page.scss']
})
export class PlayerPage implements OnInit, OnDestroy {

    constructor(
        private title: Title,
        private route: ActivatedRoute,
        iconRegistry: MatIconRegistry,
        sanitizer: DomSanitizer,
        private api: ApiService
    ) {
        this.title.setTitle('Fut Muse | Player');
        route.params.subscribe(params => this.params = params);
        this.id = this.params['id'];
        iconRegistry.addSvgIcon(
            'rip',
            sanitizer.bypassSecurityTrustResourceUrl('assets/icons/rip.svg')
        );
    }

    params: Params;
    id: number;
    player: Player | null;
    achievements: Achievement[];
    playerIsLoading = true;
    achievementsLoading = true;
    playerNotFound = false;
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
                    this.title.setTitle(`${this.title.getTitle()} | ${this.player.name}`);
                    this.playerIsLoading = false;
                    return this.api.getAchievements(this.id);
                })
            )
            .subscribe({
                next: (achievementDTOs: AchievementDTO[]) => {
                    const achievements: Achievement[] = achievementDTOs.map(a => Achievement.adapt(a));
                    this.achievements = achievements;
                    this.achievementsLoading = false;
                },
                error: () => {
                    this.player = null;
                    this.achievements = [];
                    this.playerIsLoading = false;
                    this.achievementsLoading = false;
                }
            });
        this.playerSub.add(() => {
            this.playerNotFound = !this.playerIsLoading     &&
                                  !this.achievementsLoading &&
                                  !this.player;
        });
    }
}
