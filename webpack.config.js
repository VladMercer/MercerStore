const path = require('path');
const webpack = require('webpack'); 

module.exports = {
    entry: {
        mainApp: './wwwroot/components/mainApp.jsx',
        searchBar: './wwwroot/components/logic/searchBar/index.js',
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot/dist'),
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: 'babel-loader',
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
            },
        ],
    },
    plugins: [
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': JSON.stringify('development'),
        }),
    ],
    mode: 'development', 
    resolve: {
        extensions: ['.js', '.jsx'],
    },
};