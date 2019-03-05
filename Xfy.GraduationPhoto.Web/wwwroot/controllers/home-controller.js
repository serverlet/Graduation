(function () {
    'use strict';

    angular
        .module('app')
        .controller('homecontroller', login_controller);

    //login_controller.$inject = ['$location'];

    function login_controller($location, $scope) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'login_controller';

        activate();

        function activate() { }
    }
})();
