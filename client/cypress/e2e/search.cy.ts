import 'cypress-failed-log';

describe('Search page', () => {
    beforeEach(() => {
        // stub search results
        cy.intercept(
            'GET',
            `${Cypress.env('apiUrl')}/search/*`,
            { fixture: 'search-results.json' }
        ).as('getSearchResults');

        cy.visit('search/messi');
    });

    it(`should navigate to the player page when a row's anchor is clicked`, () => {
        cy.wait('@getSearchResults').then(() => {
            cy.on('url:changed', url => {
                expect(url).to.contain('/player/');
            });

            // stub player responses
            cy.intercept(
                'GET',
                `${Cypress.env('apiUrl')}/player/profile/*`,
                { fixture: 'player-profile.json' }
            ).as('getPlayerProfile');
            cy.intercept(
                'GET',
                `${Cypress.env('apiUrl')}/player/achievements/*`,
                { fixture: 'player-achievements.json' }
            ).as('getPlayerAchievements');

            cy.get(`[data-cy="search-anchor"]`)
                .first()
                .click();

            // force stubbing
            cy.wait('@getPlayerProfile');
            cy.wait('@getPlayerAchievements');
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