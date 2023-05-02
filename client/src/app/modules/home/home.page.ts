import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { Observable, debounceTime, distinctUntilChanged, filter, finalize, map, switchMap, tap } from 'rxjs';
import { Hit, HitDTO } from 'src/app/models/hit.model';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'app-home',
    templateUrl: './home.page.html',
    styleUrls: ['./home.page.scss']
})
export class HomePage implements OnInit {

    constructor(private api: ApiService) { }

    playerName: FormControl = new FormControl('');
    isSearching: boolean;
    hits$: Observable<Hit[]>;

    ngOnInit(): void {
        this.initializeAutoComplete();
    }

    initializeAutoComplete() {
        this.hits$ = this.playerName.valueChanges
            .pipe(
                distinctUntilChanged(),
                filter(value => value.length >= 3),
                tap(() => this.isSearching = true),
                debounceTime(1000),
                switchMap((value: string) => {
                    return this.api.getSearchResults(value)
                        .pipe(
                            map((hitDTOs: HitDTO[]) => hitDTOs.map(Hit.adapt)),
                            finalize(() => this.isSearching = false)
                        );
                })
            );
    }

}
