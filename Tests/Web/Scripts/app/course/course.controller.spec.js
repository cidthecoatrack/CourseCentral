'use strict'

describe('Course Controller', function () {
    var vm;
    var courseServiceMock;
    var bootstrapDataMock;
    var guidServiceMock;
    var q;
    var scope;

    beforeEach(module('app.course'));

    beforeEach(function () {
        courseServiceMock = {
            add: function (course) {
                return postMockedPromise();
            },
            update: function (course) {
                return postMockedPromise();
            },
            remove: function (courseId) {
                return postMockedPromise();
            }
        };

        bootstrapDataMock = {
            model: {
                Courses: [
                    {
                        Id: 'first guid',
                        Name: 'first name',
                        Department: 'first department',
                        Number: 9266,
                        Section: 'A',
                        Professor: 'first professor',
                        Year: 90210,
                        Semester: 'FALL'
                    }, {
                        Id: 'second guid',
                        Name: 'second name',
                        Department: 'second department',
                        Number: 1337,
                        Section: 'B',
                        Professor: 'second professor',
                        Year: 42,
                        Semester: 'SPRING'
                    }
                ]
            }
        };

        guidServiceMock = {
            generate: function () {
                return 'new guid';
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

        vm = $controller('Course as vm', {
            $scope: scope,
            bootstrapData: bootstrapDataMock,
            courseService: courseServiceMock,
            guidService: guidServiceMock
        });
    }));

    it('has all courses from bootstrap data', function () {
        expect(vm.courses).toBe(bootstrapDataMock.model.Courses);
    });

    it('has an empty course for editing and adding', function () {
        expect(vm.courseInEdit.Id).toBe('');
        expect(vm.courseInEdit.Name).toBe('');
        expect(vm.courseInEdit.Department).toBe('');
        expect(vm.courseInEdit.Number).toBe(0);
        expect(vm.courseInEdit.Section).toBe('A');
        expect(vm.courseInEdit.Professor).toBe('');
        expect(vm.courseInEdit.Year).toBe(0);
        expect(vm.courseInEdit.Semester).toBe('FALL');
    });

    it('is not editing a course on load', function () {
        expect(vm.editingCourse).toBeFalsy();
    });

    it('has no duplicate course on load', function () {
        expect(vm.duplicateCourseInEdit).toBeFalsy();
    });

    it('has sections to choose from', function () {
        expect(vm.sections[0]).toBe('A');
        expect(vm.sections[1]).toBe('B');
        expect(vm.sections[2]).toBe('C');
        expect(vm.sections[3]).toBe('D');
        expect(vm.sections[4]).toBe('E');
        expect(vm.sections[5]).toBe('F');
        expect(vm.sections[6]).toBe('G');
        expect(vm.sections[7]).toBe('H');
        expect(vm.sections[8]).toBe('I');
        expect(vm.sections[9]).toBe('J');
        expect(vm.sections[10]).toBe('K');
        expect(vm.sections[11]).toBe('L');
        expect(vm.sections[12]).toBe('M');
        expect(vm.sections[13]).toBe('N');
        expect(vm.sections[14]).toBe('O');
        expect(vm.sections[15]).toBe('P');
        expect(vm.sections[16]).toBe('Q');
        expect(vm.sections[17]).toBe('R');
        expect(vm.sections[18]).toBe('S');
        expect(vm.sections[19]).toBe('T');
        expect(vm.sections[20]).toBe('U');
        expect(vm.sections[21]).toBe('V');
        expect(vm.sections[22]).toBe('W');
        expect(vm.sections[23]).toBe('X');
        expect(vm.sections[24]).toBe('Y');
        expect(vm.sections[25]).toBe('Z');
        expect(vm.sections.length).toBe(26);
    });

    it('has semesters to choose from', function () {
        expect(vm.semesters[0]).toBe('FALL');
        expect(vm.semesters[1]).toBe('SPRING');
        expect(vm.semesters[2]).toBe('SUMMER');
        expect(vm.semesters.length).toBe(3);
    });

    it('prepares a new course to be added', function () {
        vm.edit();

        expect(vm.editingCourse).toBeTruthy();
        expect(vm.courseInEdit.Id).toBe('');
        expect(vm.courseInEdit.Name).toBe('');
        expect(vm.courseInEdit.Department).toBe('');
        expect(vm.courseInEdit.Number).toBe(0);
        expect(vm.courseInEdit.Section).toBe(vm.sections[0]);
        expect(vm.courseInEdit.Professor).toBe('');
        expect(vm.courseInEdit.Year).toBe(0);
        expect(vm.courseInEdit.Semester).toBe(vm.semesters[0]);
        expect(vm.courses.length).toBe(2);
    });

    it('adds a new course', function () {
        vm.edit();

        vm.courseInEdit.Name = 'new name';
        vm.courseInEdit.Department = 'new department';
        vm.courseInEdit.Number = 2345;
        vm.courseInEdit.Section = vm.sections[2];
        vm.courseInEdit.Professor = 'new professor';
        vm.courseInEdit.Year = 1234;
        vm.courseInEdit.Semester = vm.semesters[2];

        spyOn(courseServiceMock, 'add').and.callThrough();

        vm.save();
        scope.$apply();

        expect(vm.editingCourse).toBeFalsy();

        expect(courseServiceMock.add).toHaveBeenCalledWith({
            Id: 'new guid',
            Name: 'new name',
            Department: 'new department',
            Number: 2345,
            Section: 'C',
            Professor: 'new professor',
            Year: 1234,
            Semester: 'SUMMER'
        });

        expect(vm.courses[2].Id).toBe('new guid');
        expect(vm.courses[2].Name).toBe('new name');
        expect(vm.courses[2].Department).toBe('new department');
        expect(vm.courses[2].Number).toBe(2345);
        expect(vm.courses[2].Section).toBe('C');
        expect(vm.courses[2].Professor).toBe('new professor');
        expect(vm.courses[2].Year).toBe(1234);
        expect(vm.courses[2].Semester).toBe('SUMMER');

        expect(vm.courseInEdit.Id).toBe('');
        expect(vm.courseInEdit.Name).toBe('');
        expect(vm.courseInEdit.Department).toBe('');
        expect(vm.courseInEdit.Number).toBe(0);
        expect(vm.courseInEdit.Section).toBe(vm.sections[0]);
        expect(vm.courseInEdit.Professor).toBe('');
        expect(vm.courseInEdit.Year).toBe(0);
        expect(vm.courseInEdit.Semester).toBe(vm.semesters[0]);
    });

    it('cancels adding a new course', function () {
        vm.edit();

        vm.courseInEdit.Name = 'new name';
        vm.courseInEdit.Department = 'new department';
        vm.courseInEdit.Number = 2345;
        vm.courseInEdit.Section = vm.sections[2];
        vm.courseInEdit.Professor = 'new professor';
        vm.courseInEdit.Year = 1234;
        vm.courseInEdit.Semester = vm.semesters[2];

        vm.cancel();
        scope.$digest();

        expect(vm.editingCourse).toBeFalsy();
        expect(vm.courseInEdit.Id).toBe('');
        expect(vm.courseInEdit.Name).toBe('');
        expect(vm.courseInEdit.Department).toBe('');
        expect(vm.courseInEdit.Number).toBe(0);
        expect(vm.courseInEdit.Section).toBe(vm.sections[0]);
        expect(vm.courseInEdit.Professor).toBe('');
        expect(vm.courseInEdit.Year).toBe(0);
        expect(vm.courseInEdit.Semester).toBe(vm.semesters[0]);
        expect(vm.courses.length).toBe(2);
    });

    it('places a course in edit', function () {
        vm.edit(1);

        expect(vm.editingCourse).toBeTruthy();
        expect(vm.courseInEdit.Id).toBe('second guid');
        expect(vm.courseInEdit.Name).toBe('second name');
        expect(vm.courseInEdit.Department).toBe('second department');
        expect(vm.courseInEdit.Number).toBe(1337);
        expect(vm.courseInEdit.Section).toBe('B');
        expect(vm.courseInEdit.Professor).toBe('second professor');
        expect(vm.courseInEdit.Year).toBe(42);
        expect(vm.courseInEdit.Semester).toBe('SPRING');
    });

    it('can make changes to a course without altering the original', function () {
        vm.edit(1);

        vm.courseInEdit.Name = 'new name';
        vm.courseInEdit.Department = 'new department';
        vm.courseInEdit.Number = 2345;
        vm.courseInEdit.Section = vm.sections[2];
        vm.courseInEdit.Professor = 'new professor';
        vm.courseInEdit.Year = 1234;
        vm.courseInEdit.Semester = vm.semesters[2];

        expect(vm.courseInEdit.Id).toBe('second guid');
        expect(vm.courseInEdit.Name).toBe('new name');
        expect(vm.courseInEdit.Department).toBe('new department');
        expect(vm.courseInEdit.Number).toBe(2345);
        expect(vm.courseInEdit.Section).toBe('C');
        expect(vm.courseInEdit.Professor).toBe('new professor');
        expect(vm.courseInEdit.Year).toBe(1234);
        expect(vm.courseInEdit.Semester).toBe('SUMMER');

        expect(vm.courses[1].Id).toBe('second guid');
        expect(vm.courses[1].Name).toBe('second name');
        expect(vm.courses[1].Department).toBe('second department');
        expect(vm.courses[1].Number).toBe(1337);
        expect(vm.courses[1].Section).toBe('B');
        expect(vm.courses[1].Professor).toBe('second professor');
        expect(vm.courses[1].Year).toBe(42);
        expect(vm.courses[1].Semester).toBe('SPRING');
    });

    it('can cancel changes to a course', function () {
        vm.edit(1);

        vm.courseInEdit.Name = 'new name';
        vm.courseInEdit.Department = 'new department';
        vm.courseInEdit.Number = 2345;
        vm.courseInEdit.Section = vm.sections[2];
        vm.courseInEdit.Professor = 'new professor';
        vm.courseInEdit.Year = 1234;
        vm.courseInEdit.Semester = vm.semesters[2];

        vm.cancel();
        scope.$digest();

        expect(vm.editingCourse).toBeFalsy();
        expect(vm.courseInEdit.Id).toBe('');
        expect(vm.courseInEdit.Name).toBe('');
        expect(vm.courseInEdit.Department).toBe('');
        expect(vm.courseInEdit.Number).toBe(0);
        expect(vm.courseInEdit.Section).toBe(vm.sections[0]);
        expect(vm.courseInEdit.Professor).toBe('');
        expect(vm.courseInEdit.Year).toBe(0);
        expect(vm.courseInEdit.Semester).toBe(vm.semesters[0]);

        expect(vm.courses[1].Id).toBe('second guid');
        expect(vm.courses[1].Name).toBe('second name');
        expect(vm.courses[1].Department).toBe('second department');
        expect(vm.courses[1].Number).toBe(1337);
        expect(vm.courses[1].Section).toBe('B');
        expect(vm.courses[1].Professor).toBe('second professor');
        expect(vm.courses[1].Year).toBe(42);
        expect(vm.courses[1].Semester).toBe('SPRING');
    });

    it('updates a course', function () {
        vm.edit(1);

        vm.courseInEdit.Name = 'new name';
        vm.courseInEdit.Department = 'new department';
        vm.courseInEdit.Number = 2345;
        vm.courseInEdit.Section = vm.sections[2];
        vm.courseInEdit.Professor = 'new professor';
        vm.courseInEdit.Year = 1234;
        vm.courseInEdit.Semester = vm.semesters[2];

        spyOn(courseServiceMock, 'update').and.callThrough();

        vm.save();
        scope.$apply();

        expect(vm.editingCourse).toBeFalsy();

        expect(courseServiceMock.update).toHaveBeenCalledWith({
            Id: 'second guid',
            Name: 'new name',
            Department: 'new department',
            Number: 2345,
            Section: 'C',
            Professor: 'new professor',
            Year: 1234,
            Semester: 'SUMMER'
        });

        expect(vm.editingCourse).toBeFalsy();
        expect(vm.courseInEdit.Id).toBe('');
        expect(vm.courseInEdit.Name).toBe('');
        expect(vm.courseInEdit.Department).toBe('');
        expect(vm.courseInEdit.Number).toBe(0);
        expect(vm.courseInEdit.Section).toBe(vm.sections[0]);
        expect(vm.courseInEdit.Professor).toBe('');
        expect(vm.courseInEdit.Year).toBe(0);
        expect(vm.courseInEdit.Semester).toBe(vm.semesters[0]);

        expect(vm.courses[1].Id).toBe('second guid');
        expect(vm.courses[1].Name).toBe('new name');
        expect(vm.courses[1].Department).toBe('new department');
        expect(vm.courses[1].Number).toBe(2345);
        expect(vm.courses[1].Section).toBe('C');
        expect(vm.courses[1].Professor).toBe('new professor');
        expect(vm.courses[1].Year).toBe(1234);
        expect(vm.courses[1].Semester).toBe('SUMMER');
    });

    it('removes a course', function () {
        vm.edit(0);

        spyOn(courseServiceMock, 'remove').and.callThrough();

        expect(vm.courseInEdit.Id).toBe('first guid');

        vm.remove();
        scope.$apply();

        expect(courseServiceMock.remove).toHaveBeenCalledWith('first guid');

        expect(vm.editingCourse).toBeFalsy();
        expect(vm.courseInEdit.Id).toBe('');
        expect(vm.courseInEdit.Name).toBe('');
        expect(vm.courseInEdit.Department).toBe('');
        expect(vm.courseInEdit.Number).toBe(0);
        expect(vm.courseInEdit.Section).toBe(vm.sections[0]);
        expect(vm.courseInEdit.Professor).toBe('');
        expect(vm.courseInEdit.Year).toBe(0);
        expect(vm.courseInEdit.Semester).toBe(vm.semesters[0]);

        expect(vm.courses.length).toBe(1);
        expect(vm.courses[0].Id).toBe('second guid');
        expect(vm.courses[0].Name).toBe('second name');
        expect(vm.courses[0].Department).toBe('second department');
        expect(vm.courses[0].Number).toBe(1337);
        expect(vm.courses[0].Section).toBe('B');
        expect(vm.courses[0].Professor).toBe('second professor');
        expect(vm.courses[0].Year).toBe(42);
        expect(vm.courses[0].Semester).toBe('SPRING');
    });

    it('checks for duplicate courses and finds them', function () {
        vm.edit();

        vm.courseInEdit.Name = 'new name';
        vm.courseInEdit.Department = vm.courses[0].Department;
        vm.courseInEdit.Number = vm.courses[0].Number;
        vm.courseInEdit.Section = vm.courses[0].Section;
        vm.courseInEdit.Professor = 'new professor';
        vm.courseInEdit.Year = vm.courses[0].Year;
        vm.courseInEdit.Semester = vm.courses[0].Semester;

        scope.$digest();

        expect(vm.duplicateCourseInEdit).toBeTruthy();
    });

    it('checks for duplicate courses and finds none', function () {
        vm.edit();

        vm.courseInEdit.Name = 'new name';
        vm.courseInEdit.Department = vm.courses[0].Department;
        vm.courseInEdit.Number = vm.courses[1].Number;
        vm.courseInEdit.Section = vm.courses[0].Section;
        vm.courseInEdit.Professor = 'new professor';
        vm.courseInEdit.Year = vm.courses[0].Year;
        vm.courseInEdit.Semester = vm.courses[0].Semester;

        scope.$digest();

        expect(vm.duplicateCourseInEdit).toBeFalsy();
    });

    it('putting a course in edit does not trigger duplicate', function () {
        vm.edit(1);

        scope.$digest();

        expect(vm.duplicateCourseInEdit).toBeFalsy();
    });
});