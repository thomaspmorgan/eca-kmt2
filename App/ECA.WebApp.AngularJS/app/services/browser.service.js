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
            setDocumentTitleByReport: setDocumentTitleByReport
        };

        return service;

        function setAllProgramsDocumentTitle() {
            service.setDocumentTitle(function () {
                return 'All Programs';
            })
        }

        function setDocumentTitleByReport(tab) {
            service.setDocumentTitle(function () {
                var title = 'Reports';
                if (tab) {
                    title += ' - ' + tab;
                }
                return title;
            })
        }

        function setHomeDocumentTitle(tab) {
            service.setDocumentTitle(function () {
                if (tab) {
                    return tab;
                }
                else {
                    return '';
                }
            })
        }

        function setMoneyFlowTitleByEntityName(entityName) {
            service.setDocumentTitle(function () {
                var title = entityName + ' - Funding';
                return title;
            });
        }

        function setDocumentTitleByPerson(person, tab) {
            service.setDocumentTitle(function () {
                var title = person.fullName;
                if (tab) {
                    title += ' - ' + tab;
                }
                return title;
            });
        }

        function setAllPeopleDocumentTitle() {
            service.setDocumentTitle(function () {
                return 'People';
            })
        }

        function setAllOfficesDocumentTitle() {
            service.setDocumentTitle(function () {
                return 'Office Directory';
            });
        }

        function setAllOrganizationsDocumentTitle() {
            service.setDocumentTitle(function () {
                return 'Organizations';
            });
        }

        function setDocumentTitleByOffice(office, tab) {
            service.setDocumentTitle(function () {
                var title = office.name;
                if (tab) {
                    title += ' - ' + tab;
                }
                return title;
            })
        }

        function setDocumentTitleByOrganization(org, tab) {
            service.setDocumentTitle(function () {
                var title = org.name;
                if (tab) {
                    title += ' - ' + tab;
                }
                return title;
            })
        }

        function setDocumentTitleByProject(project, tab) {
            service.setDocumentTitle(function () {
                var title = project.name;
                if (tab) {
                    title += ' - ' + tab;
                }
                return title;
            });
        };

        function setDocumentTitleByProgram(program, tab) {
            service.setDocumentTitle(function () {
                var title = program.name;
                if (tab) {
                    title += ' - ' + tab;
                }
                return title;
            });
        };


        function setDocumentTitle(documentTitleFn) {
            if (angular.isUndefined(documentTitleFn)) {
                throw Error('The documentTitleFn is undefined.');
            }
            if (!angular.isFunction(documentTitleFn)) {
                throw Error('The documentTitleFn must be a function');
            }
            var title = documentTitleFn();
            if (!angular.isString(title)) {
                throw Error('The documentTitleFn must return a string.');
            }
            title = 'ECA KMT - ' + title;

            document.title = title;
        };

    }
})();