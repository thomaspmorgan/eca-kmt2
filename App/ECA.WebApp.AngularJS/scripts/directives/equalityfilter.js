﻿angular.module('staticApp')
  .directive('equalityFilter', ['stConfig', '$timeout', '$filter', '$parse', 'ConstantsService', function (stConfig, $timeout, $filter, $parse, ConstantsService) {
      return {
          require: '^stTable',
          templateUrl: '../views/directives/equalityfilter.html',
          scope: {
              property: '=property',
              propertyType: '=propertytype'
          },
          link: function (scope, element, attr, ctrl) {
              var tableCtrl = ctrl;
              var promise = null;
              var throttle = attr.stDelay || stConfig.search.delay;
              var event = attr.stInputEvent || stConfig.search.inputEvent;
              scope.displayValues = [];
              scope.isLessThanDatePickerOpen = false;
              scope.isGreaterThanDatePickerOpen = false;
              if (scope.propertyType !== 'date'
                  && scope.propertyType !== 'int'
                  && scope.propertyType !== 'float') {
                  throw Error('The property type [' + scope.propertyType + '] is not supported.');
              }

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

              scope.onLessThanDatePickerOpen = function ($event) {
                  scope.stopEvents($event);
                  scope.isLessThanDatePickerOpen = true;
              }

              scope.onGreaterThanDatePickerOpen = function ($event) {
                  scope.stopEvents($event);
                  scope.isGreaterThanDatePickerOpen = true;
              }

              scope.onLessThanDatePickerChange = function () {
                  updateDisplayValue();
                  doFilter();
              }

              scope.onGreaterThanDatePickerChange = function () {
                  updateDisplayValue();
                  doFilter();
              }

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

              scope.onClearClick = function () {
                  scope.lessThanFilter.value = '';
                  scope.greaterThanFilter.value = '';
                  scope.equalFilter.value = '';
                  updateDisplayValue();
                  doFilter();
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

              function getFilterValueAsString(filterValue) {
                  if (angular.isDate(filterValue)) {
                      return $filter('date')(filterValue, 'mediumDate');
                  }
                  else {
                      return filterValue;
                  }
              }

              function updateDisplayValue() {
                  scope.displayValues = [];
                  if (scope.lessThanFilter.value) {
                      scope.displayValues.push("< " + getFilterValueAsString(scope.lessThanFilter.value));
                  }
                  if (scope.greaterThanFilter.value) {
                      scope.displayValues.push("> " + getFilterValueAsString(scope.greaterThanFilter.value));
                  }
                  if (scope.equalFilter.value) {
                      scope.displayValues.push("= " + getFilterValueAsString(scope.equalFilter.value));
                  }
                  if (scope.displayValues.length === 0) {
                      scope.displayValues.push('Add filters...');
                  }
              }
              updateDisplayValue();

              function parseValue(value) {
                  if (!scope.propertyType) {
                      throw Error('The property type is not defined.');
                  }
                  if (scope.propertyType === 'float') {
                      return parseFloat(value);
                  }
                  else if (scope.propertyType === 'int') {
                      return parseInt(value, 10);
                  }
                  else {
                      throw Error('The propertyType [' + scope.propertyType + '] is not supported.');
                  }
              }

              function doFilter() {
                  var predicateObjects = [];
                  if (scope.propertyType === 'date') {
                      if (scope.lessThanFilter.value) {
                          predicateObjects.push({
                              comparison: scope.lessThanFilter.comparison,
                              value: scope.lessThanFilter.value
                          });
                      }
                      if (scope.greaterThanFilter.value) {
                          predicateObjects.push({
                              comparison: scope.greaterThanFilter.comparison,
                              value: scope.greaterThanFilter.value
                          });
                      }
                  }
                  else {
                      if (scope.lessThanFilter.value && scope.lessThanFilter.value.trim() !== '-') {
                          predicateObjects.push({
                              comparison: scope.lessThanFilter.comparison,
                              value: parseValue(scope.lessThanFilter.value)
                          });
                      }
                      if (scope.greaterThanFilter.value && scope.greaterThanFilter.value.trim() !== '-') {
                          predicateObjects.push({
                              comparison: scope.greaterThanFilter.comparison,
                              value: parseValue(scope.greaterThanFilter.value)
                          });
                      }
                      if (scope.equalFilter.value && scope.equalFilter.value.trim() !== '-') {
                          predicateObjects.push({
                              comparison: scope.equalFilter.comparison,
                              value: parseValue(scope.equalFilter.value)
                          });
                      }
                  }
                  
                  promise = $timeout(function () {
                      tableCtrl.search(predicateObjects, scope.property);
                      promise = null;
                  }, throttle);

                  // view -> table state
                  element.bind(event, function (evt) {
                      evt = evt.originalEvent || evt;
                      if (promise !== null) {
                          $timeout.cancel(promise);
                      }
                  });
              }
          }
      };
  }]);