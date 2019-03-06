(function () {
    'use strict';

    angular
        .module('app')
        .controller('index_controller', index_controller);

    //index_controller.$inject = ['$location'];

    function index_controller($location, $scope) {
        /* jshint validthis:true */

        activate();

        function activate() { }
    }
})();
