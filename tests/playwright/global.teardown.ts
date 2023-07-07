import { test as teardown } from '@playwright/test'
import { ZapClient } from 'zaproxy'
import fs from 'fs'

teardown('Generate ZAP report', async () => {
    if(process.env.zapReport) {
        console.log('Generating ZAP report')

        const zapOptions = {
            apiKey: process.env.zapApiKey || '',
            proxy: process.env.zapUrl || 'http://localhost:8080'
        }
        const zaproxy = new ZapClient(zapOptions)
        try {
            await zaproxy.core.htmlreport()
            .then(
                resp => {
                    if(!fs.existsSync('./reports')){
                        fs.mkdirSync('./reports')
                    }
                    fs.writeFileSync('./reports/ZAP-Report.html', resp)
                },
                err => {
                    console.log(`Error during report generation: ${err}`)
                }
            )
        } catch (err) {
            console.log(`Error contacting the ZAP API: ${err}`)
        }
    }
});