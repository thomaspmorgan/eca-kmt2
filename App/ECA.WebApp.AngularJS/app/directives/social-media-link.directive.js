'use strict';

/**
 * @ngdoc directive
 * @name staticApp.directive:socialmedialink
 * @description
 * # socialmedialink
 */
angular.module('staticApp')
  .directive('socialmedialink', function ($log, ConstantsService) {
      var directive = {
          template: '<a href="{{url}}" target="_blank">{{displayUrl}}</a>',
          scope: {
              socialMedia: '=model'
          },

          getSocialMediaUrl: function (socialMedia) {
              return socialMedia.value || "";
          },

          getDisplayUrl: function(url){
              var maxLength = 35;
              var displayUrl = url;
              if (displayUrl.length > maxLength) {
                  displayUrl = displayUrl.substring(0, maxLength) + '...';
              }
              return displayUrl;
          },

          link: function postLink(scope, element, attrs) {
              var socialMediaValue = scope.socialMedia.value;
              scope.$watch(attrs.model,
                  function (newValue, oldValue, scope) {
                      scope.url = directive.getSocialMediaUrl(newValue);
                      scope.displayUrl = directive.getDisplayUrl(scope.url);
                      
                  });
          }
      };
      return directive;
  });
