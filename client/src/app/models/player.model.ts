export interface PlayerDTO {
    tMId: number,
    name: string,
    fullName: string,
    imageUrl: string,
    dateOfBirth: string | null,
    placeOfBirth: string | null,
    countryOfBirth: string | null,
    dateOfDeath: string | null,
    age: number,
    height: number,
    position: string,
    currentClub: string | null,
    status: string | null
};

export class Player {
    constructor(
        public tmId: number,
        public name: string,
        public fullName: string,
        public imageUrl: string,
        public dateOfBirth: Date | null,
        public placeOfBirth: string | null,
        public countryOfBirth: string | null,
        public dateOfDeath: Date | null,
        public age: number,
        public height: string,
        public position: string,
        public currentClub: string | null,
        public status: string | null
    ) { }

    static adapt(playerDTO: PlayerDTO) {
        return new Player(
            playerDTO.tMId,
            playerDTO.name,
            playerDTO.fullName,
            playerDTO.imageUrl,
            this.parseDate(playerDTO.dateOfBirth),
            playerDTO.placeOfBirth,
            playerDTO.countryOfBirth,
            this.parseDate(playerDTO.dateOfDeath),
            playerDTO.age,
            this.parseHeight(playerDTO.height),
            playerDTO.position,
            playerDTO.currentClub,
            playerDTO.status
        );
    }

    private static parseDate(dateString: string | null): Date | null {
        return dateString ? new Date(dateString) : null;
    }

    private static parseHeight(height: number): string {
        return `${(height / 100).toFixed(2)} m`;
    }
}