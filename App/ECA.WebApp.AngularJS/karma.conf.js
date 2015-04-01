module.exports = function (config) {
    config.set({

        basePath: '../..',

        files: [
          'bower_components/angular/angular.js',
          'bower_components/angular-route/angular-route.js',
          'bower_components/angular-mocks/angular-mocks.js',
          'scripts/**/*_test.js',
          '/scrips/controllers/project/overview_test.js'
        ],

        autoWatch: true,

        frameworks: ['jasmine'],

        browsers: ['Chrome'],

        plugins: [
                'karma-chrome-launcher',
                'karma-firefox-launcher',
                'karma-jasmine',
                'karma-junit-reporter'
        ],

        junitReporter: {
            outputFile: 'test_out/unit.xml',
            suite: 'unit'
        },
        logLevel: config.LOG_INFO

    });
};