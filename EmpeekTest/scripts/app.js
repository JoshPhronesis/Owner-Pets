///<reference path="angular.js"/>
/// <reference path="angular-route.js" />

var myApp = angular.module("myModule", ["ngRoute"])
    .config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when("/home", {
                templateUrl: "views/home.html",
                controller: "homeController"
            })
            .when('/owner/:id', {
                templateUrl: "views/ownerpets.html",
                controller: "ownerPetsController"
            })
            .otherwise({
                redirectTo: "/home"
            })

        $locationProvider.html5Mode(true);
    })
   
    

angular.module('myModule').filter('pagination', function () {
    return function (input, start) {
        start = parseInt(start, 10);
        return input.slice(start);
    };
});