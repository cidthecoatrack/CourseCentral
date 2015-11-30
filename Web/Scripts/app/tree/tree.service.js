((function () {
    'use strict';

    angular
        .module('app.tree')
        .factory('treeService', treeService);

    treeService.$inject = ['promiseService'];

    function treeService(promiseService) {
        return {
            search: search
        };

        function search(tree, query) {
            var url = '/Trees/Search/' + tree + '/' + query;
            return promiseService.getPromise(url);
        }
    }
}))();