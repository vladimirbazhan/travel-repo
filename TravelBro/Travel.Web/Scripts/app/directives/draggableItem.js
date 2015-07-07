define(['./module'], function(directives) {
    'use strict';
    directives.directive('draggableItem', [function () {
            return {
                restrict: 'A',
                scope: {
                    draggableItemData : '='
                },
                link: function (scope, element, attrs) {
                    init.call(scope, element);
                }
            };

            function init(element) {
                element.attr('draggable', 'true');
                element.addClass('draggable');

                element.bind('dragstart', $.proxy(onDragStart, this));
                element.bind('dragend', $.proxy(onDragEnd, this));
            }

            // event handlers
            function onDragStart(e) {
                $(e.target).addClass('dragging');
                $(e.target).css('opacity', '0.4');
                e.originalEvent.dataTransfer.setData('dragData', JSON.stringify(this.draggableItemData));
            }

            function onDragEnd(e) {
                $(this).removeClass('dragging');
                $(this).css('opacity', '1');
            }
        }
    ]);
});