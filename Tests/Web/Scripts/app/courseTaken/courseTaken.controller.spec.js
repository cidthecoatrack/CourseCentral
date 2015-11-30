'use strict'

describe('Course Taken Controller', function () {
    var vm;
    var courseTakenServiceMock;
    var bootstrapDataMock;
    var q;
    var scope;
    var controllerFactory;

    beforeEach(module('app.courseTaken'));

    beforeEach(function () {
        courseTakenServiceMock = {
            add: function (courseTaken) {
                return postMockedPromise();
            },
            update: function (courseTaken) {
                return postMockedPromise();
            },
            remove: function (courseTaken) {
                return postMockedPromise();
            }
        };

        bootstrapDataMock = {
            model: {
                CoursesTaken: [
                    {
                        Student: { Id: 'first student guid', Name: 'first student name' },
                        Course: { Id: 'first course guid', Name: 'first course name' },
                        Grade: 9266
                    }, {
                        Student: { Id: 'second student guid', Name: 'second student name' },
                        Course: { Id: 'second course guid', Name: 'second course name' },
                        Grade: 90210
                    }
                ],
                ToAssign: [
                    { Id: 'first to assign guid', Name: 'first to assign name' },
                    { Id: 'second to assign guid', Name: 'second to assign name' }
                ],
                Course: { Id: 'course guid', Name: 'course name' },
                Student: { Id: 'student guid', Name: 'student name' }
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
        controllerFactory = $controller;

        vm = controllerFactory('CourseTaken as vm', {
            $scope: scope,
            bootstrapData: bootstrapDataMock,
            courseTakenService: courseTakenServiceMock,
        });
    }));

    it('has all courses taken from bootstrap data', function () {
        expect(vm.coursesTaken).toBe(bootstrapDataMock.model.CoursesTaken);
    });

    it('has all assignable models from bootstrap data', function () {
        expect(vm.toAssign).toBe(bootstrapDataMock.model.ToAssign);
    });

    it('has course and student from bootstrap data', function () {
        expect(vm.student).toBe(bootstrapDataMock.model.Student);
        expect(vm.course).toBe(bootstrapDataMock.model.Course);
    });

    it('has a course taken for editing and adding with set student', function () {
        bootstrapDataMock.model.Course = null;

        vm = controllerFactory('CourseTaken as vm', {
            $scope: scope,
            bootstrapData: bootstrapDataMock,
            courseTakenService: courseTakenServiceMock,
        });

        expect(vm.courseTakenInEdit.Student).toBe(vm.student);
        expect(vm.courseTakenInEdit.Course.Id).toBe('');
        expect(vm.courseTakenInEdit.Course.Name).toBe('');
        expect(vm.courseTakenInEdit.Grade).toBe(0);
    });

    it('has a course taken for editing and adding with set course', function () {
        bootstrapDataMock.model.Student = null;

        vm = controllerFactory('CourseTaken as vm', {
            $scope: scope,
            bootstrapData: bootstrapDataMock,
            courseTakenService: courseTakenServiceMock,
        });

        expect(vm.courseTakenInEdit.Course).toBe(vm.course);
        expect(vm.courseTakenInEdit.Student.Id).toBe('');
        expect(vm.courseTakenInEdit.Student.Name).toBe('');
        expect(vm.courseTakenInEdit.Grade).toBe(0);
    });

    it('is not editing a course taken on load', function () {
        expect(vm.editingCourseTaken).toBeFalsy();
    });

    it('has no duplicate course taken on load', function () {
        expect(vm.duplicateCourseTakenInEdit).toBeFalsy();
    });

    it('prepares a new course taken to be added', function () {
        vm.edit();

        expect(vm.editingCourseTaken).toBeTruthy();
    });

    it('adds a new course taken', function () {
        vm.edit();

        vm.courseTakenInEdit.Student = vm.toAssign[0];
        vm.courseTakenInEdit.Course = vm.toAssign[1];
        vm.courseTakenInEdit.Grade = 42;

        spyOn(courseTakenServiceMock, 'add').and.callThrough();

        vm.save();
        scope.$apply();

        expect(vm.editingCourseTaken).toBeFalsy();

        expect(courseTakenServiceMock.add).toHaveBeenCalledWith({
            Student: { Id: 'first to assign guid', Name: 'first to assign name' },
            Course: { Id: 'second to assign guid', Name: 'second to assign name' },
            Grade: 42
        });

        expect(vm.coursesTaken[2].Student).toBe(vm.toAssign[0]);
        expect(vm.coursesTaken[2].Course).toBe(vm.toAssign[1]);
        expect(vm.coursesTaken[2].Grade).toBe(42);

        expect(vm.courseTakenInEdit.Student).toBe(vm.student);
        expect(vm.courseTakenInEdit.Course).toBe(vm.course);
        expect(vm.courseTakenInEdit.Grade).toBe(0);
    });

    it('cancels adding a new course taken', function () {
        vm.edit();

        vm.courseTakenInEdit.Student = vm.toAssign[0];
        vm.courseTakenInEdit.Course = vm.toAssign[1];
        vm.courseTakenInEdit.Grade = 42;

        vm.cancel();
        scope.$digest();

        expect(vm.editingCourseTaken).toBeFalsy();
        expect(vm.courseTakenInEdit.Student).toBe(vm.student);
        expect(vm.courseTakenInEdit.Course).toBe(vm.course);
        expect(vm.courseTakenInEdit.Grade).toBe(0);
        expect(vm.coursesTaken.length).toBe(2);
    });

    it('places a course taken in edit', function () {
        vm.edit(1);

        expect(vm.editingCourseTaken).toBeTruthy();
        expect(vm.courseTakenInEdit.Student).toBe(vm.coursesTaken[1].Student);
        expect(vm.courseTakenInEdit.Course).toBe(vm.coursesTaken[1].Course);
        expect(vm.courseTakenInEdit.Grade).toBe(90210);
    });

    it('can make changes to a course taken without altering the original', function () {
        vm.edit(1);

        vm.courseTakenInEdit.Student = vm.toAssign[0];
        vm.courseTakenInEdit.Course = vm.toAssign[1];
        vm.courseTakenInEdit.Grade = 42;

        expect(vm.courseTakenInEdit.Student).toBe(vm.toAssign[0]);
        expect(vm.courseTakenInEdit.Course).toBe(vm.toAssign[1]);
        expect(vm.courseTakenInEdit.Grade).toBe(42);

        expect(vm.coursesTaken[1].Student).toBe(bootstrapDataMock.model.CoursesTaken[1].Student);
        expect(vm.coursesTaken[1].Course).toBe(bootstrapDataMock.model.CoursesTaken[1].Course);
        expect(vm.coursesTaken[1].Grade).toBe(90210);
    });

    it('can cancel changes to a course taken', function () {
        vm.edit(1);

        vm.courseTakenInEdit.Student = vm.toAssign[0];
        vm.courseTakenInEdit.Course = vm.toAssign[1];
        vm.courseTakenInEdit.Grade = 42;

        vm.cancel();
        scope.$digest();

        expect(vm.editingCourseTaken).toBeFalsy();
        expect(vm.courseTakenInEdit.Student).toBe(vm.student);
        expect(vm.courseTakenInEdit.Course).toBe(vm.course);
        expect(vm.courseTakenInEdit.Grade).toBe(0);

        expect(vm.coursesTaken[1].Student).toBe(bootstrapDataMock.model.CoursesTaken[1].Student);
        expect(vm.coursesTaken[1].Course).toBe(bootstrapDataMock.model.CoursesTaken[1].Course);
        expect(vm.coursesTaken[1].Grade).toBe(90210);
    });

    it('updates a course', function () {
        vm.edit(1);

        vm.courseTakenInEdit.Student = vm.toAssign[0];
        vm.courseTakenInEdit.Course = vm.toAssign[1];
        vm.courseTakenInEdit.Grade = 42;

        spyOn(courseTakenServiceMock, 'update').and.callThrough();

        vm.save();
        scope.$apply();

        expect(vm.editingCourseTaken).toBeFalsy();

        expect(courseTakenServiceMock.update).toHaveBeenCalledWith({
            Student: { Id: 'first to assign guid', Name: 'first to assign name' },
            Course: { Id: 'second to assign guid', Name: 'second to assign name' },
            Grade: 42
        });

        expect(vm.courseTakenInEdit.Student).toBe(vm.student);
        expect(vm.courseTakenInEdit.Course).toBe(vm.course);
        expect(vm.courseTakenInEdit.Grade).toBe(0);

        expect(vm.coursesTaken[1].Student).toBe(vm.toAssign[0]);
        expect(vm.coursesTaken[1].Course).toBe(vm.toAssign[1]);
        expect(vm.coursesTaken[1].Grade).toBe(42);
    });

    it('removes a course', function () {
        vm.edit(0);

        spyOn(courseTakenServiceMock, 'remove').and.callThrough();

        vm.remove();
        scope.$apply();

        expect(courseTakenServiceMock.remove).toHaveBeenCalledWith({
            Student: { Id: 'first student guid', Name: 'first student name' },
            Course: { Id: 'first course guid', Name: 'first course name' },
            Grade: 9266
        });

        expect(vm.editingCourseTaken).toBeFalsy();

        expect(vm.courseTakenInEdit.Student).toBe(vm.student);
        expect(vm.courseTakenInEdit.Course).toBe(vm.course);
        expect(vm.courseTakenInEdit.Grade).toBe(0);

        expect(vm.coursesTaken.length).toBe(1);
        expect(vm.coursesTaken[0].Student.Id).toBe('second student guid');
        expect(vm.coursesTaken[0].Student.Name).toBe('second student name');
        expect(vm.coursesTaken[0].Course.Id).toBe('second course guid');
        expect(vm.coursesTaken[0].Course.Name).toBe('second course name');
        expect(vm.coursesTaken[0].Grade).toBe(90210);
    });
});