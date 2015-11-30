'use strict'

describe('Course Taken Service', function () {
    var courseTakenService;
    var promiseServiceMock;

    beforeEach(module('app.courseTaken', function ($provide) {
        promiseServiceMock = {};
        promiseServiceMock.postPromise = jasmine.createSpy();

        $provide.value('promiseService', promiseServiceMock);
    }));

    beforeEach(inject(function (_courseTakenService_) {
        courseTakenService = _courseTakenService_;
    }));

    it('adds a course taken', function () {
        var courseTaken = { Id: 'new guid', Name: 'new name' };
        var body = { courseTaken: courseTaken };

        var promise = courseTakenService.add(courseTaken);
        expect(promise).not.toBeNull();
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('CoursesTaken/Add', body);
    });

    it('updates a course taken', function () {
        var courseTaken = { Id: 'new guid', Name: 'new name' };
        var body = { courseTaken: courseTaken };

        var promise = courseTakenService.update(courseTaken);
        expect(promise).not.toBeNull();
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('CoursesTaken/Update', body);
    });

    it('removes a course taken', function () {
        var courseTaken = { Id: 'new guid', Name: 'new name' };
        var body = { courseTaken: courseTaken };

        var promise = courseTakenService.remove(courseTaken);
        expect(promise).not.toBeNull();
        expect(promiseServiceMock.postPromise).toHaveBeenCalledWith('CoursesTaken/Remove', body);
    });
});