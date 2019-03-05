(function () {
    'use strict';

    angular
        .module('app')
        .controller('register_controller', register_controller);

    //register_controller.$inject = ['$location'];

    function register_controller($location, $scope) {
        /* jshint validthis:true */

        activate();
        function activate() {
            let user = {};
            user.sex = 0;
            $scope.user = user;
        }

        $scope.back = function () {
            $location.path('login');
        };
    }
})();
