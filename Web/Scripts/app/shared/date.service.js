(function () {
    'use strict';

    angular
        .module('app.shared')
        .factory('dateService', dateService);

    function dateService() {
        return {
            parseJsonDate: parseJsonDate
        };

        function parseJsonDate(jsonDate) {
            var dateInt = parseInt(jsonDate.substr(6));
            if (dateInt > 0)
                return new Date(dateInt);

            return '';
        }
    }
})();