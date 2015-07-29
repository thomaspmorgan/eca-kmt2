MKDIR C:\Users\buildguest\AppData\Roaming\npm
call .bin\npm install
call .bin\npm dedupe
call .bin\bower install
call .bin\node node_modules\gulp\bin\gulp.js