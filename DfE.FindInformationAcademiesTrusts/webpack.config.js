const path = require('path')
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const CopyPlugin = require('copy-webpack-plugin')

module.exports = {
  entry: ['./assets/index.js', './assets/index.scss'],
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist')
  },
  plugins: [
    new MiniCssExtractPlugin(),
    new CopyPlugin({
      patterns: [
        {
          from: '*.{png,svg}',
          to: path.join(__dirname, 'wwwroot/dist/images'),
          context: 'node_modules/govuk-frontend/govuk/assets/images'
        },
        {
          from: 'dfefrontend.js',
          to: path.join(__dirname, 'wwwroot/dist/javascripts'),
          context: 'node_modules/dfe-frontend-alpha/dist'
        },
        {
          from: path.join(__dirname, 'assets/images'),
          to: path.join(__dirname, 'wwwroot/dist/images')
        }
      ]
    })
  ],
  module: {
    rules: [
      {
        test: /\.s[ac]ss$/i,
        use: [
          MiniCssExtractPlugin.loader,
          // Translates CSS into CommonJS
          'css-loader',
          // Compiles Sass to CSS
          'sass-loader'
        ]
      },
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            presets: ['@babel/preset-env']
          }
        }
      }
    ]
  }
}
