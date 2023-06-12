const path = require('path')
const CopyPlugin = require('copy-webpack-plugin')

module.exports = {
  plugins: [
    new CopyPlugin({
      patterns: [
        {
          from: '*.{png, jpg, jpeg, gif, svg}',
          to: path.join(__dirname, 'wwwroot/dist/images'),
          context: 'node_modules/dfe-frontend-alpha/packages/assets'
        },
        {
          from: 'dfefrontend.js',
          to: path.join(__dirname, 'wwwroot/dist/javascripts'),
          context: 'node_modules/dfe-frontend-alpha/dist'
        }
      ]
    })
  ],
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist')
  }
}
