describe('Home page', () => {
    beforeEach(() => cy.visit('/'));

    context('Search autocomplete', () => {
        it('should not trigger search when the query length is less than 3 characters', () => {
            cy.get(`[data-cy="search-player"]`).type('me');
            cy.get('.search-autocomplete').should('not.be.visible');
        });
    
        it('should trigger search when typing a player name that is 3 characters or longer', () => {
            cy.get(`[data-cy="search-player"]`).type('messi');
            cy.get('.search-autocomplete').should('be.visible');
        });
    
        it('should navigate to the player page when an option is clicked', () => {
            cy.intercept('GET', `${Cypress.env('apiUrl')}/search/*`).as('getSearchResults');
            cy.get(`[data-cy="search-player"]`).type('messi');
            cy.wait('@getSearchResults').then(() => {
                cy.get('.search-autocomplete')
                    .find('.mat-option')
                    .eq(0)
                    .click();
                cy.location().then(loc => {
                    expect(loc.href).to.contain('/player/');
                });
            });
        });
    
        it(`should navigate to the search page when the "View all results option" is clicked`, () => {
            cy.intercept('GET', `${Cypress.env('apiUrl')}/search/*`).as('getSearchResults');
            cy.get(`[data-cy="search-player"]`).type('messi');
            cy.wait('@getSearchResults').then(() => {
                cy.get('.search-autocomplete')
                    .find('.mat-option')
                    .last()
                    .click();
                cy.location().then(loc => {
                    expect(loc.href).to.contain('/search/messi');
                });
            });
        })
    
        it('should have only 1 option (disabled) when no search results are found', () => {
            cy.intercept('GET', `${Cypress.env('apiUrl')}/search/*`).as('getSearchResults');
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