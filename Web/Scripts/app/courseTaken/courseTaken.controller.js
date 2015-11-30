((function () {
    'use strict';

    angular
        .module('app.courseTaken')
        .controller('CourseTaken', CourseTaken);

    CourseTaken.$inject = ['$scope', 'bootstrapData', 'courseTakenService'];

    function CourseTaken($scope, bootstrapData, courseTakenService) {
        var vm = this;

        vm.coursesTaken = bootstrapData.model.CoursesTaken;
        vm.toAssign = bootstrapData.model.ToAssign;

        vm.courseTakenInEdit = {
            Student: { Id: '', Name: '' },
            Course: { Id: '', Name: '' },
            Grade: 0
        };
        
        if (bootstrapData.model.Student) {
            vm.student = bootstrapData.model.Student;
            vm.courseTakenInEdit.Student = vm.student;
        }

        if (bootstrapData.model.Course) {
            vm.course = bootstrapData.model.Course;
            vm.courseTakenInEdit.Course = vm.course;
        }

        vm.editingCourseTaken = false;

        vm.editIndex = -1;

        vm.edit = function (index) {
            vm.editingCourseTaken = true;

            if (index || index > -1) {
                vm.editIndex = index;
                copyCourseTakenValues(vm.coursesTaken[vm.editIndex], vm.courseTakenInEdit);
            }
        };

        vm.save = function () {
            if (vm.editIndex == -1)
                add();
            else
                update();
        };

        vm.cancel = function () {
            clearCourseTakenInEdit();
        };

        function clearCourseTakenInEdit() {
            vm.editIndex = -1;

            vm.editingCourseTaken = false;

            if (vm.student) {
                vm.courseTakenInEdit.Student = vm.student;
            } else {
                vm.courseTakenInEdit.Student = vm.toAssign[0];
            }

            if (vm.course) {
                vm.courseTakenInEdit.Course = vm.course;
            } else {
                vm.courseTakenInEdit.Course = vm.toAssign[0];
            }

            vm.courseTakenInEdit.Grade = 0;
        }

        function update() {
            var courseTakenToUpdate = {};
            copyCourseTakenValues(vm.courseTakenInEdit, courseTakenToUpdate);

            courseTakenService.update(courseTakenToUpdate).then(function () {
                copyCourseTakenValues(courseTakenToUpdate, vm.coursesTaken[vm.editIndex]);
                clearCourseTakenInEdit();
            });
        }

        function copyCourseTakenValues(source, target) {
            target.Student = { Id: source.Student.Id, Name: source.Student.Name };
            target.Course = { Id: source.Course.Id, Name: source.Course.Name };
            target.Grade = source.Grade;
        }

        function add() {
            var courseTakenToAdd = {};
            copyCourseTakenValues(vm.courseTakenInEdit, courseTakenToAdd);

            courseTakenService.add(courseTakenToAdd).then(function () {
                vm.coursesTaken.push(courseTakenToAdd);
                clearCourseTakenInEdit();
            });
        }

        vm.remove = function () {
            var courseTakenToRemove = {};
            copyCourseTakenValues(vm.courseTakenInEdit, courseTakenToRemove);

            courseTakenService.remove(courseTakenToRemove).then(function () {
                vm.coursesTaken.splice(vm.editIndex, 1);
                clearCourseTakenInEdit();
            });
        };
    }
}))();