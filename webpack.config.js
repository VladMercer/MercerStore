const path = require('path');

module.exports = {
    entry: './wwwroot/js/main.js',
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist'), 
        filename: 'bundle.js', 
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/, 
                use: {
                    loader: 'babel-loader', 
                },
            },
        ],
    },
    resolve: {
        extensions: ['.js', '.jsx'],
    },
 
};
