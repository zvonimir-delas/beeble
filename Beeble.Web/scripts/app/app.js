﻿var app = angular.module('myApp', ['ui.router', 'ngDialog', 'ngMessages', 'angular-hidScanner', 'angularjs-dropdown-multiselect']);

app.run(function (authService, $http) {

    authService.fillAuthData();
});