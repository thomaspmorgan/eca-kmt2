(function () {
    'use strict';

    angular
        .module('staticApp')
        .factory('BrowserService', browserService);

    browserService.$inject = ['$rootScope', '$state', '$q', 'DragonBreath'];

    function browserService($rootScope, $state, $q, DragonBreath) {
        var service = {
            setDocumentTitleByProject: setDocumentTitleByProject,
            setDocumentTitleByProgram: setDocumentTitleByProgram,
            setDocumentTitleByOffice: setDocumentTitleByOffice,
            setAllProgramsDocumentTitle: setAllProgramsDocumentTitle,
            setAllOfficesDocumentTitle: setAllOfficesDocumentTitle,
            setAllOrganizationsDocumentTitle: setAllOrganizationsDocumentTitle,
            setDocumentTitleByOrganization: setDocumentTitleByOrganization,
            setDocumentTitleByPerson: setDocumentTitleByPerson,
            setDocumentTitle: setDocumentTitle,
            setAllPeopleDocumentTitle: setAllPeopleDocumentTitle,
            setMoneyFlowTitleByEntityName: setMoneyFlowTitleByEntityName,
            setHomeDocumentTitle: setHomeDocumentTitle,
            setDocumentTitleByReport: setDocumentTitleByReport,
            getModel: getModel,
            _model: { title: null, tab: null }
        };

        return service;

        function setAllProgramsDocumentTitle() {
            service._model.title = 'All Programs';
            service._model.tab = null;
            setDocumentTitle();
        }

        function setDocumentTitleByReport(tab) {
            service._model.title = 'Reports';
            service._model.tab = tab;
            setDocumentTitle();
        }

        function setHomeDocumentTitle(tab) {
            service._model.title = 'Home';
            service._model.tab = tab;
            setDocumentTitle();
        }

        function setMoneyFlowTitleByEntityName(entityName) {
            console.assert(entityName, 'The entity name must be defined.');
            service._model.title = entityName;
            service._model.tab = 'Funding';
            setDocumentTitle();
        }

        function setDocumentTitleByPerson(person, tab) {
            console.assert(person, 'The person must be defined.');
            console.assert(person.fullName, 'The fullName must be defined.');
            service._model.title = person.fullName;
            service._model.tab = tab;
            setDocumentTitle();
        }

        function setAllPeopleDocumentTitle() {
            service._model.title = 'People';
            service._model.tab = null;
            setDocumentTitle();
        }

        function setAllOfficesDocumentTitle() {
            service._model.title = 'Office Directory';
            service._model.tab = null;
            setDocumentTitle();
        }

        function setAllOrganizationsDocumentTitle() {
            service._model.title = 'Organizations';
            service._model.tab = null;
            setDocumentTitle();
        }

        function setDocumentTitleByOffice(office, tab) {
            console.assert(office, 'The office must be defined.');
            console.assert(office.name, 'The name must be defined.');
            service._model.title = office.name;
            service._model.tab = tab;
            setDocumentTitle();
        }

        function setDocumentTitleByOrganization(org, tab) {
            console.assert(org, 'The org must be defined.');
            console.assert(org.name, 'The name must be defined.');
            service._model.title = org.name;
            service._model.tab = tab;
            setDocumentTitle();
        }

        function setDocumentTitleByProject(project, tab) {
            console.assert(project, 'The project must be defined.');
            console.assert(project.name, 'The name must be defined.');
            service._model.title = project.name;
            service._model.tab = tab;
            setDocumentTitle();
        };

        function setDocumentTitleByProgram(program, tab) {
            console.assert(program, 'The program must be defined.');
            console.assert(program.name, 'The name must be defined.');
            service._model.title = program.name;
            service._model.tab = tab;
            setDocumentTitle();
        };

        function getModel() {
            return service._model;
        }

        function setDocumentTitle() {
            if (angular.isUndefined(service._model.title)) {
                throw Error('The model title is undefined.');
            }
            if (!angular.isString(service._model.title)) {
                throw Error('the model title must be a string.');
            }
            var title = 'ECA KMT - ' + service._model.title;
            if (!angular.isUndefined(service._model.title) && service._model.tab !== null) {
                if (!angular.isString(service._model.tab)) {
                    throw Error('The model tab must be a string.');
                }
                title += ' - ' + service._model.tab;
            }
            document.title = title;
        };

    }
})();