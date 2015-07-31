define(['./module'], function (directives) {
    'use strict';
    directives.directive('settingsNameEditor', [
        function () {
            return {
                replace: true,
                restrict: 'E',
                require: '^settingsListItem',
                templateUrl: '/Scripts/app/partials/settings-name-editor.html',
                scope: {
                    itemData: '=',
                    saveChanges: '&onSave',
                    cancel: '&onCancel',
                    label: '@'
                },
            };
        }
    ]);
});