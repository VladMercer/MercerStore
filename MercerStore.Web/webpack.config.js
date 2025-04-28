const path = require('path');
const webpack = require('webpack');

module.exports = {
    entry: {
        mainApp: './wwwroot/mainApp.js',
        searchBar: './wwwroot/features/searchBar/index.js',
    },
    output: {
        filename: '[name].bundle.js',
        chunkFilename: '[name].[contenthash].chunk.js',
        path: path.resolve(__dirname, 'wwwroot/dist'),
        clean: true,
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: [
                            '@babel/preset-env',
                            [
                                '@babel/preset-react',
                                {runtime: 'automatic'},
                            ],
                        ],
                    },
                },

            },
            {
                test: /\.svg$/,
                use: ['svg-url-loader'],
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
    devtool: 'source-map',
    resolve: {
        extensions: ['.js', '.jsx'],
    },
    devServer: {
        static: [
            {
                directory: path.resolve(__dirname, 'wwwroot/favicon'),
                publicPath: '/',
            },
            {
                directory: path.resolve(__dirname, 'wwwroot/adminApp/admin-app/public'),
                publicPath: '/admin',
            },
        ],
        open: true,
        compress: true,
        port: 8080,
        hot: true,
    },
};