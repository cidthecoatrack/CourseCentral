'use strict'

describe('Tree Controller', function () {
    var vm;
    var treeServiceMock;
    var q;
    var scope;
    var levels;

    beforeEach(module('app.tree'));

    beforeEach(function () {
        treeServiceMock = {
            search: function (tree, query) {
                var body = { levels: levels };
                return getMockedPromise(body);
            }
        };

        levels = [];
    });

    function getMockedPromise(body) {
        var deferred = q.defer();
        deferred.resolve(body);
        return deferred.promise;
    }

    beforeEach(inject(function ($rootScope, $controller, $q) {
        q = $q;
        scope = $rootScope.$new();

        vm = $controller('Tree as vm', {
            $scope: scope,
            treeService: treeServiceMock
        });
    }));

    it('has initial empty values', function () {
        expect(vm.levels.length).toBe(0);
        expect(vm.tree).toBe('');
        expect(vm.query).toBe(0);
    });

    it('performs a search when the tree changes', function () {
        levels.push(90210);
        levels.push(42);

        vm.tree = '1,2,3';
        scope.$digest();

        expect(vm.levels[0]).toBe(90210);
        expect(vm.levels[1]).toBe(42);
        expect(vm.levels.length).toBe(2);
    });

    it('performs a search when the query changes', function () {
        levels.push(90210);
        levels.push(42);

        vm.tree = '1,2,3';
        scope.$digest();

        levels.push(600);

        vm.query = 9266;
        scope.$digest();

        expect(vm.levels[0]).toBe(90210);
        expect(vm.levels[1]).toBe(42);
        expect(vm.levels[2]).toBe(600);
        expect(vm.levels.length).toBe(3);
    });

    it('registers when the tree is invalid and does not perform a search', function () {
        levels.push(90210);
        levels.push(42);

        vm.tree = '1,2,3,tree';
        scope.$digest();

        expect(vm.levels.length).toBe(0);
    });

    it('does not perform search when query changes and tree is invalid', function () {
        levels.push(90210);
        levels.push(42);

        vm.tree = '1,2,3,tree';
        scope.$digest();

        vm.query = 9266;
        scope.$digest();

        expect(vm.levels.length).toBe(0);
    });
});