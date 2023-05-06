export interface TitleDTO {
    entity: string | null,
    entityImageUrl: string | null
    periods: string[]
};

export class Title {
    constructor(titleDTO: TitleDTO) {
        this.entity = titleDTO.entity;
        this.entityImageUrl = titleDTO.entityImageUrl;
        this.periods = titleDTO.periods;
    }

    periods: string[];
    entity: string | null;
    entityImageUrl: string | null;
}