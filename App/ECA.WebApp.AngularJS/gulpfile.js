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


gulp.task('html', ['clean'], function () {
    var useref = require('gulp-useref');
    var gulpIf = require('gulp-if');
    var uglify = require('gulp-uglify');
    var minifyCss = require('gulp-minify-css');

    var assets = useref.assets();

    gulp.src('index.html')
        .pipe(assets)
        .pipe(gulpIf('*.js', uglify({mangle: false})))
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
});

gulp.task('default', ['clean', 'styles', 'html', 'copy'], function () {

});

