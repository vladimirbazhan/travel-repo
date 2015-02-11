define(['./module'], function (services) {
    'use strict';
    
    services.factory('GMapsUtils', [function () {
        var service = {
            performNearbySearch: function(map, center, callback) {
                new NearbySearch(map, center).execute(callback);
            }
        };

        return service;
    }]);

    // implementations

    /// performNearbySearch

    function NearbySearch(map, center) {
        this.params = {
            map: map,
            center: center
        };

        this.execute = function (callback) {
            this.callback = callback;
            execute.call(this, this.params.map, this.params.center);
        }

        function boundsToMeters(bounds) {
            var sw = bounds.getSouthWest();
            var ne = bounds.getNorthEast();
            var distance = google.maps.geometry.spherical.computeDistanceBetween(sw, ne);
            return distance;
        }

        function mapBoundsToMeters(map) {
            var bounds = map.getBounds();
            return boundsToMeters(bounds);
        }

        function execute(map, center) {
            var oldZoom = map.getZoom();

            var zoomFrom = 3;
            var zoomTo = 12;
            this.callTimes = zoomTo - zoomFrom;
            this.searchRequests = [];
            this.searchResults = [];

            var delay = 0;
            for (var zoom = zoomFrom; zoom < zoomTo; zoom++) {

                var reqFunc = function (currZoom) {
                    setTimeout($.proxy(function () {
                        map.setZoom(currZoom);
                        var searchRadius = mapBoundsToMeters(map) / 20;
                        var service = new google.maps.places.PlacesService(map);
                        service.nearbySearch({
                            location: center,
                            radius: searchRadius
                        }, $.proxy(function (a, b) {
                             nearbySearchCallback.call(this, a, b, currZoom);
                        }, this), this);
                        map.setZoom(oldZoom);
                    }, this), delay);
                };

                reqFunc.call(this, zoom);

                this.searchRequests.push({
                    func: reqFunc,
                    zoom: zoom
                });

                delay += 0;
            }
        };

        function nearbySearchCallback(results, status, zoom) {
            this.callTimes--;
            switch (status) {
                case google.maps.places.PlacesServiceStatus.OK:
                    {
                        results = results.filter(function (a) {
                            return a.types.indexOf("establishment") != -1;
                        });
                        if (results.length == 0) {
                            break;
                        }

                        $.merge(this.searchResults, results);
                        this.searchResults = this.searchResults.unique(function (a, b) {
                            return a.place_id === b.place_id;
                        });
                        break;
                    }
                case google.maps.places.PlacesServiceStatus.ZERO_RESULTS:
                    {
                        break;
                    }
                case google.maps.places.PlacesServiceStatus.OVER_QUERY_LIMIT:
                    {
                        var requestData = this.searchRequests.find(function (elem) {
                            return elem.zoom == zoom;
                        });
                        if (requestData) {
                            this.callTimes++;
                            requestData.func.call(this, zoom);
                        }
                        break;
                    }
                default:
                    {
                        debugger;
                    }
            }
            if (this.callTimes == 0) {
                this.searchRequests = [];
                nearbySearchSeriesFinishedCallback.call(this, this.searchResults);
            }
        };

        function nearbySearchSeriesFinishedCallback(results) {
            results.sort(function (a, b) {
                var x1 = !!a.geometry.viewport ? 1 : 0;
                var x2 = !!b.geometry.viewport ? 1 : 0;
                return x2 - x1;
            });
            results.sort(function (a, b) {
                if (!(a.geometry.viewport && b.geometry.viewport))
                    return;
                return boundsToMeters(b.geometry.viewport) - boundsToMeters(a.geometry.viewport);
            });

            var curr = 0;
            var searchRadius = mapBoundsToMeters(map) / 10;
            while (curr < results.length) {
                var currLocation = results[curr].geometry.location;
                var distance = google.maps.geometry.spherical.computeDistanceBetween(currLocation, center);
                if (distance > searchRadius) {
                    results.splice(curr, 1);
                } else {
                    curr++;
                }
            }

            results = results.filter(function (a) {
                return a.types.indexOf("establishment") != -1;
            });

            this.callback(results);
        }
    }

    ///--- performNearbySearch
});