import { test as setup } from '@playwright/test'

const authFile = '.auth/user.json'
const tenantId = process.env.AuthTenantId ?? ''
const baseUrl = process.env.PLAYWRIGHT_BASEURL ?? 'http://localhost:5163/'

setup('authenticate', async ({ request }) => {
  await request.post(`https://login.microsoftonline.com/${tenantId}/oauth2/v2.0/authorize`, {
    form: {
      response_type: 'code',
      client_id: process.env.AuthClientID ?? '',
      scope: 'openid profile email',
      username: process.env.AuthTestUserName ?? '',
      password: process.env.AuthTestUserPassword ?? '',
      redirect_uri: baseUrl + 'signin-oidc',
      state: '12345',
      response_mode: 'query'
    }
  })
  await request.storageState({ path: authFile })
})
