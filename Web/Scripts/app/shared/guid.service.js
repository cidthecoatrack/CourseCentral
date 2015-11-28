(function () {
    'use strict';

    angular
        .module('app.shared')
        .factory('guidService', guidService);

    function guidService() {
        return {
            generate: generate
        };

        function generate() {
            return uuid.v4();
        }
    }
})();