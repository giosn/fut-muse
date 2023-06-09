import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment as env } from '@env';
import { PlayerDTO } from 'src/app/core/models/player.model';
import { AchievementDTO } from 'src/app/core/models/achievement.model';
import { SearchDTO } from 'src/app/core/models/search.model';

@Injectable({
    providedIn: 'root'
})
export class ApiService {

    constructor(private http: HttpClient) { }

    private readonly baseUrl = env.baseUrl;

    getPlayerProfile(id: number) {
        const url = `${this.baseUrl}/player/profile/${id}`;
        return this.http.get<PlayerDTO>(url);
    }

    getAchievements(id: number) {
        const url = `${this.baseUrl}/player/achievements/${id}`;
        return this.http.get<AchievementDTO[]>(url);
    }

    getSearchResults(query: string, page: number = 0) {
        const url = `${this.baseUrl}/search/${query}?page=${page}`;
        return this.http.get<SearchDTO>(url);
    }
}
