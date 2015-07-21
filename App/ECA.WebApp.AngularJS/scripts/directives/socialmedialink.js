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
          template: '<a href="{{url}}" target="_blank">{{url}}</a>',
          scope: {
              socialMedia: '=model'
          },

          getSocialMediaUrl: function (socialMedia) {
              var http = "http";
              var facebookUrl = 'facebook.com';
              var linkedInUrl = 'linkedin.com';
              var weiboUrl = 'weibo.com';
              var twitterUrl = 'twitter.com';

              var lowerCaseValue = socialMedia.socialMediaValue.toLowerCase();
              var isFacebook = lowerCaseValue.indexOf(facebookUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isLinkedIn = lowerCaseValue.indexOf(linkedInUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isWeibo = lowerCaseValue.indexOf(weiboUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              var isTwitter = lowerCaseValue.indexOf(twitterUrl) >= 0 && lowerCaseValue.indexOf(http) >= 0;
              if (isFacebook || isLinkedIn || isWeibo || isTwitter) {
                  $log.info('Social media value has url.');
                  return socialMedia.value;
              }
              else {
                  $log.info('Calculating social media value.');
                  var value = socialMedia.socialMediaValue;
                  var type = socialMedia.socialMediaType.toLowerCase().trim();
                  if (!ConstantsService.socialMediaType[type]) {
                      $log.error('The social media type [' + type + '] is not recognized.');
                  }
                  
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
                  return 'google.com';
              }
          },

          link: function postLink(scope, element, attrs) {
              var sm = scope.socialMedia;
              scope.url = directive.getSocialMediaUrl(scope.socialMedia);
          }
      };
      return directive;
  });
