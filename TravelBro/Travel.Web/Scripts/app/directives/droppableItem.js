define(['./module'], function(directives) {
    'use strict';
    directives.directive('droppableItem', [function () {
            return {
                scope: {
                    droppableItemData: '=',
                    onDrop: '&'
                },
                restrict: 'A',
                link: function (scope, element, attrs) {
                    init.call(scope, element);
                }
            };

            function init(element) {
                element.addClass('droppable');
                element.bind('dragenter', $.proxy(onDragEnter, this));
                element.bind('dragleave', $.proxy(onDragLeave, this));
                element.bind('dragover', $.proxy(onDragOver, this));
                element.bind('drop', $.proxy(onDrop, this));
            }

            // event handlers
            function onDragOver(e) {
                if (e.preventDefault) {
                    e.preventDefault(); // Necessary. Allows us to drop.
                }
                return false;
            }

            function onDragEnter(e) {
                e.target.dragenterCount = e.target.dragenterCount || 0;
                e.target.dragenterCount++;

                $('.dragging-over').removeClass('dragging-over');
                $(e.target).addClass('dragging-over');
            }

            function onDragLeave(e) {
                if (--e.target.dragenterCount == 0) {
                    $(e.target).removeClass('dragging-over');
                }
            }

            function onDrop(e) {
                if (e.stopPropagation) {
                    e.stopPropagation(); // stops the browser from redirecting.
                }

                var dto = e.originalEvent.dataTransfer.getData('dragData');
                var dragData = JSON.parse(dto);

                var dropData = this.droppableItemData;

                this.onDrop({
                    dragData: dragData,
                    dropData: dropData
                });

                return false;
            }
        }
    ]);
});