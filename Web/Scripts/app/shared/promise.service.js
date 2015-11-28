((function () {
    'use strict';

    angular
        .module('app.shared')
        .factory('promiseService', promiseService);

    promiseService.$inject = ['$http', '$q'];

    function promiseService($http, $q) {
        return {
            postPromise: postPromise
        };

        function postPromise(url, body) {
            var deferred = $q.defer();
            $http.post(url, body)
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        }
    }
})();