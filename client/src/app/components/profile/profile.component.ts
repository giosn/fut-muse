import { Component, Input, OnInit } from '@angular/core';
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

    constructor() { }

    ngOnInit(): void {
    }

}
