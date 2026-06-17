/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/Hello-PROG7311/tree/main/16%20-%20Playwright%20Integration%20Tests
Date: [n.d]
Date Accessed: 16 May 2026
*/

const { defineConfig, devices } = require('@playwright/test');

module.exports = defineConfig({
  testDir: '.',
  timeout: 30 * 1000,

  use: {
    baseURL: process.env.CLIENT_BASE_URL || 'http://localhost:8082',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure'
  },

  reporter: [
    ['list'],
    ['html', { outputFolder: '../../playwright-report', open: 'never' }]
  ],

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] }
    }
  ]
});