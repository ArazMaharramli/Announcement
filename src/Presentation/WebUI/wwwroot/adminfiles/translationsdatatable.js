"use strict";
// Class definition


var KTDatatableRemoteAjaxDemo = function () {
    // Private functions
    var pathname = window.location.pathname;
    pathname = pathname.split("/");
    var lang = pathname[1];

    // basic demo
    var demo = function () {

        var datatable = $('#kt_datatable').KTDatatable({
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        url: '/' + lang + '/admin/Translations/Datatable',
                        // sample custom headers
                        // headers: {'x-my-custom-header': 'some value', 'x-test-header': 'the value'},
                        map: function (raw) {
                            // sample data mapping
                            var dataSet = raw;
                            if (typeof raw.data !== 'undefined') {
                                dataSet = raw.data;
                            }
                            return dataSet;
                        },
                    },
                },
                pageSize: 10,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },

            // layout definition
            layout: {
                scroll: false,
                footer: false,
            },

            // column sorting
            sortable: true,

            pagination: true,

            search: {
                input: $('#kt_datatable_search_query'),
                key: 'generalSearch'
            },

            // columns definition
            columns: [
                {
                    field: 'key',
                    title: 'Key',
                },
                {
                    field: 'text',
                    title: 'Text',
                },
                {
                    field: 'Translations',
                    title: 'Translations',
                    template: function (row) {
                        var langstr = '';
                        languages.forEach(lang => {
                            var exists = false;
                            row.translations.forEach(element => {
                                console.log(element);
                                if (element.langCode == lang) {
                                    langstr += '<a href="' + lang + '/admin/Translations/EditTranslation/' + element.langCode + '/' + $('#kt_datatable_search_resources').val().toLowerCase() + '/' + row.key + '" class="btn btn-sm btn-clean btn-icon mr-2" title="' + element.langCode + '">\
                                    <i class="la la-edit"></i>\
                                    </a>';
                                    exists = true;
                                }
                            });
                            if (!exists) {
                                langstr += '<a href="' + lang + '/admin/Translations/addTranslation/' + lang + '/' + $('#kt_datatable_search_resources').val().toLowerCase() + '/' + row.key + '" class="btn btn-sm btn-clean btn-icon mr-2" title="' + lang + '">\
                                    <i class="la la-plus"></i>\
                                    </a>';
                            }
                        });

                        return langstr;
                    },
                },
                {
                    field: 'updatedAt',
                    title: 'Update Date',
                    type: 'datetime',
                    template: function (row) {
                        return new Date(row.updatedAt).toLocaleDateString();
                    }
                },
            ],
            //from _layout.cshtml
            translate: datatableTranslation,

        });

        $('#kt_datatable_search_language').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Language');
        });

        $('#kt_datatable_search_resources').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'resource');
        });
        $('#kt_datatable_search_query').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'resource');
        });

        $('#kt_datatable_search_language, #kt_datatable_search_resources').selectpicker();
    };


    return {
        // public functions
        init: function () {
            demo();
        },
    };
}();

jQuery(document).ready(function () {
    KTDatatableRemoteAjaxDemo.init();
});
