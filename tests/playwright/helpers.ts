export const javaScriptContexts = [{ name: 'enabled', isEnabled: true }, { name: 'disabled', isEnabled: false }]

export const formatDateAsExpected = (date: string | Date): string => {
  const options: Intl.DateTimeFormatOptions = {
    day: 'numeric', month: 'short', year: 'numeric'
  }
  const formattedDate = new Date(date).toLocaleDateString('en-GB', options).split(' ')

  // fixing an issue where Sept is 4 characters in Chromium using `toLocaleDateString`
  const shortMonth = formattedDate[1].slice(0, 3)
  formattedDate.splice(1, 1, shortMonth)
  return formattedDate.join(' ')
}
