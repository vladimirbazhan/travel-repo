define(['./module'], function(directives) {
    'use strict';
    directives.directive('draggableItem', ['DragNDrop',
        function (DragNDrop) {
            return {
                restrict: 'A',

                // necessary parameters are passed by attributes. 
                // Required attributes: 
                //      draggableItemGroups: groups in DragNDrop service which will be notified about element drag events
                //      draggableItemData: data that will be put to dragging item and retrieved by drop item
                link: function (scope, element, attrs) {
                    var linkData = {
                        scope: scope,
                        attrs: attrs,
                        element: element
                    }
                    element.data('linkData', linkData);
                    init(element);
                    console.info(getAttr(element, 'data'));
                }
            };

            function init(element) {
                element.attr('draggable', 'true');
                element.addClass('draggable');

                element.bind('dragstart', onDragStart);
                element.bind('dragend', onDragEnd);
            }

            function getAttr(element, attr) {
                attr = attr[0].toUpperCase() + attr.toLowerCase().substr(1);
                return unescape(element.data('linkData').attrs['draggableItem' + attr]);
            }

            // event handlers
            function onDragStart(e) {
                $(this).addClass('dragging');
                $(this).css('opacity', '0.4');

                e.originalEvent.dataTransfer.setData('text', getAttr($(this), 'data'));
            }

            function onDragEnd(e) {
                $(this).removeClass('dragging');
                $(this).css('opacity', '1');
                //$('.dragging-over').removeClass('dragging-over');
            }
        }
    ]);
});