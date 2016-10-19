(function ($, portals, window, document) {
    portals.init = portals.init || function () {
        $(document).ready(function () {
            portals.dashboards.init();
        });
    };

    var dashboards = portals.dashboards = portals.dashboard || (function () {
        var initialize = function () {
            $(document).ready(function () {
                $(".toolbox-items").sortable({
                    connectWith: ".dashboard",
                    placeholder: "ui-state-highlight",
                    helper: function (e, li) {
                        this.copyHelper = li.clone().insertAfter(li);
                        $(this).data('copied', false);
                        return li.clone();
                    },
                    stop: function () {
                        var copied = $(this).data('copied');
                        if (!copied) {
                            this.copyHelper.remove();
                        }
                        this.copyHelper = null;
                    }
                });

                $(".dashboard").sortable({
                    connectWith: ".dashboard",
                    receive: function (e, ui) {
                        ui.item.removeClass('toolbox-item');
                        ui.item.addClass('dashboard-item');

                        ui.sender.data('copied', true);
                    }
                });

                $(".toolbox-items").disableSelection();

                $(document).on('click', '.dashboard-item .edit', function () {
                    var $item = $(this).closest('.dashboard-item');
                    var properties = $item.data('properties');
                    var $dialogElement = getDialogElement(properties);
                    var title = $item.data('item-title');

                    $($dialogElement).dialog({
                        dialogClass: 'property-dialog',
                        height: 'auto',
                        width: 400,
                        title: title,
                        modal: true,
                        buttons: {
                            "Apply": function () {
                                $(this).find('input').each(function () {
                                    var property = {
                                        Name: $(this).attr('name'),
                                        Value: $(this).val()
                                    };

                                    for (var i = properties.length - 1; i > -1; i--) {
                                        if (properties[i].Name === $(this).attr('name')) {
                                            properties[i].Value = $(this).val();
                                        }
                                    }
                                });

                                $item.data('properties', properties);
                                $(this).closest('.ui-dialog-content').dialog('close');
                            }
                        }
                    });
                });

                $(document).on('click', '.dashboard-item .remove', function () {
                    $(this).closest('.dashboard-item').remove();
                });

                $(".toolbox .controls #save")
                    .button()
                    .click(function (element) {
                        event.preventDefault();
                        var dashboardData = getDashboardData();
                        saveDashboardData(dashboardData);
                    });

                $(".toolbox .controls #cancel")
                    .button()
                    .click(function (element) {
                        event.preventDefault();
                        redirectToDashboard();
                    });

                var saveDashboardData = function (dashboardData) {
                    $.ajax({
                        type: "POST",
                        data: dashboardData,
                        url: "/portals/dashboards/save",
                        contentType: "application/json",
                        success: function () {
                            redirectToDashboard();
                        },
                    });
                };

                var redirectToDashboard = function () {
                    window.location = window.location.pathname.replace("configure", "").replace("//", "/");
                };

                var getDialogElement = function (properties) {
                    var $element = $("<div>").attr({
                        id: 'property-editor'
                    });

                    properties.forEach(function (property) {
                        var $container = $('<div class="property-container">');

                        var $label = $('<label>').attr({
                            'for': property.Name
                        });
                        $label.text(property.DisplayName);
                        var $input = $('<input>').attr({
                            'type': 'text',
                            'id': property.Name,
                            'name': property.Name,
                            'value': property.Value
                        });

                        $container.append($label);
                        $container.append($input);

                        $element.append($container);
                    });

                    return $element;
                };

                var getDashboardData = function () {
                    var dashboards = [];

                    $('.dashboard').each(function () {
                        $d = $(this);

                        var dashboard = { DashboardId: $d.data('dashboard-id'), DashboardItems: []};

                        $d.children('.dashboard-item').each(function () {
                            $i = $(this);

                            var item = { ControllerType: $i.data('controller-type'), Properties: $i.data('properties') };

                            dashboard.DashboardItems.push(item);
                        });

                        dashboards.push(dashboard);
                    });

                    return JSON.stringify(dashboards);
                };
            });
        };

        return {
            init: initialize,
        }
    }());

}(jQuery, window.portals = window.portals || {}, window, document));

portals.init();