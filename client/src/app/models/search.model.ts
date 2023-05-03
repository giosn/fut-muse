import { Hit, HitDTO } from "./hit.model";

export interface SearchDTO {
    extendedSearchAvailable: boolean,
    totalHits: number,
    hits: HitDTO[]
};

export class Search {
    constructor(
        public extendedSearchAvailable: boolean,
        public totalHits: number,
        public hits: Hit[]
    ) { }
    
    static adapt(searchDTO: SearchDTO) {
        return new Search(
            searchDTO.extendedSearchAvailable,
            searchDTO.totalHits,
            searchDTO.hits.map(Hit.adapt)
        )
    }
}