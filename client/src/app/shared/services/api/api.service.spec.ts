import { TestBed } from '@angular/core/testing';

import { ApiService } from './api.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { environment as env } from '@env';
import { mockId, mockPlayer, mockAchievements, mockSearchQuery, mockSearch } from './dto.mock';
import { PlayerDTO } from 'src/app/core/models/player.model';
import { AchievementDTO } from 'src/app/core/models/achievement.model';
import { SearchDTO } from 'src/app/core/models/search.model';

describe('ApiService', () => {
    let apiService: ApiService;
    let httpTestingController: HttpTestingController;
    const baseUrl = env.baseUrl;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [ HttpClientTestingModule ]
        });

        apiService = TestBed.inject(ApiService);
        httpTestingController = TestBed.inject(HttpTestingController);
    });

    afterEach(() => httpTestingController.verify());
    
    it('#getProfileData should return player profile data given an id', () => {
        apiService.getPlayerProfile(mockId)
            .subscribe({
                next: (playerDTO: PlayerDTO) => {
                    expect(playerDTO).toBeTruthy();
                    expect(playerDTO).toEqual(mockPlayer);
                }
            });

        const mockRequest = httpTestingController.expectOne({
            method: 'GET',
            url: `${baseUrl}/player/profile/${mockId}`
        });

        mockRequest.flush(mockPlayer);
    });

    it('#getAchievements should return player achievements data given an id', () => {
        apiService.getAchievements(mockId)
            .subscribe({
                next: (achievementDTOs: AchievementDTO[]) => {
                    expect(achievementDTOs).toBeTruthy();
                    expect(achievementDTOs).toEqual(mockAchievements);
                }
            });

        const mockRequest = httpTestingController.expectOne({
            method: 'GET',
            url: `${baseUrl}/player/achievements/${mockId}`
        });

        mockRequest.flush(mockAchievements);
    });

    it(`#getSearchResults should return an array of players given a search query`, () => {
        apiService.getSearchResults(mockSearchQuery)
            .subscribe({
                next: (searchDTO: SearchDTO) => {
                    expect(searchDTO).toBeTruthy();
                    expect(searchDTO).toEqual(mockSearch);
                }
            });

        const mockRequest = httpTestingController.expectOne({
            method: 'GET',
            url: `${baseUrl}/search/${mockSearchQuery}?page=0`
        });

        mockRequest.flush(mockSearch);
    });
});
