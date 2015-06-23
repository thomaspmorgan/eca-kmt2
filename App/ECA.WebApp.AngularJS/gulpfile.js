var gulp = require('gulp');

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

gulp.task('localstyles', ['clean'], function () {
    var sass = require('gulp-sass');
    gulp.src('styles/main.scss')
        .pipe(sass())
        .pipe(gulp.dest('styles'));
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

gulp.task('default', ['clean', 'styles', 'html', 'copy', 'localstyles'], function () {

});