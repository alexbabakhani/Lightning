'use strict';

angular.module('app', ['ui.router', 'app.filters', 'app.services', 'app.directives', 'app.controllers'])
    .config(['$stateProvider', '$locationProvider', '$httpProvider', function ($stateProvider, $locationProvider, $httpProvider) {

        delete $httpProvider.defaults.headers.common['X-Requested-With'];

        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/views/index',
                controller: 'HomeCtrl'

            })
            .state('otherwise', {
                url: '/',
                templateUrl: '/views/index',
                controller: 'HomeCtrl'
            });

        $locationProvider.html5Mode(true);

    }])

    .run(['$templateCache', '$rootScope', '$state', '$stateParams', function ($templateCache, $rootScope, $state, $stateParams) {
        var view = angular.element('#ui-view');
        $templateCache.put(view.data('tmpl-url'), view.html());
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        $rootScope.$on('$stateChangeSuccess', function (event, toState) {
            $rootScope.layout = toState.layout;
        });
    }]);