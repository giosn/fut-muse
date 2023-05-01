export interface HitDTO {
    tmId: number,
    name: string,
    imageUrl: string,
    club: string | null,
    clubImageUrl: string | null,
    mainNationality: string | null,
    mainNationalityImageUrl: string | null,
    status: string | null
};

export class Hit {
    constructor(
        public tmId: number,
        public name: string,
        public imageUrl: string,
        public club: string | null,
        public clubImageUrl: string | null,
        public mainNationality: string | null,
        public mainNationalityImageUrl: string | null,
        public status: string
    ) { }

    static adapt(playerDTO: HitDTO) {
        return new Hit(
            playerDTO.tmId,
            playerDTO.name,
            playerDTO.imageUrl,
            playerDTO.club,
            playerDTO.clubImageUrl,
            playerDTO.mainNationality,
            playerDTO.mainNationalityImageUrl,
            playerDTO.status || 'Unknown'
        );
    }
}