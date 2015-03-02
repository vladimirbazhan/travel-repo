define(['./module'], function (directives) {
    'use strict';

    directives.directive('map', [function () {
        var _scope, _element, _map, _control, _contextMenu;

        return {
            scope: {
                options: '=',
                map: '=',
                control: '='
            },
            restrict: 'E',
            template: '<div></div>',
            replace: true,
            link: function (scope, element) {
                _scope = scope;
                _element = element;

                _control = scope.control || {};
                $.extend(_control, {
                    setContextMenu: setContextMenu
                });                

                scope.options = scope.options || {};
                scope.map = _map = new google.maps.Map(element.get(0), scope.options);
            }
        };

        function setContextMenu() {
            google.maps.event.addListener(_map, 'rightclick', function (e) {
                debugger;
                _contextMenu = new MapContextMenu(_map, _element);
                _contextMenu.show(e.latLng);
            });            
        }

        function MapContextMenu(map, elem) {
            this.map = map;
            this.element = elem;
            this.show = function(latLng) {
                //var projection;
                var contextmenuDir;
                //projection = this.map.getProjection();
                $('.contextmenu').remove();
                contextmenuDir = document.createElement("div");
                contextmenuDir.className = 'contextmenu';
                contextmenuDir.innerHTML = "<a id='menu1'><div class=context>menu item 1<\/div><\/a><a id='menu2'><div class=context>menu item 2<\/div><\/a>";
                $(this.map.getDiv()).append(contextmenuDir);

                this.setMenuXY(latLng);

                contextmenuDir.style.visibility = "visible";
            }

            // private
            this.setMenuXY = function (latLng) {
                var mapWidth = this.element.width();
                var mapHeight = this.element.height();
                var menuWidth = $('.contextmenu').width();
                var menuHeight = $('.contextmenu').height();
                var clickedPosition = this.getCanvasXY(latLng);
                var x = clickedPosition.x;
                var y = clickedPosition.y;

                if ((mapWidth - x) < menuWidth)
                    x = x - menuWidth;
                if ((mapHeight - y) < menuHeight)
                    y = y - menuHeight;

                $('.contextmenu').css('left', x);
                $('.contextmenu').css('top', y);
            };

            this.getCanvasXY = function (latLng) {
                var scale = Math.pow(2, this.map.getZoom());
                var nw = new google.maps.LatLng(
                    this.map.getBounds().getNorthEast().lat(),
                    this.map.getBounds().getSouthWest().lng()
                );
                var worldCoordinateNW = this.map.getProjection().fromLatLngToPoint(nw);
                var worldCoordinate = this.map.getProjection().fromLatLngToPoint(latLng);
                var latLngOffset = new google.maps.Point(
                    Math.floor((worldCoordinate.x - worldCoordinateNW.x) * scale),
                    Math.floor((worldCoordinate.y - worldCoordinateNW.y) * scale)
                );
                return latLngOffset;
            }
        }
    }]);
});