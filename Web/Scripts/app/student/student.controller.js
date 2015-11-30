((function () {
    'use strict';

    angular
        .module('app.student')
        .controller('Student', Student);

    Student.$inject = ['$scope', 'bootstrapData', 'studentService', 'guidService', 'dateService'];

    function Student($scope, bootstrapData, studentService, guidService, dateService) {
        var vm = this;

        vm.students = bootstrapData.model.Students;

        vm.students.forEach(function (student) {
            student.DateOfBirth = dateService.parseJsonDate(student.DateOfBirth);
        });

        vm.studentInEdit = {
            Id: '',
            FirstName: '',
            MiddleName: '',
            LastName: '',
            Suffix: '',
            DateOfBirth: ''
        };

        vm.editingStudent = false;

        var editIndex = -1;

        vm.edit = function (index) {
            vm.editingStudent = true;

            if (index || index > -1) {
                editIndex = index;
                copyStudentValues(vm.students[editIndex], vm.studentInEdit);
            }
        };

        vm.save = function () {
            if (editIndex == -1)
                add();
            else
                update();
        };

        vm.cancel = function () {
            clearStudentInEdit();
        };

        function clearStudentInEdit() {
            editIndex = -1;

            vm.editingStudent = false;

            vm.studentInEdit.Id = '';
            vm.studentInEdit.FirstName = '';
            vm.studentInEdit.MiddleName = '';
            vm.studentInEdit.LastName = '';
            vm.studentInEdit.Suffix = '';
            vm.studentInEdit.DateOfBirth = '';
        }

        function update() {
            var studentToUpdate = {};
            copyStudentValues(vm.studentInEdit, studentToUpdate);

            studentService.update(studentToUpdate).then(function () {
                copyStudentValues(studentToUpdate, vm.students[editIndex]);
                clearStudentInEdit();
            });
        }

        function copyStudentValues(source, target) {
            target.Id = source.Id;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.DateOfBirth = source.DateOfBirth;
        }

        function add() {
            vm.studentInEdit.Id = guidService.generate();

            var studentToAdd = {};
            copyStudentValues(vm.studentInEdit, studentToAdd);

            studentService.add(studentToAdd).then(function () {
                vm.students.push(studentToAdd);

                clearStudentInEdit();
            });
        }

        vm.remove = function () {
            studentService.remove(vm.studentInEdit.Id).then(function () {
                vm.students.splice(editIndex, 1);
                clearStudentInEdit();
            });
        };
    }
}))();