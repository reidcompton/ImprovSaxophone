var measure = angular.module('measure', []), start = new Date();

//measure.controller('MeasureCtrl', function ($scope, $http) {
//    $http.get('home/measure', {
//        params: {
//            start: start.toISOString(),
//            bpm: 140,
//            tsN: 4,
//            tsD: 4,
//            root: "c",
//            quality: "major",
//            auxiliary: "6"
//        }
//    }).success(function (data) {
//        $scope.measures = data;
//        console.log($scope.measures);
//    });
//});