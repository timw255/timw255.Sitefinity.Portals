var customModule = angular.module('customModule', ['pageEditorServices', 'breadcrumb']);

angular.module('designer').requires.push('customModule');

customModule.controller('DefaultViewCtrl', ['$scope', 'propertyService',
    function ($scope, propertyService) {
        $scope.feedback.showLoadingIndicator = true;

        propertyService
        .get()
        .then(function (data) {
            if (data) {
                $scope.properties = propertyService.toAssociativeArray(data.Items);

                if ($scope.properties.DashboardId.PropertyValue === "00000000-0000-0000-0000-000000000000") {
                    $scope.properties.DashboardId.PropertyValue = kendo.guid();
                }
            }
        },
        function (data) {
            $scope.feedback.showError = true;
            if (data)
            {
                $scope.feedback.errorMessage = data.Detail;
            }
        })
        .finally(function () {
            $scope.feedback.showLoadingIndicator = false;
        });
    }
]);
