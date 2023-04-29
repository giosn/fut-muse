export interface TitleDTO {
    period: string,
    entity: string | null,
    entityImageUrl: string | null
};

export class Title {
    constructor(titleDTO: TitleDTO) {
        this.period = titleDTO.period;
        this.entity = titleDTO.entity;
        this.entityImageUrl = titleDTO.entityImageUrl;
    }

    period: string;
    entity: string | null;
    entityImageUrl: string | null;
}