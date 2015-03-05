define(['./module'], function(directives) {
    'use strict';

    directives.directive('map', [
        function() {
            var _scope, _element, _map, _control, _eventHandler;

            return {
                scope: {
                    options: '=',
                    map: '=',
                    control: '='
                },
                restrict: 'E',
                template: '<div></div>',
                replace: true,
                link: function(scope, element) {
                    _scope = scope;
                    _element = element;

                    _control = scope.control || {};
                    $.extend(_control, {
                        setContextMenu: setContextMenu
                    });

                    scope.options = scope.options || {};
                    scope.map = _map = new google.maps.Map(element.get(0), scope.options);
                    _eventHandler = new MapEventHandler(_map, _element);
                    _eventHandler.subscribe(scope.map);
                }
            };

            function setContextMenu(param) {
                _eventHandler.contextMenu = new MapContextMenu(_map, _element, param);
            }

            function MapEventHandler(map, elem) {
                this.map = map;
                this.element = elem;
                this.contextMenu = null;
                var self = this;

                this.subscribe = function() {
                    google.maps.event.addListener(this.map, 'click', onMapClick);
                    google.maps.event.addListener(this.map, 'rightclick', onMapRightClick);
                    google.maps.event.addListener(this.map, 'zoom_changed', onMapZoomChanged);
                    google.maps.event.addListener(this.map, 'dragstart', onMapDragStart);
                };

                function removeContextMenu() {
                    if (self.contextMenu) {
                        self.contextMenu.remove();
                    }
                }

                function onMapClick(e) {
                    removeContextMenu();
                }

                function onMapRightClick(e) {
                    if (self.contextMenu) {
                        self.contextMenu.create(e.latLng);
                    }
                }

                function onMapZoomChanged(e) {
                    removeContextMenu();
                }

                function onMapDragStart(e) {
                    removeContextMenu();
                }
            }

            function MapContextMenu(map, elem, itemsParam) {
                var self = this;
                this.map = map;
                this.element = elem;
                this.itemsParam = itemsParam;

                var className = 'mapcontextmenu';
                var dotClassName = '.' + className;


                this.create = function(latLng) {
                    this.remove();
                    var contextmenuDir = $('<div>').addClass(className).addClass('dropdown open');

                    var ul = $('<ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu1"></ul>');

                    var items = [];
                    if (Array.isArray(this.itemsParam)) {
                        items = this.itemsParam;
                    }
                    else if ($.isFunction(this.itemsParam)) {
                        items = this.itemsParam();
                    }

                    items.forEach(function(item) {
                        if (item.hasOwnProperty('divider')) {
                            ul.append('<li class="divider"></li>');
                        } else {
                            var a = $('<a role="menuitem" tabindex="-1" href="#">' + item.text + '</a>').click(function(e) {
                                item.handler(latLng);
                                self.remove();
                                e.preventDefault();
                            });
                            var li = $('<li role="presentation"></li>').append(a);
                            ul.append(li);
                        }
                    });
                    
                    contextmenuDir.append(ul);

                    $(this.map.getDiv()).append(contextmenuDir);

                    this.setMenuXY(latLng);
                };

                this.remove = function() {
                    $(dotClassName).remove();
                };



                // private
                this.setMenuXY = function(latLng) {
                    var mapWidth = this.element.width();
                    var mapHeight = this.element.height();
                    var menuSel = dotClassName + ' .dropdown-menu';
                    var menuWidth = $(menuSel).width();
                    var menuHeight = $(menuSel).height();
                    var clickedPosition = this.getCanvasXY(latLng);
                    var x = clickedPosition.x;
                    var y = clickedPosition.y;

                    if ((mapWidth - x) < menuWidth)
                        x = x - menuWidth;
                    if ((mapHeight - y) < menuHeight)
                        y = y - menuHeight;

                    $(menuSel).css('left', x);
                    $(menuSel).css('top', y);
                };

                this.getCanvasXY = function(latLng) {
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
                };
            }
        }
    ]);
});