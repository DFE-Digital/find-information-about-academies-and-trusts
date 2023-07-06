import { test as teardown } from '@playwright/test'

teardown('Generate ZAP report', async ({ request }) => {
    if(process.env.ZAP) {
        console.log('\nGenerating ZAP report')

        // Wait for passive scanner to finish scanning before generating report
        let recordsRemaining = 100;
        while(recordsRemaining != 0) {
            const pscanRecords = await request.get(`${process.env.ZAP_URL}/JSON/pscan/view/recordsToScan/`, {
                headers: {
                    'X-ZAP-API-Key': process.env.ZAP_API_KEY ? process.env.ZAP_API_KEY : ''
                }
            })
            await pscanRecords.json()
            .then(resp => {
                recordsRemaining = parseInt(resp.recordsToScan, 10)
            })
            .catch(err => {
                console.log(`Error contacting the ZAP API: ${err}`)
                // avoid infinite loop on error state
                recordsRemaining = 0
            })
        }

        const report = await request.post(`${process.env.ZAP_URL}/JSON/reports/action/generate/`, {
            headers: {
                'X-ZAP-API-Key': process.env.ZAP_API_KEY ? process.env.ZAP_API_KEY : ''
            },
            form: {
                title: 'Report',
                template: 'traditional-html',
                reportFileName: 'ZAP-report'
            }
        })

        await report.json()
        .then(resp => {
            console.log(`Generated report at ${ resp.generate }`)
        })
        .catch(err => {
            console.log(`Error generating report from ZAP: ${err}`)
        })
    }
});
