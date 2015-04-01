//'use strict';
//describe('staticApp.overview controller', function () {
//    beforeEach(module('staticApp.overview'));

//    describe('overview controller', function () {
//        it('should find focus by id', inject(function ($controller) {
//            var controller = $controller('OverviewCtrl');
//            expect(controller).toBeDefined();
//        }));
//    });

//});

'use strict';

describe('myApp.view1 module', function () {

    beforeEach(module('myApp.view1'));

    describe('view1 controller', function () {

        it('should ....', inject(function ($controller) {
            //spec body
            var view1Ctrl = $controller('View1Ctrl');
            expect(view1Ctrl).toBeDefined();
        }));

    });
});