angular.module('staticApp')
  .directive('equalityFilter', ['stConfig', '$timeout', '$parse', 'ConstantsService', function (stConfig, $timeout, $parse, ConstantsService) {
      return {
          require: '^stTable',
          templateUrl: '../views/directives/equalityfilter.html',
          scope: {
              property: '=property'
          },
          link: function (scope, element, attr, ctrl) {
              var tableCtrl = ctrl;
              var promise = null;
              var throttle = attr.stDelay || stConfig.search.delay;
              var event = attr.stInputEvent || stConfig.search.inputEvent;
              scope.displayValues = [];

              scope.lessThanFilter = {
                  comparison: ConstantsService.lessThanComparisonType,
                  value: ''
              };

              scope.greaterThanFilter = {
                  comparison: ConstantsService.greaterThanComparisonType,
                  value: ''
              };

              scope.equalFilter = {
                  comparison: ConstantsService.equalComparisonType,
                  value: ''
              };

              scope.stopEvents = function ($event) {
                  $event.preventDefault();
                  $event.stopPropagation();
              };

              scope.getInputElement = function (suffix) {
                  var target = null;
                  var elements = element.find('input');
                  angular.forEach(elements, function (e, index) {
                      if (e.id === suffix) {
                          target = e;
                      }
                  });
                  return target;
              }

              scope.getLessThanInputElement = function () {
                  return scope.getInputElement('LessThan');
              }

              scope.getGreaterThanInputElement = function () {
                  return scope.getInputElement('GreaterThan');
              }

              scope.getEqualInputElement = function () {
                  return scope.getInputElement('Equal');
              }

              function bindEnterToElement(element) {
                  var inputElement = angular.element(element);
                  inputElement.bind("keyup", function (event) {
                      updateDisplayValue();
                      doFilter();
                  });
              }
              bindEnterToElement(scope.getLessThanInputElement());
              bindEnterToElement(scope.getGreaterThanInputElement());
              bindEnterToElement(scope.getEqualInputElement());

              function updateDisplayValue() {
                  scope.displayValues = [];
                  if (scope.lessThanFilter.value) {
                      scope.displayValues.push("< " + scope.lessThanFilter.value);
                  }
                  if (scope.greaterThanFilter.value) {
                      scope.displayValues.push("> " + scope.greaterThanFilter.value);
                  }
                  if (scope.equalFilter.value) {
                      scope.displayValues.push("= " + scope.equalFilter.value);
                  }
              }

              function doFilter() {
                  var predicateObjects = [];
                  if (scope.lessThanFilter.value) {
                      predicateObjects.push({
                          comparison: scope.lessThanFilter.comparison,
                          value: parseFloat(scope.lessThanFilter.value)
                      });
                  }
                  if (scope.greaterThanFilter.value) {
                      predicateObjects.push({
                          comparison: scope.greaterThanFilter.comparison,
                          value: parseFloat(scope.greaterThanFilter.value)
                      });
                  }
                  if (scope.equalFilter.value) {
                      predicateObjects.push({
                          comparison: scope.equalFilter.comparison,
                          value: parseFloat(scope.equalFilter.value)
                      });
                  }
                  tableCtrl.search(predicateObjects, scope.property);
                  //promise = $timeout(function () {
                  //    //tableCtrl.search(evt.target.value, attr.stSearch || '');
                  //    tableCtrl.search(predicateObjects, scope.property);
                  //    promise = null;
                  //}, throttle);


                  // view -> table state
                  //element.bind(event, function (evt) {
                  //    evt = evt.originalEvent || evt;
                  //    if (promise !== null) {
                  //        $timeout.cancel(promise);
                  //    }

                      
                  //});

              }

              //element.bind(event, function (evt) {
              //    evt = evt.originalEvent || evt;
              //    if (promise !== null) {
              //        $timeout.cancel(promise);
              //    }

              //    promise = $timeout(function () {
              //        tableCtrl.search(evt.target.value, attr.stSearch || '');
              //        promise = null;
              //    }, throttle);
              //});





          }
      };
  }]);