(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('ParticipantStudentVisitorService', participantStudentVisitorService);

    participantStudentVisitorService.$inject = ['$q', 'DragonBreath'];

    function participantStudentVisitorService($q, DragonBreath) {
        var service = {
            getParticipantStudentVisitors: getParticipantStudentVisitors,
            getParticipantStudentVisitorsByProject: getParticipantStudentVisitorsByProject,
            getParticipantStudentVisitorById: getParticipantStudentVisitorById
        };

        return service;

        function getParticipantStudentVisitors(params) {
            var defer = $q.defer();
            DragonBreath.get(params, 'ParticipantStudentVisitors')
              .success(function (data) {
                  defer.resolve(data);
              });
            return defer.promise;
        };

        function getParticipantStudentVisitorsByProject(id, params) {
            var defer = $q.defer();
            var path = 'projects/' + id + "/ParticipantStudentVisitors";
            DragonBreath.get(params, path)
              .success(function (data) {
                  defer.resolve(data);
              });
            return defer.promise;
        };

        function getParticipantStudentVisitorById(id) {
            return DragonBreath.get('ParticipantStudentVisitors', id);
        };
    }
})();