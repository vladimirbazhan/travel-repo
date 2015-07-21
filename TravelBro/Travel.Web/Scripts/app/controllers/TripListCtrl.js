define(['./module'], function (controllers) {
    'use strict';
    controllers.controller('TripListCtrl', TripListCtrl);

    TripListCtrl.$inject = ['$location', '$route', '$window', 'Backend', 'Auth', 'Entity'];

    function TripListCtrl($location, $route, $window, Backend, Auth, Entity) {
        var vm = {
            trips: Backend.trips.query(),
            tripsOrder: 'Name',
            commentText: "",
            create: function() {
                $location.path('/trips/edit/new');
            },
            sendComment: sendComment
        };
        $.extend(this, vm);

        // collapse all comments
        vm.trips.$promise.then(function(items) {
            items.forEach(function(item) {
                item.isCommentsCollapsed = true;
            });
        });

        function sendComment(trip) {
            if (!trip.commentText) {
                return;
            }
            var comment = Entity.comment.Default();
            comment.Text = trip.commentText;

            Backend.trips.saveComment({ tripId: trip.Id }, comment, function (res) {
                trip.Comments.push(res);
                trip.commentText = "";
            });
        }
  };
});