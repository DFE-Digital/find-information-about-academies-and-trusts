import { htmlReport } from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js'
import { textSummary } from 'https://jslib.k6.io/k6-summary/0.0.1/index.js'
import { browser } from 'k6/browser'
import { Trend } from 'k6/metrics'

export const options = {
  scenarios: {
    ui: {
      executor: 'shared-iterations',
      options: {
        browser: {
          type: 'chromium',
        },
      },
    },
  },
  thresholds: {
    browser_http_req_failed: ['rate<0.01'],
    page_load_time: ['p(95) < 1000']
  },
}

const baseUrl = `${__ENV.BASE_URL}`

const trend = new Trend('page_load_time', true);

export default async function () {

  const context = await browser.newContext()
  const page = await context.newPage()

  // Set extra headers for auth and User-Agent to be for k6
  await page.setExtraHTTPHeaders({
    'Authorization': `Bearer ${__ENV.AUTHORIZATION_HEADER}`,
    'User-Agent': 'FindInformationAcademiesTrusts/1.0 k6-browser'
  })

  // Go through each page for the trust and capture the page load timings
  try {
    await checkPageLoadTiming(page, `${baseUrl}/trusts/details?uid=5143`)

    await checkPageLoadTiming(page, `${baseUrl}/trusts/contacts?uid=5143`)

    await checkPageLoadTiming(page, `${baseUrl}/trusts/overview?uid=5143`)

    await checkPageLoadTiming(page, `${baseUrl}/trusts/academies/details?uid=5143`)

    await checkPageLoadTiming(page, `${baseUrl}/trusts/academies/ofsted-ratings?uid=5143`)

    await checkPageLoadTiming(page, `${baseUrl}/trusts/academies/pupil-numbers?uid=5143`)
  }
  finally {
    await page.close()
  }
}

async function checkPageLoadTiming(page, url) {
  await page.goto(url)
  await page.evaluate(() => window.performance.mark('page_load_time'));

  let totalActionTime = await page.evaluate(
    () =>
      JSON.parse(JSON.stringify(window.performance.getEntriesByName('page_load_time')))[0]
        .startTime
  )

  trend.add(totalActionTime)

  Promise.resolve()
}

export function handleSummary(data) {
  return {
    'summary.html': htmlReport(data),
    stdout: textSummary(data, { indent: ' ', enableColors: true }),
  }
}
