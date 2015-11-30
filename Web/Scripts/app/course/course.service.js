((function () {
    'use strict';

    angular
        .module('app.course')
        .factory('courseService', courseService);

    courseService.$inject = ['promiseService'];

    function courseService(promiseService) {
        return {
            add: add,
            update: update,
            remove: remove
        };

        function add(course) {
            var body = { course: course };
            return promiseService.postPromise('/Courses/Add', body);
        }

        function update(course) {
            var body = { course: course };
            return promiseService.postPromise('/Courses/Update', body);
        }

        function remove(courseId) {
            var body = { courseId: courseId };
            return promiseService.postPromise('/Courses/Remove', body);
        }
    }
}))();