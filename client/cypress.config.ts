import { defineConfig } from 'cypress';
import { environment as env } from '@env';

export default defineConfig({
    e2e: {
        setupNodeEvents(on, config) {
            // implement node event listeners here
            require('cypress-failed-log/on')(on)
        },
        baseUrl: 'http://localhost:4200',
        env: {
            apiUrl: env.baseUrl
        }
    },
});
