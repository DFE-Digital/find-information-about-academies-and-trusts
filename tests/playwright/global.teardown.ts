import { test as teardown } from '@playwright/test'
import zaproxy from 'zaproxy'

teardown('Generate ZAP report', async () => {
  // eslint-disable-next-line @typescript-eslint/strict-boolean-expressions
  if (process.env.ZAP) {
    console.log('\nGenerating ZAP report')

    const zapOptions = {
      apiKey: process.env.ZAP_API_KEY,
      proxy: {
        host: process.env.ZAP_ADDRESS,
        port: process.env.ZAP_PORT
      }
    }

    // eslint-disable-next-line new-cap
    const zap = new zaproxy(zapOptions)
    // Wait for passive scanner to finish scanning before generating report
    let recordsRemaining = 100
    while (recordsRemaining !== 0) {
      await zap.pscan.recordsToScan()
        .then((resp) => {
          try {
            recordsRemaining = parseInt(resp.recordsToScan, 10)
          } catch (err) {
            if (err instanceof Error) {
              console.log(`Error converting result: ${err.message}`)
            } else {
              console.log('Unknown error during results conversion')
            }
            recordsRemaining = 0
          }
        })
        .catch((err: string) => {
          console.log(`Error from the ZAP API: ${err}`)
          recordsRemaining = 0
        })
    }

    await zap.reports.generate({
      title: 'Report',
      template: 'traditional-html',
      reportfilename: 'ZAP-Report.html'
    })
      .then((resp) => {
        console.log(`${JSON.stringify(resp)}`)
      })
      .catch((err: string) => {
        console.log(`Error from ZAP API: ${err}`)
      })
  }
})
