define(['./module'], function (directives) {
    'use strict';
    directives.directive('settingsListItem', ['Alerts', '$timeout', 
        function (Alerts, $timeout) {
            return {
                scope: {
                    name: '@',
                    content: '@',
                    itemData: '=',
                    editor: '@',
                    isEditMode: '=?',
                    onSave: '&',
                    onCancel: '&'
                },
                restrict: 'E',
                templateUrl: '/Scripts/app/partials/settingsListItem.html',
                controller: SettingsListItemCtrl,
                controllerAs: 'settingsListItemVm',
                bindToController: true,
            };

            function SettingsListItemCtrl($scope) {
                $scope.isEditMode = false;
                $scope.switchToEditMode = function (toEditMode) {
                    $scope.isEditMode = toEditMode;
                    if (toEditMode && !$scope.editor) {
                        $timeout(function () {
                            $scope.switchToEditMode(false);
                        }, 1000);
                    }
                }
                this.save = function (params) {
                    $scope.onSave({
                        params: params,
                        callbacks: {
                            onsuccess: function() {
                                $scope.switchToEditMode(false);
                            },
                            onerror: function() {}
                        }
                    });
                }
                this.cancel = function () {
                    $scope.onCancel();
                    $scope.switchToEditMode(false);
                }
            }
        }
    ]);
});