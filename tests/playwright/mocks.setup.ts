import { test as setup } from '@playwright/test'
import { MockTrustsProvider } from './mocks/mock-trusts-provider'

setup('mock trust provider', async () => {
  const mockTrustProvider = new MockTrustsProvider()
  const searchTerm = 'trust'

  for (const keyword in MockTrustsProvider.fakeTrustsResponseData) {
    await mockTrustProvider.registerGetTrustsBy(keyword)
  }

  for (const trust of MockTrustsProvider.fakeTrustsResponseData[searchTerm]) {
    await mockTrustProvider.registerGetTrustByUkprn(trust.GroupName, trust.Ukprn)
  }
})
