'use strict'

describe('Student Service', function () {
    var studentService;
    var promiseServiceMock;

    beforeEach(module('app.student', function ($provide) {
        promiseServiceMock = {};
        promiseServiceMock.postPromise = jasmine.createSpy();

        $provide.value('promiseService', promiseServiceMock);
    }));

    beforeEach(inject(function (_studentService_) {
        studentService = _studentService_;
    }));

    it('adds a student', function () {
        var student = { Id: 'new guid', Name: 'new name' };
        var body = { student: student };

        studentService.add(student);
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('Students/Add', body);
    });

    it('updates a student', function () {
        var student = { Id: 'new guid', Name: 'new name' };
        var body = { student: student };

        studentService.update(student);
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('Students/Update', body);
    });

    it('removes a student', function () {
        var body = { studentId: 'new guid' };

        studentService.remove('new guid');
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('Students/Remove', body);
    });
});