((function () {
    'use strict';

    angular
        .module('app.course')
        .controller('Course', Course);

    Course.$inject = ['$scope', 'bootstrapData', 'courseService', 'guidService'];

    function Course($scope, bootstrapData, courseService, guidService) {
        var vm = this;

        vm.courses = bootstrapData.model.Courses;

        vm.semesters = ['FALL', 'SPRING', 'SUMMER'];
        vm.sections = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z']

        vm.courseInEdit = {
            Id: '',
            Name: '',
            Department: '',
            Number: 0,
            Section: vm.sections[0],
            Professor: '',
            Year: 0,
            Semester: vm.semesters[0]
        };

        vm.editingCourse = false;
        vm.duplicateCourseInEdit = false;

        var editIndex = -1;

        vm.edit = function (index) {
            vm.editingCourse = true;

            if (index || index > -1) {
                editIndex = index;
                copyCourseValues(vm.courses[editIndex], vm.courseInEdit);
            }
        };

        vm.save = function () {
            if (editIndex == -1)
                add();
            else
                update();
        };

        vm.cancel = function () {
            clearCourseInEdit();
        };

        function clearCourseInEdit() {
            editIndex = -1;

            vm.editingCourse = false;

            vm.courseInEdit.Id = '';
            vm.courseInEdit.Name = '';
            vm.courseInEdit.Department = '';
            vm.courseInEdit.Number = 0;
            vm.courseInEdit.Section = vm.sections[0];
            vm.courseInEdit.Professor = '';
            vm.courseInEdit.Year = 0;
            vm.courseInEdit.Semester = vm.semesters[0];
        }

        function update() {
            var courseToUpdate = {};
            copyCourseValues(vm.courseInEdit, courseToUpdate);

            courseService.update(courseToUpdate).then(function () {
                copyCourseValues(courseToUpdate, vm.courses[editIndex]);
                clearCourseInEdit();
            });
        }

        function copyCourseValues(source, target) {
            target.Id = source.Id;
            target.Name = source.Name;
            target.Department = source.Department;
            target.Number = source.Number;
            target.Section = source.Section;
            target.Professor = source.Professor;
            target.Year = source.Year;
            target.Semester = source.Semester;
        }

        function add() {
            vm.courseInEdit.Id = guidService.generate();

            var courseToAdd = {};
            copyCourseValues(vm.courseInEdit, courseToAdd);

            courseService.add(courseToAdd).then(function () {
                vm.courses.push(courseToAdd);
                clearCourseInEdit();
            });
        }

        vm.remove = function () {
            courseService.remove(vm.courseInEdit.Id).then(function () {
                vm.courses.splice(editIndex, 1);
                clearCourseInEdit();
            });
        };

        $scope.$watch('vm.courseInEdit', verifyNoDuplicates, true);

        function verifyNoDuplicates() {
            vm.duplicateCourseInEdit = false;

            for (var i = 0; i < vm.courses.length; i++) {
                vm.duplicateCourseInEdit |= vm.courseInEdit.Id != vm.courses[i].Id
                    && vm.courseInEdit.Department == vm.courses[i].Department
                    && vm.courseInEdit.Number == vm.courses[i].Number
                    && vm.courseInEdit.Section == vm.courses[i].Section
                    && vm.courseInEdit.Year == vm.courses[i].Year
                    && vm.courseInEdit.Semester == vm.courses[i].Semester;
            }
        }
    }
}))();