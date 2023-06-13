const path = require('path')
const CopyPlugin = require('copy-webpack-plugin')

module.exports = {
  plugins: [
    new CopyPlugin({
      patterns: [
        {
          from: 'images',
          to: path.join(__dirname, 'wwwroot/dist/images/govuk'),
          context: 'node_modules/govuk-frontend/govuk/assets'
        },
        {
          from: '*.{png, jpg, jpeg, gif, svg}',
          to: path.join(__dirname, 'wwwroot/dist/images'),
          context: 'node_modules/dfe-frontend-alpha/packages/assets'
        },
        {
          from: path.join(__dirname, 'src/images'),
          to: path.join(__dirname, 'wwwroot/dist/images')
        }
      ]
    })
  ],
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist')
  }
}
