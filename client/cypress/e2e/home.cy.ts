import 'cypress-failed-log';

describe('Home page', () => {
    beforeEach(() => cy.visit('/'));

    context('Search autocomplete', () => {
        it('should not trigger search when the query length is less than 3 characters', () => {
            cy.get(`[data-cy="search-player"]`).type('me');
            cy.get('.search-autocomplete').should('not.be.visible');
        });
    
        it('should trigger search when typing a player name that is 3 characters or longer', () => {
            // stub search results
            cy.intercept(
                'GET',
                `${Cypress.env('apiUrl')}/search/*`,
                { fixture: 'search-results.json' }
            ).as('getSearchResults');

            cy.get(`[data-cy="search-player"]`).type('mes');

            // force stubbing
            cy.wait('@getSearchResults');

            cy.get('.search-autocomplete').should('be.visible');
        });
    
        it('should navigate to the player page when an option is clicked', () => {
            // stub search results
            cy.intercept(
                'GET',
                `${Cypress.env('apiUrl')}/search/*`,
                { fixture: 'search-results.json' }
            ).as('getSearchResults');

            cy.get(`[data-cy="search-player"]`).type('messi');

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

                cy.get('.search-autocomplete')
                    .find('.mat-option')
                    .eq(0)
                    .click();

                // force stubbing
                cy.wait('@getPlayerProfile');
                cy.wait('@getPlayerAchievements');
            });
        });
    
        it(`should navigate to the search page when the "View all results option" is clicked`, () => {
            // stub search results
            cy.intercept(
                'GET',
                `${Cypress.env('apiUrl')}/search/*`,
                { fixture: 'search-results.json' }
            ).as('getSearchResults');

            cy.get(`[data-cy="search-player"]`).type('messi');

            cy.wait('@getSearchResults').then(() => {
                cy.on('url:changed', url => {
                    expect(url).to.contain('/search/messi');
                });
                cy.get('.search-autocomplete')
                    .find('.mat-option')
                    .last()
                    .click();
            });
        })
    
        it('should have only 1 option (disabled) when no search results are found', () => {
            // stub search results
            cy.intercept(
                'GET',
                `${Cypress.env('apiUrl')}/search/*`,
                { fixture: 'empty-search.json' }
            ).as('getSearchResults');

            cy.get(`[data-cy="search-player"]`).type('not_a_player');

            cy.wait('@getSearchResults').then(() => {
                cy.get('.search-autocomplete')
                    .find('.mat-option')
                    .should('have.length', 1)
                    .eq(0)
                    .should('have.attr', 'disabled');
            });
        });
    });

    after(() => cy.visit('/'));

})