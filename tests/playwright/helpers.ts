export const javaScriptContexts = [{ name: 'enabled', isEnabled: true }, { name: 'disabled', isEnabled: false }]

export const formatDateAsExpected = (date: string): string => {
  const options: Intl.DateTimeFormatOptions = {
    day: 'numeric', month: 'short', year: 'numeric'
  }
  return new Date(date).toLocaleDateString('en-GB', options)
}
