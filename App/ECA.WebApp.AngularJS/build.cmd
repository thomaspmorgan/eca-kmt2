MKDIR C:\Users\buildguest\AppData\Roaming\npm
RMDIR .\dist /S /Q
RMDIR .\node_modules /S /Q
RMDIR .\bowser_components /S /Q
call .bin\npm install
call .bin\bower install
call .bin\node node_modules\gulp\bin\gulp.js