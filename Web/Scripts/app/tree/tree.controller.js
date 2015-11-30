((function () {
    'use strict';

    angular
        .module('app.tree')
        .controller('Tree', Tree);

    Tree.$inject = ['$scope', 'treeService'];

    function Tree($scope, treeService) {
        var vm = this;

        vm.levels = [];
        vm.tree = '';
        vm.query = 0;

        var validTreeRegex = /^(\d+,*)+$/;

        function search() {
            treeService.search(vm.tree, vm.query).then(function (data) {
                vm.levels = data.levels;
            });
        }

        $scope.$watch('vm.tree', function () {
            var validTree = validTreeRegex.test(vm.tree);

            if (validTree)
                search();
        });

        $scope.$watch('vm.query', function () {
            var validTree = validTreeRegex.test(vm.tree);

            if (validTree)
                search();
        });
    }
}))();