import { Hit, HitDTO } from "./hit.model";

export interface PlayerDTO extends HitDTO {
    fullName: string,
    dateOfBirth: string | null,
    placeOfBirth: string | null,
    countryOfBirth: string | null,
    countryOfBirthImageUrl: string | null,
    dateOfDeath: string | null,
    age: number,
    height: number | null,
    position: string
};

export class Player extends Hit {
    constructor(
        tmId: number,
        name: string,
        public fullName: string,
        imageUrl: string,
        public dateOfBirth: Date | null,
        public placeOfBirth: string | null,
        public countryOfBirth: string | null,
        public countryOfBirthImageUrl: string | null,
        public dateOfDeath: Date | null,
        public age: number,
        public height: string | null,
        public position: string,
        club: string | null,
        clubImageUrl: string | null,
        status: string
    ) {
        super(
            tmId,
            name,
            imageUrl,
            club,
            clubImageUrl,
            null,
            null,
            status
        );
    }

    static override adapt(playerDTO: PlayerDTO) {
        return new Player(
            playerDTO.tmId,
            playerDTO.name,
            playerDTO.fullName,
            playerDTO.imageUrl,
            this.parseDate(playerDTO.dateOfBirth),
            playerDTO.placeOfBirth,
            playerDTO.countryOfBirth,
            playerDTO.countryOfBirthImageUrl,
            this.parseDate(playerDTO.dateOfDeath),
            playerDTO.age,
            this.parseHeight(playerDTO.height),
            playerDTO.position,
            playerDTO.club,
            playerDTO.clubImageUrl,
            playerDTO.status || 'Unknown'
        );
    }

    private static parseDate(dateString: string | null): Date | null {
        return dateString ? new Date(dateString) : null;
    }

    private static parseHeight(height: number | null): string | null {
        return height ? `${(height / 100).toFixed(2)} m` : null;
    }
}