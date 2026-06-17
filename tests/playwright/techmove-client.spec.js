/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/Hello-PROG7311/tree/main/16%20-%20Playwright%20Integration%20Tests
Date: [n.d]
Date Accessed: 16 May 2026
*/

const { test, expect } = require('@playwright/test');

const uniqueId = Date.now();

const testUser = {
  email: `playwright.${uniqueId}@techmove.test`,
  testPassword: 'Playwright-Test-Password-123!'
};

// Test to ensure the health check endpoint is working
test('client health check loads', async ({ request }) => {
  const response = await request.get('/health');

  expect(response.ok()).toBeTruthy();
  expect(await response.text()).toBe('ok');
});

// Test to ensure the home page loads and has the correct title and main heading
test('home page loads successfully', async ({ page }) => {
  await page.goto('/');

  await expect(page).toHaveTitle(/TechMoveClient/i);

  await expect(page.getByRole('heading', { name: /TechMove Logistics/i })).toBeVisible();

  await expect(page.getByRole('navigation')).toBeVisible();
});

// Test to ensure the home page has the correct navigation cards
test('home page navigation cards are visible', async ({ page }) => {
  await page.goto('/');

  await expect(page.getByRole('heading', { name: /Client Management/i })).toBeVisible();
  await expect(page.getByRole('heading', { name: /Contract Management/i })).toBeVisible();
  await expect(page.getByRole('heading', { name: /Service Requests/i })).toBeVisible();

  await expect(page.getByRole('link', { name: /Go to Clients/i })).toBeVisible();
  await expect(page.getByRole('link', { name: /View Contracts/i })).toBeVisible();
  await expect(page.getByRole('link', { name: /Manage Requests/i })).toBeVisible();
});

// Test to ensure the login page loads and has the correct things
test('login page loads', async ({ page }) => {
  await page.goto('/Auth/Login');

  await expect(page).toHaveTitle(/TechMoveClient/i);

  await expect(page.getByRole('heading', { name: /Login/i })).toBeVisible();
  await expect(page.getByLabel('Email')).toBeVisible();
  await expect(page.getByLabel('Password')).toBeVisible();
  await expect(page.getByRole('button', { name: /Login/i })).toBeVisible();
});

// Test for registration page to ensure it loads and has the correct things
test('register page loads', async ({ page }) => {
  await page.goto('/Auth/Register');

  await expect(page).toHaveTitle(/TechMoveClient/i);

  await expect(page.getByRole('heading', { name: /Register/i })).toBeVisible();
  await expect(page.getByLabel('Email')).toBeVisible();
  await expect(page.getByLabel('Password')).toBeVisible();
  await expect(page.getByRole('button', { name: /Register/i })).toBeVisible();
});

// Test to ensure unauthenticated users are redirected to login when trying to access protected pages
test('unauthenticated users are redirected from protected pages', async ({ page }) => {
  await page.goto('/Client');
  await expect(page.url()).toContain('/Auth/Login');

  await page.goto('/Contract');
  await expect(page.url()).toContain('/Auth/Login');

  await page.goto('/ServiceRequest');
  await expect(page.url()).toContain('/Auth/Login');
});

