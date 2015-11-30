'use strict'

describe('Course Service', function () {
    var courseService;
    var promiseServiceMock;

    beforeEach(module('app.course', function ($provide) {
        promiseServiceMock = {};
        promiseServiceMock.postPromise = jasmine.createSpy();

        $provide.value('promiseService', promiseServiceMock);
    }));

    beforeEach(inject(function (_courseService_) {
        courseService = _courseService_;
    }));

    it('adds a course', function () {
        var course = { Id: 'new guid', Name: 'new name' };
        var body = { course: course };

        var promise = courseService.add(course);
        expect(promise).not.toBeNull();
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('Courses/Add', body);
    });

    it('updates a course', function () {
        var course = { Id: 'new guid', Name: 'new name' };
        var body = { course: course };

        var promise = courseService.update(course);
        expect(promise).not.toBeNull();
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('Courses/Update', body);
    });

    it('removes a course', function () {
        var body = { courseId: 'new guid' };

        var promise = courseService.remove('new guid');
        expect(promise).not.toBeNull();
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('Courses/Remove', body);
    });
});