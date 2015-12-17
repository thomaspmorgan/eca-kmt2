'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProjectOverviewCtrl
 * @description The overview controller is used on the overview tab of a project.
 * # ProjectOverviewCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProjectOverviewCtrl', function (
        $scope,
        $stateParams,
        $q,
        $log,
        $compile,
        orderByFilter,
        ProjectService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.view.isLoadingOfficeSettings = false;
      $scope.view.isMapIdle = false;
      $scope.categoryLabel = "...";
      $scope.objectiveLabel = "...";
      $scope.sortedCategories = [];
      $scope.sortedObjectives = [];

      $scope.view.isLoadingOfficeSetting = true;
      $scope.$parent.data.loadOfficeSettingsPromise.promise
        .then(function (settings) {
            console.assert(settings.objectiveLabel, "The objective label must exist.");
            console.assert(settings.categoryLabel, "The category label must exist.");
            console.assert(settings.focusLabel, "The focus label must exist.");
            console.assert(settings.justificationLabel, "The justification label must exist.");
            console.assert(typeof (settings.isCategoryRequired) !== 'undefined', "The is category required bool must exist.");
            console.assert(typeof (settings.isObjectiveRequired) !== 'undefined', "The is objective required bool must exist.");

            var objectiveLabel = settings.objectiveLabel;
            var categoryLabel = settings.categoryLabel;
            var focusLabel = settings.focusLabel;
            var justificationLabel = settings.justificationLabel;

            $scope.categoryLabel = categoryLabel + ' / ' + focusLabel;
            $scope.objectiveLabel = objectiveLabel + ' / ' + justificationLabel;
            $scope.view.isLoadingOfficeSetting = false;
        });


      $scope.view.isLoading = true;
      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          if ($scope.$parent.project.locations) {
              $scope.$parent.project.locations = orderByFilter($scope.$parent.project.locations, '+name');
          }
          $scope.sortedCategories = orderByFilter($scope.$parent.project.categories, '+focusName');
          $scope.sortedObjectives = orderByFilter($scope.$parent.project.objectives, '+justificationName');
          $scope.view.isLoading = false;

      });

      var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
      var labelIndex = 0;
      var markers = [];
      function setMarkers(locations) {
          var map = getLocationMap();
          var bounds = new google.maps.LatLngBounds();
          angular.forEach(locations, function (location, index) {
              if (location.latitude && location.longitude) {
                  var latLng = new google.maps.LatLng(location.latitude, location.longitude);
                  var marker = new google.maps.Marker({
                      position: latLng,
                      map: map,
                      animation: google.maps.Animation.DROP,
                      title: location.name,
                      label: labels[labelIndex++ % labels.length],
                  });
                  location.mapMarkerLabel = marker.label;
                  location.marker = marker;
                  marker.addListener('click', function () {
                      onMapMarkerClick(marker, location);
                  });
                  bounds.extend(latLng);
                  markers.push(marker);
              }
          });
          map.fitBounds(bounds);
      }

      function clearMapMarkers() {
          angular.forEach(markers, function (marker, index) {
              marker.setMap(null);
          });
          markers = [];
      }


      $scope.view.mapOptions = {
          center: new google.maps.LatLng(38.9071, -77.0368),
          zoom: 0,
          mapTypeId: google.maps.MapTypeId.ROADMAP
      };

      $scope.view.onMapIdle = function () {
          $scope.view.isMapIdle = true;
          if (markers.length === 0) {
              setMarkers($scope.$parent.project.locations);
          }
      }

      $scope.view.onMapMarkerLabelClick = function (location) {
          var marker = location.marker;
          var map = getLocationMap();
          map.setCenter(marker.getPosition());
      }

      var infoWindow = null;
      function onMapMarkerClick(marker, location) {
          var map = getLocationMap();
          $scope.mapLocation = location;
          
          var content = '<div><ng-include src="\'app/projects/project-location-marker.html\'"></ng-include></div>';
          var compiled = $compile(content)($scope);
          if (infoWindow) {
              infoWindow.close();
          }
          infoWindow = new google.maps.InfoWindow();
          infoWindow.setContent(compiled[0]);
          infoWindow.open(map, marker);
      }

      function getLocationMap() {
          return $scope.view.locationMap;
      }
  });
