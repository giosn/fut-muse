import { ChangeDetectorRef } from '@angular/core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, switchMap, finalize, map } from 'rxjs';
import { Hit } from 'src/app/models/hit.model';
import { Search, SearchDTO } from 'src/app/models/search.model';
import { ApiService } from 'src/app/services/api.service';

@Component({
    selector: 'app-search',
    templateUrl: './search.page.html',
    styleUrls: ['./search.page.scss']
})
export class SearchPage implements OnInit {

    @ViewChild(MatPaginator) paginator: MatPaginator;

    constructor(
        private title: Title,
        private router: Router,
        private route: ActivatedRoute,
        private changeDetect: ChangeDetectorRef,
        private api: ApiService
    ) {
        title.setTitle(`Fut Muse | Search | ${this.query}`);
        route.params.subscribe(params => this.query = params['query']);
        const state = router.getCurrentNavigation()?.extras?.state;
        this.totalHits = state?.['totalHits'] || 0;
        const hits: Hit[] = state?.['hits'] || [];
        this.table = {
            columns: ['name', 'nation', 'status', 'club'],
            data: new MatTableDataSource(hits),
            isLoading: false
        };
    }

    isLoading = true;
    query: string;
    totalHits: number;
    table: {
        columns: string[],
        data: MatTableDataSource<Hit>,
        isLoading: boolean
    };
    paginatorSub: Subscription;

    ngOnInit(): void {
    }

    ngAfterViewInit(): void {
        this.initializeTable();
        this.changeDetect.detectChanges();
    }

    ngOnDestroy(): void {
        this.paginatorSub?.unsubscribe();
    }

    initializeTable() {
        if (this.totalHits === 0 || this.totalHits > 10) {
            this.listenToPageChanges();
            this.paginator.page.emit({
                pageIndex: 0,
                pageSize: 0,
                length: this.totalHits
            });
        }
    }

    listenToPageChanges() {
        this.paginatorSub = this.paginator.page
            .pipe(
                switchMap((pageEvent: PageEvent) => {
                    this.table.isLoading = true;
                    const page = pageEvent.pageIndex + 1;
                    return this.api.getSearchResults(this.query, page)
                        .pipe(
                            map((searchDTO: SearchDTO) => {
                                const search: Search = Search.adapt(searchDTO);
                                if (this.paginator.length === 0) {
                                    this.totalHits = search.totalHits;
                                }
                                this.table.data.data = search.hits;
                            }),
                            finalize(() => {
                                this.table.isLoading = false;
                                this.isLoading = false;
                            })
                        );
                })
            )
            .subscribe();
    }
}
