import { test as setup } from '@playwright/test'
import { MockTrustsProvider } from './mocks/mock-trusts-provider'

setup('mock trust provider', async () => {
  const mockTrustProvider = new MockTrustsProvider()
  await mockTrustProvider.registerGetTrusts()

  for (const trust of MockTrustsProvider.fakeTrustsResponseData) {
    await mockTrustProvider.registerGetTrustByUkprn(trust.GroupName, trust.Ukprn)
  }
})
