export interface HitDTO {
    tmId: number,
    name: string,
    imageUrl: string,
    mainNationality: string | null,
    mainNationalityImageUrl: string | null,
    club: string | null,
    clubImageUrl: string | null,
    status: string | null
};

export class Hit {
    constructor(
        public tmId: number,
        public name: string,
        public imageUrl: string,
        public mainNationality: string | null,
        public mainNationalityImageUrl: string | null,
        public club: string | null,
        public clubImageUrl: string | null,
        public status: string
    ) { }

    static adapt(hitDTO: HitDTO) {
        return new Hit(
            hitDTO.tmId,
            hitDTO.name,
            hitDTO.imageUrl,
            hitDTO.mainNationality,
            hitDTO.mainNationalityImageUrl,
            hitDTO.club,
            hitDTO.clubImageUrl,
            hitDTO.status || 'Unknown'
        );
    }
}