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
        $filter,
        orderByFilter,
        BrowserService,
        ProjectService,
        ConstantsService,
        NotificationService) {

      $scope.view = {};
      $scope.view.params = $stateParams;
      $scope.view.isLoading = false;
      $scope.view.isLoadingOfficeSettings = false;
      $scope.view.isMapIdle = false;
      $scope.view.canShowLocationMarkers = false;
      $scope.view.dataPointConfigurations = {};

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

      $scope.data.loadDataPointConfigurationsPromise.promise
      .then(function (dataConfigurations) {
          var array = $filter('filter')(dataConfigurations, { categoryId: ConstantsService.dataPointCategory.project.id });
          for (var i = 0; i < array.length; i++) {
              $scope.view.dataPointConfigurations[array[i].propertyId] = array[i].isRequired;
          }
      });


      $scope.view.isLoading = true;
      $scope.$parent.data.loadProjectByIdPromise.promise.then(function (project) {
          BrowserService.setDocumentTitleByProject(project, 'Overview');
          if ($scope.$parent.project.locations) {
              $scope.$parent.project.locations = orderByFilter($scope.$parent.project.locations, '+name');
              for(var i=0; i<$scope.$parent.project.locations.length; i++){
                  var location = $scope.$parent.project.locations[i];
                  if (location.latitude && location.longitude) {
                      $scope.view.canShowLocationMarkers = true;
                      break;
                  }
              }
          }
          $scope.sortedCategories = orderByFilter($scope.$parent.project.categories, '+focusName');
          $scope.sortedObjectives = orderByFilter($scope.$parent.project.objectives, '+justificationName');
          $scope.view.isLoading = false;

      });
      $scope.$parent.data.loadDefaultExchangeVisitorFundingPromise.promise.then(function (defaultVisitorExchangeFunding) {
          $scope.view.sevisFunding = defaultVisitorExchangeFunding;
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
                      locationId: location.id
                  });
                  location.mapMarkerLabel = marker.label;
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

      var fixedRedrawIssue = false;
      $scope.view.onMapIdle = function () {
          //this fixes a google map issue when the edit state is saved and then the overview state is shown
          //otherwise the map would just show a grey box
          if (!fixedRedrawIssue) {
              var map = getLocationMap();
              google.maps.event.trigger(map, 'resize');
              map.setCenter($scope.view.mapOptions.center);
              fixedRedrawIssue = true;
          }
          if (markers.length === 0) {
              setMarkers($scope.$parent.project.locations);
          }
          $scope.view.isMapIdle = true;
      }

      $scope.view.onMapMarkerLabelClick = function (location) {
          var markerToCenter = null;
          angular.forEach(markers, function (marker, index) {
              if (marker.locationId === location.id) {
                  markerToCenter = marker;
              }
          });
          if (markerToCenter !== null) {
              var map = getLocationMap();
              map.setCenter(markerToCenter.getPosition());
          }
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
