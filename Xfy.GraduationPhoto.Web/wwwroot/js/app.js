// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    'use strict';

    angular
        .module('app', ['ui.router', 'ngMaterial', 'ngMessages'])
        .config(route);
    function route($stateProvider) {
        $stateProvider.state({ name: 'main', url: '', templateUrl: '/Account/Login', controller: 'logincontroller' });
        $stateProvider.state({ name: 'login', url: '/login', templateUrl: '/Account/Login', controller: 'logincontroller' });
        $stateProvider.state({ name: 'register', url: '/register', templateUrl: '/Account/register', controller: 'register_controller' });
        $stateProvider.state({ name: 'home', url: 'home', templateUrl: '/Home/index', controller: 'homecontroller' });
    }
})();