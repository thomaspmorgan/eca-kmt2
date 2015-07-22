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
              var http = "http";
              var facebookUrl = 'facebook.com';
              var linkedInUrl = 'linkedin.com';
              var weiboUrl = 'weibo.com';
              var twitterUrl = 'twitter.com';
              var pinterestUrl = 'pinterest.com';
              var googleUrl = 'google.com';
              var instagramUrl = 'instagram.com';

              var lowerCaseValue = socialMedia.value || '';
              lowerCaseValue = lowerCaseValue.toLowerCase();
              var isFacebook = lowerCaseValue.indexOf(facebookUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isLinkedIn = lowerCaseValue.indexOf(linkedInUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isWeibo = lowerCaseValue.indexOf(weiboUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isTwitter = lowerCaseValue.indexOf(twitterUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isPinterest = lowerCaseValue.indexOf(pinterestUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isGoogle = lowerCaseValue.indexOf(googleUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isInstragram = lowerCaseValue.indexOf(instagramUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              if (isFacebook || isLinkedIn || isWeibo || isTwitter || isPinterest || isGoogle || isInstragram) {
                  $log.info('Social media value has url.');
                  return socialMedia.value;
              }
              else {
                  $log.info('Calculating social media value.');
                  var value = socialMedia.value || '';
                  var type = socialMedia.socialMediaType || '';
                  type = type.toLowerCase().trim();

                  if (type === ConstantsService.socialMediaType.facebook.value.trim().toLowerCase()) {
                      return 'https://facebook.com/' + value;
                  }
                  if (type === ConstantsService.socialMediaType.linkedin.value.trim().toLowerCase()) {
                      return 'https://linkedin.com/' + value;
                  }
                  if (type === ConstantsService.socialMediaType.weibo.value.trim().toLowerCase()) {
                      return 'http://weibo.com/' + value;
                  }
                  if (type === ConstantsService.socialMediaType.twitter.value.trim().toLowerCase()) {
                      return 'https://twitter.com/' + value;
                  }
                  if (type === ConstantsService.socialMediaType.instagram.value.trim().toLowerCase()) {
                      return 'https://instagram.com/' + value;
                  }
                  if (type === ConstantsService.socialMediaType.pinterest.value.trim().toLowerCase()) {
                      return 'https://www.pinterest.com/' + value;
                  }
                  if (type === ConstantsService.socialMediaType.google.value.trim().toLowerCase()) {
                      return 'https://plus.google.com/' + value;
                  }
                  return 'google.com';
              }
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
