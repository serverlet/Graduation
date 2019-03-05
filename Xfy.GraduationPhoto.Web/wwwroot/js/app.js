// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    'use strict';

    angular
        .module('app', ['ngRoute', 'ngMaterial', 'ngMessages'])
        .config(route);
    function route($routeProvider) {
        $routeProvider
            .when('/login', { templateUrl: '/Account/Login', controller: 'logincontroller' })
            .when('/register', { templateUrl: '/Account/register', controller: 'register_controller' })
            .when('/home', { templateUrl: '/Home/index', controller: 'homecontroller' });
    }
})();