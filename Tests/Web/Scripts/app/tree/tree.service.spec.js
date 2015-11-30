'use strict'

describe('Tree Service', function () {
    var treeService;
    var promiseServiceMock;

    beforeEach(module('app.tree', function ($provide) {
        promiseServiceMock = {};
        promiseServiceMock.getPromise = jasmine.createSpy();

        $provide.value('promiseService', promiseServiceMock);
    }));

    beforeEach(inject(function (_treeService_) {
        treeService = _treeService_;
    }));

    it('performs a search', function () {
        var promise = treeService.search('tree', 'value');
        expect(promise).not.toBeNull();
        expect(promiseServiceMock.getPromise).toHaveBeenCalledWith('/Trees/Search/tree/value');
    });
});