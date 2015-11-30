((function () {
    'use strict';

    angular
        .module('app.student')
        .factory('studentService', studentService);

    studentService.$inject = ['promiseService'];

    function studentService(promiseService) {
        return {
            add: add,
            update: update,
            remove: remove
        };

        function add(student) {
            var body = { student: student };
            return promiseService.postPromise('/Students/Add', body);
        }

        function update(student) {
            var body = { student: student };
            return promiseService.postPromise('/Students/Update', body);
        }

        function remove(studentId) {
            var body = { studentId: studentId };
            return promiseService.postPromise('/Students/Remove', body);
        }
    }
}))();