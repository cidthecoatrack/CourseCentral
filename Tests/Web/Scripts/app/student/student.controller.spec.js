'use strict'

describe('Student Controller', function () {
    var vm;
    var studentServiceMock;
    var bootstrapDataMock;
    var guidServiceMock;
    var q;
    var scope;
    var dateServiceMock;

    beforeEach(module('app.student'));

    beforeEach(function () {
        studentServiceMock = {
            add: function (student) {
                return postMockedPromise();
            },
            update: function (student) {
                return postMockedPromise();
            },
            remove: function (studentId) {
                return postMockedPromise();
            }
        };

        bootstrapDataMock = {
            model: {
                Students: [
                    {
                        Id: 'first guid',
                        FirstName: 'first first name',
                        MiddleName: 'first middle name',
                        LastName: 'first last name',
                        Suffix: 'first suffix',
                        DateOfBirth: '\/Date(' + new Date(1989, 9, 29).valueOf() + ')\/'
                    }, {
                        Id: 'second guid',
                        FirstName: 'second first name',
                        MiddleName: 'second middle name',
                        LastName: 'second last name',
                        Suffix: 'second suffix',
                        DateOfBirth: '\/Date(' + new Date(1991, 6, 17).valueOf() + ')\/'
                    }
                ]
            }
        };

        guidServiceMock = {
            generate: function () {
                return 'new guid';
            }
        };

        dateServiceMock = {
            parseJsonDate: function (jsonDate) {
                var dateInt = parseInt(jsonDate.substr(6));
                if (dateInt > 0)
                    return new Date(dateInt);

                return '';
            }
        };
    });

    function postMockedPromise() {
        var deferred = q.defer();
        deferred.resolve();
        return deferred.promise;
    }

    beforeEach(inject(function ($rootScope, $controller, $q) {
        q = $q;
        scope = $rootScope.$new();

        vm = $controller('Student as vm', {
            $scope: scope,
            bootstrapData: bootstrapDataMock,
            studentService: studentServiceMock,
            guidService: guidServiceMock,
            dateService: dateServiceMock
        });
    }));

    it('has all students from bootstrap data', function () {
        expect(vm.students[0].Id).toBe('first guid');
        expect(vm.students[0].FirstName).toBe('first first name');
        expect(vm.students[0].MiddleName).toBe('first middle name');
        expect(vm.students[0].LastName).toBe('first last name');
        expect(vm.students[0].Suffix).toBe('first suffix');
        expect(vm.students[1].Id).toBe('second guid');
        expect(vm.students[1].FirstName).toBe('second first name');
        expect(vm.students[1].MiddleName).toBe('second middle name');
        expect(vm.students[1].LastName).toBe('second last name');
        expect(vm.students[1].Suffix).toBe('second suffix');
        expect(vm.students.length).toBe(2);
    });

    it('parses the JSON dates from bootstrap data', function () {
        expect(vm.students[0].DateOfBirth.toDateString()).toBe('Sun Oct 29 1989');
        expect(vm.students[1].DateOfBirth.toDateString()).toBe('Wed Jul 17 1991');
    });

    it('has an empty student for editing and adding', function () {
        expect(vm.studentInEdit.Id).toBe('');
        expect(vm.studentInEdit.FirstName).toBe('');
        expect(vm.studentInEdit.MiddleName).toBe('');
        expect(vm.studentInEdit.LastName).toBe('');
        expect(vm.studentInEdit.Suffix).toBe('');
        expect(vm.studentInEdit.DateOfBirth).toBe('');
    });

    it('is not editing a student on load', function () {
        expect(vm.editingStudent).toBeFalsy();
    });

    it('prepares a new student to be added', function () {
        vm.edit();

        expect(vm.editingStudent).toBeTruthy();
        expect(vm.studentInEdit.Id).toBe('');
        expect(vm.studentInEdit.FirstName).toBe('');
        expect(vm.studentInEdit.MiddleName).toBe('');
        expect(vm.studentInEdit.LastName).toBe('');
        expect(vm.studentInEdit.Suffix).toBe('');
        expect(vm.studentInEdit.DateOfBirth).toBe('');
        expect(vm.students.length).toBe(2);
    });

    it('adds a new student', function () {
        vm.edit();

        vm.studentInEdit.FirstName = 'new first name';
        vm.studentInEdit.MiddleName = 'new middle name';
        vm.studentInEdit.LastName = 'new last name';
        vm.studentInEdit.Suffix = 'new suffix';

        var date = new Date();
        vm.studentInEdit.DateOfBirth = date;

        spyOn(studentServiceMock, 'add').and.callThrough();

        vm.save();
        scope.$apply();

        expect(vm.editingStudent).toBeFalsy();

        expect(studentServiceMock.add).toHaveBeenCalledWith({
            Id: 'new guid',
            FirstName: 'new first name',
            MiddleName: 'new middle name',
            LastName: 'new last name',
            Suffix: 'new suffix',
            DateOfBirth: date
        });

        expect(vm.students[2].Id).toBe('new guid');
        expect(vm.students[2].FirstName).toBe('new first name');
        expect(vm.students[2].MiddleName).toBe('new middle name');
        expect(vm.students[2].LastName).toBe('new last name');
        expect(vm.students[2].Suffix).toBe('new suffix');
        expect(vm.students[2].DateOfBirth).toBe(date);

        expect(vm.studentInEdit.Id).toBe('');
        expect(vm.studentInEdit.FirstName).toBe('');
        expect(vm.studentInEdit.MiddleName).toBe('');
        expect(vm.studentInEdit.LastName).toBe('');
        expect(vm.studentInEdit.Suffix).toBe('');
        expect(vm.studentInEdit.DateOfBirth).toBe('');
    });

    it('cancels adding a new student', function () {
        vm.edit();

        vm.studentInEdit.FirstName = 'new first name';
        vm.studentInEdit.MiddleName = 'new middle name';
        vm.studentInEdit.LastName = 'new last name';
        vm.studentInEdit.Suffix = 'new suffix';
        vm.studentInEdit.DateOfBirth = new Date();

        vm.cancel();
        scope.$digest();

        expect(vm.editingStudent).toBeFalsy();
        expect(vm.studentInEdit.Id).toBe('');
        expect(vm.studentInEdit.FirstName).toBe('');
        expect(vm.studentInEdit.MiddleName).toBe('');
        expect(vm.studentInEdit.LastName).toBe('');
        expect(vm.studentInEdit.Suffix).toBe('');
        expect(vm.studentInEdit.DateOfBirth).toBe('');
        expect(vm.students.length).toBe(2);
    });

    it('places a student in edit', function () {
        vm.edit(1);

        expect(vm.editingStudent).toBeTruthy();
        expect(vm.studentInEdit.Id).toBe('second guid');
        expect(vm.studentInEdit.FirstName).toBe('second first name');
        expect(vm.studentInEdit.MiddleName).toBe('second middle name');
        expect(vm.studentInEdit.LastName).toBe('second last name');
        expect(vm.studentInEdit.Suffix).toBe('second suffix');
        expect(vm.studentInEdit.DateOfBirth).toBe(vm.students[1].DateOfBirth);
    });

    it('can make changes to a student without altering the original', function () {
        vm.edit(1);

        vm.studentInEdit.FirstName = 'new first name';
        vm.studentInEdit.MiddleName = 'new middle name';
        vm.studentInEdit.LastName = 'new last name';
        vm.studentInEdit.Suffix = 'new suffix';

        var date = new Date();
        vm.studentInEdit.DateOfBirth = date;

        expect(vm.studentInEdit.Id).toBe('second guid');
        expect(vm.studentInEdit.FirstName).toBe('new first name');
        expect(vm.studentInEdit.MiddleName).toBe('new middle name');
        expect(vm.studentInEdit.LastName).toBe('new last name');
        expect(vm.studentInEdit.Suffix).toBe('new suffix');
        expect(vm.studentInEdit.DateOfBirth).toBe(date);

        expect(vm.students[1].Id).toBe('second guid');
        expect(vm.students[1].FirstName).toBe('second first name');
        expect(vm.students[1].MiddleName).toBe('second middle name');
        expect(vm.students[1].LastName).toBe('second last name');
        expect(vm.students[1].Suffix).toBe('second suffix');
        expect(vm.students[1].DateOfBirth.toDateString()).toBe('Wed Jul 17 1991');
    });

    it('can cancel changes to a student', function () {
        vm.edit(1);

        vm.studentInEdit.FirstName = 'new first name';
        vm.studentInEdit.MiddleName = 'new middle name';
        vm.studentInEdit.LastName = 'new last name';
        vm.studentInEdit.Suffix = 'new suffix';
        vm.studentInEdit.DateOfBirth = new Date();

        vm.cancel();
        scope.$digest();

        expect(vm.editingStudent).toBeFalsy();
        expect(vm.studentInEdit.Id).toBe('');
        expect(vm.studentInEdit.FirstName).toBe('');
        expect(vm.studentInEdit.MiddleName).toBe('');
        expect(vm.studentInEdit.LastName).toBe('');
        expect(vm.studentInEdit.Suffix).toBe('');
        expect(vm.studentInEdit.DateOfBirth).toBe('');

        expect(vm.students[1].Id).toBe('second guid');
        expect(vm.students[1].FirstName).toBe('second first name');
        expect(vm.students[1].MiddleName).toBe('second middle name');
        expect(vm.students[1].LastName).toBe('second last name');
        expect(vm.students[1].Suffix).toBe('second suffix');
        expect(vm.students[1].DateOfBirth.toDateString()).toBe('Wed Jul 17 1991');
    });

    it('updates a student', function () {
        vm.edit(1);

        vm.studentInEdit.FirstName = 'new first name';
        vm.studentInEdit.MiddleName = 'new middle name';
        vm.studentInEdit.LastName = 'new last name';
        vm.studentInEdit.Suffix = 'new suffix';

        var date = new Date();
        vm.studentInEdit.DateOfBirth = date;

        spyOn(studentServiceMock, 'update').and.callThrough();

        vm.save();
        scope.$apply();

        expect(vm.editingStudent).toBeFalsy();

        expect(studentServiceMock.update).toHaveBeenCalledWith({
            Id: 'second guid',
            FirstName: 'new first name',
            MiddleName: 'new middle name',
            LastName: 'new last name',
            Suffix: 'new suffix',
            DateOfBirth: date
        });

        expect(vm.studentInEdit.Id).toBe('');
        expect(vm.studentInEdit.FirstName).toBe('');
        expect(vm.studentInEdit.MiddleName).toBe('');
        expect(vm.studentInEdit.LastName).toBe('');
        expect(vm.studentInEdit.Suffix).toBe('');
        expect(vm.studentInEdit.DateOfBirth).toBe('');

        expect(vm.students[1].Id).toBe('second guid');
        expect(vm.students[1].FirstName).toBe('new first name');
        expect(vm.students[1].MiddleName).toBe('new middle name');
        expect(vm.students[1].LastName).toBe('new last name');
        expect(vm.students[1].Suffix).toBe('new suffix');
        expect(vm.students[1].DateOfBirth).toBe(date);
    });

    it('removes a student', function () {
        vm.edit(0);

        spyOn(studentServiceMock, 'remove').and.callThrough();

        expect(vm.studentInEdit.Id).toBe('first guid');

        vm.remove();
        scope.$apply();

        expect(studentServiceMock.remove).toHaveBeenCalledWith('first guid');

        expect(vm.studentInEdit.Id).toBe('');
        expect(vm.studentInEdit.FirstName).toBe('');
        expect(vm.studentInEdit.MiddleName).toBe('');
        expect(vm.studentInEdit.LastName).toBe('');
        expect(vm.studentInEdit.Suffix).toBe('');
        expect(vm.studentInEdit.DateOfBirth).toBe('');

        expect(vm.students.length).toBe(1);
        expect(vm.students[0].Id).toBe('second guid');
        expect(vm.students[0].FirstName).toBe('second first name');
        expect(vm.students[0].MiddleName).toBe('second middle name');
        expect(vm.students[0].LastName).toBe('second last name');
        expect(vm.students[0].Suffix).toBe('second suffix');
        expect(vm.students[0].DateOfBirth.toDateString()).toBe('Wed Jul 17 1991');
    });
});