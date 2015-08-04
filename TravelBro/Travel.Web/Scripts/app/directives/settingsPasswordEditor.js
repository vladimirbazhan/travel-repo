define(['./module'], function (directives) {
    'use strict';
    directives.directive('settingsPasswordEditor', [function () {
            return {
                replace: true,
                restrict: 'E',
                require: '^settingsListItem',
                templateUrl: '/Scripts/app/partials/settings-password-editor.html',
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