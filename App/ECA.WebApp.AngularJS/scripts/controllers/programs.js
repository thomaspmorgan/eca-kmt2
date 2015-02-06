'use strict';

/**
 * @ngdoc function
 * @name staticApp.controller:ProgramsCtrl
 * @description
 * # ProgramsCtrl
 * Controller of the staticApp
 */
angular.module('staticApp')
  .controller('ProgramsCtrl', function ($scope, $stateParams, $state, $q, $filter, ProgramService, ProjectService, ngTableParams) {

    $scope.newProject = {};

    $scope.tabs = {
        overview: {
            title: 'Overview',
            path: 'overview',
            active: true,
            order: 1
        },
        projects: {
            title: 'Branches & Projects',
            path: 'projects',
            active: true,
            order: 2
        },
        activity: {
            title: 'Activity',
            path: 'activity',
            active: true,
            order: 3
        },
        artifacts: {
            title: 'Artifacts',
            path: 'artifacts',
            active: true,
            order: 4
        },
        impact: {
            title: 'Impact',
            path: 'impact',
            active: true,
            order: 5
        }
    };

    $scope.branches = [];
    $scope.subprograms = [];

    ProgramService.get($stateParams.programId)
        .then(function (program) {
            $scope.program = program;
            if(angular.isArray(program.childPrograms)){
                var addBranches = function(element){
                    if(element.programType === 'branch'){
                        $scope.branches.push(element);
                    } else {
                        $scope.subprograms.push(element);
                    }
                };
                program.childPrograms.forEach(addBranches);
            }

        });



    $scope.saveProject = function () {
        var project = {
            id: Date.now().toString(),
            name: $scope.newProject.title,
            description: $scope.newProject.description,
            parentProgram: {
                displayName: $scope.program.name,
                programId: $scope.program.id
            },
            owner: {
                displayName: $scope.program.owner.longName,
                organizationId: $scope.program.owner.organizationId
            }
        };
        console.log(project);

        if ($scope.newProject.branch[0]) {
            project.branch = $scope.newProject.branch[0].name;
        }

        ProjectService.create(project)
            .then(function (project) {
                $state.go('projects.overview', {officeId: $scope.program.owner.organizationId, projectId: project.id, programId: $scope.program.id});
            });

        if (!$scope.program.projectReferences) {
            $scope.program.projectReferences = [];
        }
        $scope.program.projectReferences.push({projectName: project.name, projectId: project.id});
        saveProgram();
    };

    $scope.modalClear = function () {
    	angular.forEach($scope.newProject, function (value, key) {
    		$scope.newProject[key] = '';
    	});
    };




    $scope.updateProgram = function () {
        saveProgram();
    };

    $scope.addTab = function () {
        if ($scope.tabs.moneyflows.active && !$scope.program.moneyFlowReferences) {
            $scope.program.moneyFlowReferences = [];
        }
        if ($scope.tabs.artifacts.active && !$scope.program.artifactReferences) {
          $scope.program.artifactReferences = [];
        }
        saveProgram();
    };

    function saveProgram() {
        ProgramService.update($scope.program, $stateParams.programId)
            .then(function (program) {
                $scope.program = program;
            });
    };

    $scope.tableParams = new ngTableParams({
        page: 1,            
        count: 25,         
        filter: {
            name: ''
        },
        sorting: {
            name: 'asc'
        }
    }, {
        getData: function($defer, params) {
            getProjects().then(function (data) {
                var filteredData = params.filter() ?
                   $filter('filter')(data, params.filter()) :
                   data;
                var orderedData = params.sorting() ?
                    $filter('orderBy')(filteredData, params.orderBy()) :
                    data;
                params.total(orderedData.length);
                $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            });
        }
    });

    var projects = null;
    function getProjects() {
        var defer= $q.defer();
        if (projects === null) {
            ProjectService.getProjectsByProgram($stateParams.programId)
                .then(function (data) {
                    projects = data;
                    defer.resolve(projects);
                })
        } else {
            defer.resolve(projects);
        }
        return defer.promise;
    }

    $scope.selectProject = function (program, project) {
        $state.go('projects.overview', { officeId: program.owner.organizationId, programId: program.programId, projectId: project.projectId });
    };

  });
