import 'cypress-failed-log';

describe('Search page', () => {
    beforeEach(() => {
        cy.intercept('GET', `${Cypress.env('apiUrl')}/search/*`).as('getSearchResults');
        cy.visit('search/messi');
    });

    it(`should navigate to the player page when a row's anchor is clicked`, () => {
        cy.wait('@getSearchResults').then(() => {
            cy.on('url:changed', url => {
                expect(url).to.contain('/player/');
            });
            cy.get(`[data-cy="search-anchor"]`)
                .first()
                .click();
        });
    });

    it(`should get search results when the paginator buttons are clicked`, () => {
        let totalHits: number;
        cy.wait('@getSearchResults').then(interception => {
            totalHits = interception.response!.body.totalHits;
            cy.get(`[data-cy="search-paginator"] .mat-paginator-navigation-next`);
            return cy.get(`[data-cy="search-paginator"] .mat-paginator-navigation-next`)
                .click()
                .wait('@getSearchResults');
        }).then(interception => {
            expect(interception.request.url).to.contain('?page=2');
            return cy.get(`[data-cy="search-paginator"] .mat-paginator-navigation-previous`)
                .click()
                .wait('@getSearchResults');
        }).then(interception => {
            expect(interception.request.url).to.contain('?page=1');
            return cy.get(`[data-cy="search-paginator"] .mat-paginator-navigation-last`)
                .click()
                .wait('@getSearchResults');
        }).then(interception => {
            expect(interception.request.url).to.contain(`?page=${Math.ceil(totalHits / 10)}`);
        });
    });
});