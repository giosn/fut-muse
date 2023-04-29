import { Title, TitleDTO } from "./title.model";

export interface AchievementDTO {
    name: string,
    numberOfTitles: number,
    titles: TitleDTO[]
};

export class Achievement {
    constructor(
        public name: string,
        public numberOfTitles: number,
        public titles: Title[]
    ) { }

    static adapt(achievementDTO: AchievementDTO) {
        return new Achievement(
            achievementDTO.name,
            achievementDTO.numberOfTitles,
            this.parseTitles(achievementDTO.titles)
        );
    }

    private static parseTitles(titles: TitleDTO[]): Title[] {
        return titles.map(titleDTO => new Title(titleDTO));
    }
}