(function () {
    'use strict';

    angular
        .module('app')
        .controller('logincontroller', login_controller);

    //login_controller.$inject = ['$location'];

    function login_controller($location, $scope) {
        /* jshint validthis:true */
        
        activate();

        function activate() {
            let user = {};
            user.username = '';
            user.password = '';
            $scope.user = user;
        };

        /**
         * 注册
         * */
        $scope.register = function () {
            $location.path('register');
        };

    }
})();
