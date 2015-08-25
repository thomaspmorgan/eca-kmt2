'use strict';

var gulp = require('gulp');
var browserSync = require('browser-sync');
var reload = browserSync.reload;

gulp.task('clean', function (cb) {
    var del = require('del');
    del('dist/**/*', cb);
});

gulp.task('styles', ['clean'], function () {
    var sass = require('gulp-sass');
    gulp.src('styles/main.scss')
        .pipe(sass())
        .pipe(gulp.dest('dist/styles'));
});

gulp.task('localStyles', ['clean'], function () {
    var sass = require('gulp-sass');
    var sourcemaps = require('gulp-sourcemaps');
    gulp.src('styles/main.scss')
        .pipe(sourcemaps.init())
        .pipe(sass())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('styles'))
        .pipe(reload({stream: true}));
});

gulp.task('html', ['styles'], function () {
    var useref = require('gulp-useref');
    var gulpIf = require('gulp-if');
    var uglify = require('gulp-uglify');
    var minifyCss = require('gulp-minify-css');
    var replace = require('gulp-replace');

    var assets = useref.assets();

    gulp.src('index.html')
        .pipe(assets)
        .pipe(gulpIf('*.js', uglify({mangle: false})))
        .pipe(gulpIf('*.css', replace('select2.png', '../images/select2.png')))
        .pipe(gulpIf('*.css', minifyCss()))
        .pipe(assets.restore())
        .pipe(useref())
        .pipe(gulp.dest('dist'));
});

gulp.task('copy', ['clean'], function () {
    gulp.src('images/**/*.*')
        .pipe(gulp.dest('dist/images'));
    gulp.src('views/**/*.*')
        .pipe(gulp.dest('dist/views'));
    gulp.src('bower_components/material-design-iconic-font/fonts/**/*.*')
        .pipe(gulp.dest('dist/fonts'));
    gulp.src('bower_components/select2/*.png')
        .pipe(gulp.dest('dist/images'));
});

gulp.task('serve', ['styles'], function () {
    browserSync({
        notify: false,
        port: 5556,
        server: {
            routes: {
                '/bower_components': 'bower_components'
            }
        }
    });

    // watch for changes
    gulp.watch([
      '*.html',
      'scripts/**/*.js',
      'images/**/*'
    ]).on('change', reload);

    gulp.watch('styles/**/*.scss', ['localStyles']);
});

gulp.task('default', ['clean', 'styles', 'html', 'copy'], function () {

});