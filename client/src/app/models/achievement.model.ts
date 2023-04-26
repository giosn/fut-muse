import { Title, TitleDTO } from "./title.model";

export interface AchievementDTO {
    name: string,
    numberOfTitles: number,
    titles: string
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

    private static parseTitles(titles: string): Title[] {
        const parsedTitles: TitleDTO[] = JSON.parse(titles);
        return parsedTitles.map(titleDTO => new Title(titleDTO));
    }
}