import { AchievementDTO } from 'src/app/core/models/achievement.model';
import { PlayerDTO } from 'src/app/core/models/player.model';
import { SearchDTO } from 'src/app/core/models/search.model';

const mockId = 0;
const mockSearchQuery = '';

const mockPlayer: PlayerDTO = {
    fullName: '',
    dateOfBirth: null,
    placeOfBirth: null,
    countryOfBirth: null,
    countryOfBirthImageUrl: null,
    dateOfDeath: null,
    age: 0,
    height: null,
    position: '',
    tmId: 0,
    name: '',
    imageUrl: '',
    club: null,
    clubImageUrl: null,
    mainNationality: null,
    mainNationalityImageUrl: null,
    status: null
};

const mockAchievements: AchievementDTO[] = [
    {
        name: "",
        numberOfTitles: 0,
        titles: [
            {
                entity: null,
                entityImageUrl: null,
                periods: [ '' ]
            }
        ]
    }
];

const mockSearch: SearchDTO = {
    extendedSearchAvailable: false,
    totalHits: 0,
    hits: []
};

export {
    mockId,
    mockSearchQuery,
    mockPlayer,
    mockAchievements,
    mockSearch
};