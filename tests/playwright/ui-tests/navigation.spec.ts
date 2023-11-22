import { test } from '@playwright/test'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'
import { HomePage } from '../page-object-model/home-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { ContactsPage } from '../page-object-model/trust/contacts-page'
import { FakeTestData } from '../fake-data/fake-test-data'
import { OverviewPage } from '../page-object-model/trust/overview-page'
import { PrivacyPage } from '../page-object-model/privacy-page'
import { CookiesPage } from '../page-object-model/cookies-page'
import { AcademiesDetailsPage } from '../page-object-model/trust/academies/details-page'
import { AcademiesOfstedRatingsPage } from '../page-object-model/trust/academies/ofsted-ratings-page'

test.describe('Navigation', () => {
  let homePage: HomePage
  let currentSearch: CurrentSearch
  let detailsPage: DetailsPage
  let contactsPage: ContactsPage
  let overviewPage: OverviewPage
  let privacyPage: PrivacyPage
  let cookiesPage: CookiesPage
  let academiesDetailsPage: AcademiesDetailsPage
  let academiesOfstedRatingsPage: AcademiesOfstedRatingsPage

  test.beforeEach(async ({ page }) => {
    const fakeTestData = new FakeTestData()
    currentSearch = new CurrentSearch()
    homePage = new HomePage(page, currentSearch)
    detailsPage = new DetailsPage(page, fakeTestData)
    contactsPage = new ContactsPage(page, fakeTestData)
    overviewPage = new OverviewPage(page, fakeTestData)
    academiesDetailsPage = new AcademiesDetailsPage(page, fakeTestData)
    academiesOfstedRatingsPage = new AcademiesOfstedRatingsPage(page, fakeTestData)
    privacyPage = new PrivacyPage(page)
    cookiesPage = new CookiesPage(page)
  })

  test('user should be able to navigate between different sections about a trust', async () => {
    await detailsPage.goTo()
    await detailsPage.trustNavigation.expect.toBeVisible()
    // Details => Contacts
    await detailsPage.trustNavigation.clickOn('Contacts')
    await contactsPage.expect.toBeOnTheRightPage()
    // Contacts => Details
    await contactsPage.trustNavigation.clickOn('Details')
    await detailsPage.expect.toBeOnTheRightPage()
    // Details => Overview
    await detailsPage.trustNavigation.clickOn('Overview')
    await overviewPage.expect.toBeOnTheRightPage()
    // Overview => Contacts
    await overviewPage.trustNavigation.clickOn('Contacts')
    await contactsPage.expect.toBeOnTheRightPage()
    // Contacts => Overview
    await contactsPage.trustNavigation.clickOn('Overview')
    await overviewPage.expect.toBeOnTheRightPage()
    // Overview => Details
    await overviewPage.trustNavigation.clickOn('Details')
    await detailsPage.expect.toBeOnTheRightPage()
    // Details => Academies in trust
    await detailsPage.trustNavigation.clickOn('Academies in this trust')
    await academiesDetailsPage.expect.toBeOnTheRightPage()
    // Academies in Trust => Overview
    await academiesDetailsPage.trustNavigation.clickOn('Overview')
    await overviewPage.expect.toBeOnTheRightPage()
    // Overview => Academies in trust
    await overviewPage.trustNavigation.clickOn('Academies in this trust')
    await academiesDetailsPage.expect.toBeOnTheRightPage()
    // Academies in trust => Details
    await academiesDetailsPage.trustNavigation.clickOn('Details')
    await detailsPage.expect.toBeOnTheRightPage()
  })

  test('user should be able to navigate between different tabs within Academies in trust section', async () => {
    await academiesDetailsPage.goTo()
    await academiesDetailsPage.subNavigation.clickOn('Ofsted ratings')
    await academiesOfstedRatingsPage.expect.toBeOnTheRightPage()
    await academiesOfstedRatingsPage.subNavigation.clickOn('Details')
    await academiesDetailsPage.expect.toBeOnTheRightPage()
  })

  test('user should be able to navigate to the different links within the footer', async () => {
    // Footer Via HomePage => Privacy
    await homePage.goTo()
    await homePage.footerNavigation.clickPrivacyPolicy()
    await privacyPage.expect.toBeOnTheRightPage()
    // Cookie banner Via HomePage => Cookie
    await homePage.goTo()
    await homePage.cookieBannerNavigation.clickCookiesPage()
    await cookiesPage.expect.toBeOnTheRightPage()
  })
})
