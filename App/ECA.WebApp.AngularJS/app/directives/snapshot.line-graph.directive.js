'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive: lineGraph
 * @description
 * # data lineGraph
 */
angular.module('staticApp')
  .directive('lineGraph', function (d3, nv) {
      return {
          restrict: 'E',
          scope: {
              data: '=',
              height: '@',
              width: '@'
          },
          template: '<svg ng-attr-height="{{ height }}" ng-attr-width="{{ width }}"></svg>',
          link: function (scope, element) {
              var svg = element.find('svg'),
                  chart;
              
              var update = function () {
                  var test = [{
                      "key": "Budget",
                      "values": [
                        {
                            "x": 2014,
                            "y": 233
                        },
                        {
                            "x": 2013,
                            "y": 219
                        },
                        {
                            "x": 2012,
                            "y": 65
                        },
                        {
                            "x": 2011,
                            "y": 56
                        },
                        {
                            "x": 2010,
                            "y": 62
                        }
                      ]
                  }];

                  d3.select(svg[0])
                  .datum(test)
                  .call(chart);
              };
              
              //scope.$watch(function () { return angular.toJson(scope.data); }, function () {
              //    if (chart) {
              //        update();
              //    }
              //});

              scope.$on('chartloaded', update);
              
              nv.addGraph(function () {
                  chart = nv.models.lineChart()
                    .showLegend(false)
                    .showYAxis(true)
                    .showXAxis(true);

                  //chart.XAxis
                  //    .axisLabel('x');

                  //chart.YAxis
                  //    .axisLabel('y');

                  nv.utils.windowResize(function () {
                      chart.update();
                  });

                  scope.$emit('chartloaded');

                  return chart;
              });
          }
      };
  });
