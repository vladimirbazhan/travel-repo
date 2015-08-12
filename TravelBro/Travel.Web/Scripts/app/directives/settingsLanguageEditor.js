define(['./module'], function (directives) {
    'use strict';
    directives.directive('settingsLanguageEditor', [
        function () {
            return {
                replace: true,
                restrict: 'E',
                require: '^settingsListItem',
                templateUrl: '/Scripts/app/partials/settings-language-editor.html',
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