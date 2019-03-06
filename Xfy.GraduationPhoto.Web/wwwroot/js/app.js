// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    'use strict';

    angular
        .module('app', ['ui.router', 'ngMaterial', 'ngMessages'])
        .config(route)
    //.config(icon);
    function route($stateProvider) {
        $stateProvider.state({ name: 'main', url: '', templateUrl: '/Account/Login', controller: 'logincontroller' });
        $stateProvider.state({ name: 'login', url: '/login', templateUrl: '/Account/Login', controller: 'logincontroller' });
        $stateProvider.state({ name: 'register', url: '/register', templateUrl: '/Account/Register', controller: 'register_controller' });
        $stateProvider.state({ name: 'index', url: '/index', templateUrl: '/Home/Index', controller: 'index_controller' });
        $stateProvider.state({ name: 'forward', url: '/forward', templateUrl: '/Home/Forward', controller: 'forward_controller' });
    }
    //function icon($mdIconProvider) {
    //    $mdIconProvider
    //        .iconSet("call", 'images/icons/more_vert.svg');
    //}
})();