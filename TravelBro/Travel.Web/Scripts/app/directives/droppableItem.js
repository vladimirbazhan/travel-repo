define(['./module'], function(directives) {
    'use strict';
    directives.directive('droppableItem', ['DragNDrop', 'Backend', 'Alerts', '$route',
        function (DragNDrop, Backend, Alerts, $route) {
            return {
                restrict: 'A',

                // necessary parameters are passed by attributes. 
                // Required attributes: 
                //      draggableItemGroups: groups in DragNDrop service which will be notified about element drag events
                //      draggableItemData: data that will be used when drop happened
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
                element.addClass('droppable');

                element.bind('dragenter', onDragEnter);
                element.bind('dragleave', onDragLeave);
                element.bind('dragover', onDragOver);
                element.bind('drop', onDrop);
            }

            function getAttr(element, attr) {
                attr = attr[0].toUpperCase() + attr.toLowerCase().substr(1);
                return unescape(element.data('linkData').attrs['droppableItem' + attr]);
            }

            // event handlers
            function onDragOver(e) {
                if (e.preventDefault) {
                    e.preventDefault(); // Necessary. Allows us to drop.
                }
                return false;
            }

            function onDragEnter(e) {
                this.dragenterCount = this.dragenterCount || 0;
                this.dragenterCount++;

                $('.dragging-over').removeClass('dragging-over');
                $(this).addClass('dragging-over');
            }

            function onDragLeave(e) {
                if (--this.dragenterCount == 0) {
                    $(e.target).removeClass('dragging-over');
                }
            }

            function onDrop(e) {
                if (e.stopPropagation) {
                    e.stopPropagation(); // stops the browser from redirecting.
                }

                var dtoData = e.originalEvent.dataTransfer.getData('text');
                var dragItem = JSON.parse(dtoData);
                var dropItem = JSON.parse(getAttr($(this), 'data'));
                
                dragItem.Order = dropItem.order;
                Backend.visits.update({ visitId: dragItem.Id }, dragItem, function () {
                    Alerts.add('info', 'Changes saved');
                    $route.reload();
                }, function (res) {
                    Alerts.add('danger', res.error_description);
                });

                return false;
            }

        }
    ]);
});