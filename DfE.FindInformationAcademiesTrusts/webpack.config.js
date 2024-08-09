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
          from: 'govuk-*.{png,svg}',
          to: path.join(__dirname, 'wwwroot/dist/images'),
          context: 'node_modules/govuk-frontend/dist/govuk/assets/images'
        },
        {
          from: '*.{png,svg}',
          to: path.join(__dirname, 'wwwroot/dist/images'),
          context: 'node_modules/dfe-frontend/packages/assets'
        },
        {
          from: '*.{ico,png}',
          to: path.join(__dirname, 'wwwroot/dist/images'),
          context: path.join(__dirname, 'assets/images')
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
