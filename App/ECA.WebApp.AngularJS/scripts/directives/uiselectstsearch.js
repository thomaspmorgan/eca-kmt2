angular.module('staticApp')
  .directive('uiSelectStSearch', ['stConfig', '$timeout', '$parse', 'ConstantsService', function (stConfig, $timeout, $parse, ConstantsService) {
      return {
          require: '^stTable',
          link: function (scope, element, attr, ctrl) {
              var tableCtrl = ctrl;
              var promise = null;
              var throttle = attr.stDelay || stConfig.search.delay;
              var event = attr.stInputEvent || stConfig.search.inputEvent;
              var propertyName = attr.uiSelectStSearchProperty;
              var modelIdPropertyName = attr.uiSelectStSearchModelId || "id";
              var currentIds = [];
              
              var getModelId = function(model){
                  return model[modelIdPropertyName];
              }

              var getModel = function (args) {
                  return args[1].$item;
              }

              attr.onSelect = function () {
                  var id = getModelId(getModel(arguments));
                  currentIds.push(id);
                  onChange();                  
              }
              attr.onRemove = function () {
                  var id = getModelId(getModel(arguments));
                  var index = currentIds.indexOf(id);
                  currentIds.splice(index, 1);
                  onChange();
              }

              var onChange = function () {
                  var predicateObject = {
                      comparison: ConstantsService.containsAnyComparisonType,
                      ids: currentIds
                  };
                  tableCtrl.search(predicateObject, propertyName);
              }
          }
      };
  }]);