(function () {
    'use strict';

    angular
        .module('app')
        .controller('forward_controller', forward_controller);

    //forward_controller.$inject = ['$location'];

    function forward_controller($location, $scope) {
        /* jshint validthis:true */

        activate();

        function activate() {
            //services.hideTabBar = false;
        }

        /**
         * ng-include载入后页面
         * */
        $scope.load = function () {

        };
    }
})();
