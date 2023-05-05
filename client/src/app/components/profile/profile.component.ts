import { Component, Input, OnInit } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { Achievement } from 'src/app/models/achievement.model';
import { Player } from 'src/app/models/player.model';

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

    @Input('player') player: Player;
    @Input('achievements') achievements: Achievement[];

    constructor(
        iconRegistry: MatIconRegistry,
        sanitizer: DomSanitizer
    ) {
        iconRegistry.addSvgIcon(
            'rip',
            sanitizer.bypassSecurityTrustResourceUrl('assets/icons/rip.svg')
        );
    }

    ngOnInit(): void {
    }

}
