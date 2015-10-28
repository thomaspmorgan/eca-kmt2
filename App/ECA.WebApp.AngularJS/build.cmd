MKDIR C:\Users\buildguest\AppData\Roaming\npm
call npm install
call node node_modules\bower\bin\bower install
call npm rebuild node-sass
call node node_modules\gulp\bin\gulp.js