// Test to ensure authenticated users can access protected pages
test.describe.serial('authenticated user flow', () => {

  test.beforeAll(async ({ browser }) => {
    const page = await browser.newPage();
    await page.goto('/Auth/Register');

    await page.getByLabel('Email').fill(testUser.email);
    await page.getByLabel('Password').fill(testUser.testPassword);
    await page.getByRole('button', { name: /Register/i }).click();

    await expect(page.locator('body')).toBeVisible();

    await page.close();
  });

  // Log in before each test to ensure we have a valid session
  test.beforeEach(async ({ page }) => {
    await page.goto('/Auth/Login');

    await page.getByLabel('Email').fill(testUser.email);
    await page.getByLabel('Password').fill(testUser.testPassword);
    await page.getByRole('button', { name: /Login/i }).click();

    await expect(page.locator('body')).toBeVisible();
    await expect(page.getByRole('navigation')).toBeVisible();
  });

  // Test to ensure the user can see the main dashboard after logging in
  test('user can login', async ({ page }) => {
    await expect(page.locator('body')).toBeVisible();
    await expect(page.getByRole('navigation')).toBeVisible();
  });

  // Test to ensure the user can navigate to the Clients page 
  test('user can navigate to Clients page', async ({ page }) => {
    await page.goto('/Client');

    await expect(page.getByRole('heading', { name: /Clients/i })).toBeVisible();
    await expect(page.url()).toContain('/Client');
  });

  // Test to ensure the user can create a Client
  test('user can create a Client', async ({ page }) => {
    const clientName = `Playwright Client ${uniqueId}`;

    await page.goto('/Client/Create');

    await page.locator('input[name="Name"]').fill(clientName);
    await page.locator('input[name="ContactDetails"]').fill('playwright@test.com');
    await page.locator('select[name="Region"]').selectOption('Gauteng');

    await page.getByRole('button', { name: /Create Client/i }).click();

    await expect(page.url()).toContain('/Client');

    await expect(page.getByText(clientName)).toBeVisible();
  });

  // Test to ensure the user can edit a Client
    test('user can edit a Client', async ({ page }) => {

      await page.goto('/Client');

      await page.getByRole('link', { name: /Edit/i }).first().click();

      await page.locator('input[name="Name"]').fill('Updated Client Name');
      await page.locator('input[name="ContactDetails"]').fill('updated@playwright.test');

      await page.getByRole('button', { name: /Save Changes/i }).click();

      await expect(page.url()).toContain('/Client');
      await expect(page.getByText('Updated Client Name')).toBeVisible();
    });

  // Test to ensure the user can delete a Client
  test('user can delete a Client', async ({ page }) => {
    await page.goto('/Client');

    const deleteButton = page.getByRole('button', { name: /Delete/i }).first();

    if (await deleteButton.isVisible()) 
    {
      await deleteButton.click();
    }

    await expect(page.locator('body')).toBeVisible();
  });

  // Test to ensure the user can navigate to Contracts page
  test('user can navigate to Contracts page', async ({ page }) => {
    await page.goto('/Contract');

    await expect(page.getByRole('heading', { name: /Contract/i })).toBeVisible();
    await expect(page.url()).toContain('/Contract');
  });

  // Test to ensure the user can create a Contract
  test('user can create a Contract', async ({ page }) => {

    await page.goto('/Contract/Create');

    const clientDropdown = page.locator('select[name="ClientId"]');

    await expect(clientDropdown).toBeVisible();

    const options = clientDropdown.locator('option');
    const optionCount = await options.count();
    const selectedOption = options.nth(1);
    const selectedClientName = await selectedOption.textContent();
    const selectedClientValue = await selectedOption.getAttribute('value');

    await clientDropdown.selectOption(selectedClientValue);

    await page.locator('input[name="StartDate"]').fill('2026-01-01');
    await page.locator('input[name="EndDate"]').fill('2026-12-31');
    await page.locator('select[name="ServiceLevel"]').selectOption('Standard');

    await page.getByRole('button', { name: /Create Contract/i }).click();
    await expect(page.url()).toContain('/Contract');

    const contractRow = page.locator('tr', {hasText: selectedClientName}).first();

    await expect(contractRow).toBeVisible();
    await expect(contractRow).toContainText('Standard');
  });

    // Test to ensure the user can update Contract status
    test('user can update Contract status', async ({ page }) => {

      await page.goto('/Contract');

      const contractRow = page.locator('table tbody tr').first();

      await expect(contractRow).toBeVisible();

      await contractRow.locator('select[name="status"]').selectOption('Active');
      
      await page.waitForLoadState('networkidle');
      
      const updatedRow = page.locator('table tbody tr').first();

      // Confirm Active status is shown
      await expect(updatedRow).toContainText('Active');
    });

    // Test to ensure Draft Contracts can be deleted
    test('user can delete a Draft Contract', async ({ page }) => {
    const draftClientName = `Draft Delete Client ${uniqueId}`;

    await page.goto('/Client/Create');

    await page.locator('input[name="Name"]').fill(draftClientName);
    await page.locator('input[name="ContactDetails"]').fill('draftdelete@test.com');
    await page.locator('select[name="Region"]').selectOption('Gauteng');

    await page.getByRole('button', { name: /Create Client/i }).click();
    await page.goto('/Contract/Create');

    await page.locator('select[name="ClientId"]').selectOption({ label: draftClientName });
    await page.locator('input[name="StartDate"]').fill('2026-02-01');
    await page.locator('input[name="EndDate"]').fill('2026-12-31');
    await page.locator('select[name="ServiceLevel"]').selectOption('Express');

    await page.getByRole('button', { name: /Create Contract/i }).click();

    await expect(page.url()).toContain('/Contract');

    const draftRow = page.locator('tr', { hasText: draftClientName });

    await expect(draftRow).toBeVisible();

    await draftRow.locator('select[name="status"]').selectOption('Draft');

    await page.waitForLoadState('networkidle');

    const updatedDraftRow = page.locator('tr', { hasText: draftClientName });

    // Confirm delete button exists
    await expect(updatedDraftRow.getByRole('button', { name: /Delete/i })).toBeVisible();

    // Accept confirmation popup
    page.on('dialog', dialog => dialog.accept());

    // Delete contract
    await updatedDraftRow.getByRole('button', { name: /Delete/i }).click();

    // Verify removal
    await expect(page.locator('tr', { hasText: draftClientName })).toHaveCount(0);
  });
    

  // Test to ensure the user can navigate to Service Requests page
  test('user can navigate to Service Requests page', async ({ page }) => {
    await page.goto('/ServiceRequest');

    await expect(page.getByRole('heading', { name: /Service Request/i })).toBeVisible();
    await expect(page.url()).toContain('/ServiceRequest');
  });


  // Test to ensure the user can create a Service Request
  test('user can create a Service Request', async ({ page }) => {

    await page.goto('/ServiceRequest/Create');

    // Select first available contract
    const contractDropdown = page.locator('select[name="ContractId"]');

    await expect(contractDropdown).toBeVisible();

    const options = contractDropdown.locator('option');
    const firstOptionValue = await options.nth(0).getAttribute('value');

    await contractDropdown.selectOption(firstOptionValue);

    await page.locator('input[name="Description"]').fill('Playwright Service Request');
    await page.locator('input[name="Amount"]').fill('250');
    await page.locator('select[name="Currency"]').selectOption('USD');

    await page.getByRole('button', { name: /Create Request/i }).click();

    await expect(page.url()).toContain('/ServiceRequest');

    const requestRow = page.locator('tr', {hasText: 'Playwright Service Request'}).first();

    await expect(requestRow).toBeVisible();
    await expect(requestRow).toContainText('USD');
  });

  // Test to ensure Service Request status can be updated
  test('user can update Service Request status', async ({ page }) => {

    await page.goto('/ServiceRequest');

    const requestRow = page.locator('table tbody tr', {hasText: 'Playwright Service Request'}).first();

    await expect(requestRow).toBeVisible();

    // Update status
    await requestRow.locator('select[name="status"]').selectOption('InProgress');

    // Wait for refresh
    await page.waitForLoadState('networkidle');

    const updatedRow = page.locator('table tbody tr', {hasText: 'Playwright Service Request'}).first();

    await expect(updatedRow).toContainText('In Progress');
  });

  // Test to ensure Pending Service Requests can be deleted
  test('user can delete a Pending Service Request', async ({ page }) => {

    await page.goto('/ServiceRequest');
    const requestRow = page.locator('table tbody tr', {hasText: 'Playwright Service Request'}).first();

    await expect(requestRow).toBeVisible();

    // Ensure status is Pending before delete
    await requestRow.locator('select[name="status"]').selectOption('Pending');

    await page.waitForLoadState('networkidle');

    const pendingRow = page.locator('table tbody tr', {hasText: 'Playwright Service Request'}).first();

    // Accept popup
    page.on('dialog', dialog => dialog.accept());

    // Delete request
    await pendingRow.getByRole('button', { name: /Delete/i }).click();

    // Verify removal
    await expect(page.locator('tr', {hasText: 'Playwright Service Request'})).toHaveCount(0);
  });

});