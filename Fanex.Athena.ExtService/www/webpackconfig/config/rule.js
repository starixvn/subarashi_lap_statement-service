var commonRules = [
    {
        test: require.resolve("jquery"),
        use: [{
            loader: "expose-loader",
            options: {
                exposes: ["jQuery", "$"]
            }
        }]
    }
];

module.exports = {
    commonRules: commonRules
};