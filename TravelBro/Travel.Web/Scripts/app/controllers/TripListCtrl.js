define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('TripListCtrl', ['$scope', '$location', '$route', '$window', 'Backend', 'Auth', 'Entity', function ($scope, $location, $route, $window, Backend, Auth, Entity) {
        $scope.trips = Backend.trips.query();
        $scope.tripsOrder = 'Name';
        $scope.isCollapsed = true;
        $scope.commentText = "";

        $scope.create = function() {
          $location.path('/trips/new');
        };

        $scope.sendComment = function (trip) {
            if (!trip.commentText) {
                return;
            }
            var comment = Entity.comment.Default();
            comment.Text = trip.commentText;

            Backend.trips.saveComment({ tripId: trip.Id }, comment, function () {
                trip.Comments.push(comment);
                trip.commentText = "";
            });
        }
  }]);
});