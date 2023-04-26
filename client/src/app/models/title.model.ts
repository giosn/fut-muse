export interface TitleDTO {
    period: string,
    entity: string | null
};

export class Title {
    constructor(titleDTO: TitleDTO) {
        this.period = titleDTO.period;
        this.entity = titleDTO.entity;
    }

    period: string;
    entity: string | null;
}