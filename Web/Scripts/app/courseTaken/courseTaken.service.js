((function () {
    'use strict';

    angular
        .module('app.courseTaken')
        .factory('courseTakenService', courseTakenService);

    courseTakenService.$inject = ['promiseService'];

    function courseTakenService(promiseService) {
        return {
            add: add,
            update: update,
            remove: remove
        };

        function add(courseTaken) {
            var body = { courseTaken: courseTaken };
            return promiseService.postPromise('/CoursesTaken/Add', body);
        }

        function update(courseTaken) {
            var body = { courseTaken: courseTaken };
            return promiseService.postPromise('/CoursesTaken/Update', body);
        }

        function remove(courseTaken) {
            var body = { courseTaken: courseTaken };
            return promiseService.postPromise('/CoursesTaken/Remove', body);
        }
    }
}))();