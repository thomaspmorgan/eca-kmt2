angular.module('staticApp')
  .directive('equalityFilter', ['stConfig', '$timeout', '$parse', 'ConstantsService', function (stConfig, $timeout, $parse, ConstantsService) {
      return {
          require: '^stTable',
          templateUrl: '../views/directives/equalityfilter.html',
          scope: {
                placeholder: '=placeholderText'
          },
          link: function (scope, element, attr, ctrl) {
              var tableCtrl = ctrl;
              var promise = null;
              var throttle = attr.stDelay || stConfig.search.delay;
              var event = attr.stInputEvent || stConfig.search.inputEvent;
              var propertyName = attr.stSearchProperty;

              scope.comparisonType = '';
              scope.value = '';
              scope.operation = '';
              scope.onLessThanClick = function () {
                  scope.comparisonType = ConstantsService.lessThanComparisonType;
                  scope.operation = '&lt';
                  doFilter();
              };

              scope.onGreaterThanClick = function () {
                  scope.comparisonType = ConstantsService.greaterThanComparisonType;
                  scope.operation = '&gt';
                  doFilter();
              };

              scope.onEqualClick = function () {
                  scope.comparisonType = ConstantsService.equalComparisonType;
                  scope.operation = '=';
                  doFilter();
              }

              scope.onChange = function () {
                  doFilter();
              }

              var doFilter = function () {
                  if (!propertyName) {
                      throw Error("The property name is unknown.  Be sure to set the 'st-search-property' attribute value.");
                  }
                  if (scope.value && scope.value.length > 0 && scope.comparisonType && scope.comparisonType.length > 0) {
                      var predicateObject = {
                          comparison: scope.comparisonType,
                          value: parseFloat(scope.value)
                      };
                      tableCtrl.search(predicateObject, propertyName);
                  }
              }
          }
      };
  }]);