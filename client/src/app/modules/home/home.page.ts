import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { Observable, debounceTime, distinctUntilChanged, filter, finalize, map, switchMap, tap } from 'rxjs';
import { FormControl } from '@angular/forms';
import { Search, SearchDTO } from 'src/app/models/search.model';
import { Hit } from 'src/app/models/hit.model';
import { Router } from '@angular/router';

@Component({
    selector: 'app-home',
    templateUrl: './home.page.html',
    styleUrls: ['./home.page.scss']
})
export class HomePage implements OnInit {

    constructor(
        private api: ApiService,
        private router: Router
    ) { }

    playerName: FormControl = new FormControl('');
    isSearching: boolean;
    search$: Observable<Search>;

    ngOnInit(): void {
        this.initializeAutoComplete();
    }

    initializeAutoComplete() {
        this.search$ = this.playerName.valueChanges
            .pipe(
                distinctUntilChanged(),
                filter(value => value.length >= 3),
                tap(() => this.isSearching = true),
                debounceTime(1000),
                switchMap((value: string) => {
                    return this.api.getSearchResults(value)
                        .pipe(
                            map((searchDTO: SearchDTO) => Search.adapt(searchDTO)),
                            finalize(() => this.isSearching = false)
                        );
                })
            );
    }

    navigateToSearchPage(totalHits: number, hits: Hit[]) {
        this.router.navigateByUrl(`/search/${this.playerName.value}`, {
            state: { totalHits, hits }
        });
    }
}
