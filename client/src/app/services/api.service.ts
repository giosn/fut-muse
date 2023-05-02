import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment as env } from '@env';
import { PlayerDTO } from '../models/player.model';
import { AchievementDTO } from '../models/achievement.model';
import { HitDTO } from '../models/hit.model';

@Injectable({
    providedIn: 'root'
})
export class ApiService {

    constructor(private http: HttpClient) { }

    private readonly baseUrl = env.baseUrl;

    getPlayerProfile(id: number) {
        const url = `${this.baseUrl}/players/profile/${id}`;
        return this.http.get<PlayerDTO>(url);
    }

    getAchievements(id: number) {
        const url = `${this.baseUrl}/players/achievements/${id}`;
        return this.http.get<AchievementDTO[]>(url);
    }

    getSearchResults(query: string) {
        const url = `${this.baseUrl}/hits/${query}`;
        return this.http.get<HitDTO[]>(url);
    }
}
