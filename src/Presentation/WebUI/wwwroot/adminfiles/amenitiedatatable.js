"use strict";
// Class definition
var pathname = window.location.pathname;
pathname = pathname.split("/");
var lang = pathname[1];

//var KTDatatableRemoteAjaxDemo = function () {
//    // Private functions
//    var pathname = window.location.pathname;
//    pathname = pathname.split("/");
//    var lang = pathname[1];

//    // basic demo
//    var demo = function () {

//        var datatable = $('#kt_datatable').KTDatatable({
//            // datasource definition
//            data: {
//                type: 'remote',
//                source: {
//                    read: {
//                        url: '/' + lang + '/admin/Amenities/Datatable',
//                        // sample custom headers
//                        // headers: {'x-my-custom-header': 'some value', 'x-test-header': 'the value'},
//                        map: function (raw) {
//                            // sample data mapping
//                            var dataSet = raw;
//                            if (typeof raw.data !== 'undefined') {
//                                dataSet = raw.data;
//                            }
//                            return dataSet;
//                        },
//                    },
//                },
//                pageSize: 10,
//                serverPaging: true,
//                serverFiltering: true,
//                serverSorting: true,
//            },

//            // layout definition
//            layout: {
//                scroll: false,
//                footer: false,
//            },

//            // column sorting
//            sortable: true,

//            pagination: true,

//            search: {
//                input: $('#kt_datatable_search_query'),
//                key: 'generalSearch'
//            },

//            // columns definition
//            columns: [
//                {
//                    field: 'Icon',
//                    title: 'Icon',
//                    template: function (row) {
//                        return '<span class="svg-icon svg-icon-primary svg-icon-2x">' + row.icon + '</span>';
//                    },
//                },
//                {
//                    field: 'name',
//                    title: 'Name',
//                },
//                {
//                    field: 'Translations',
//                    title: 'Translations',
//                    template: function (row) {
//                        var langstr = '';
//                        languages.forEach(lang => {
//                            var exists = false;
//                            row.translations.forEach(element => {
//                                if (element.langCode == lang) {
//                                    langstr += '<a href="' + lang + '/admin/amenities/EditTranslation/' + element.langCode + '/' + row.id + '" class="btn btn-sm btn-clean btn-icon mr-2" title="' + element.langCode + '">\
//                                    <i class="la la-edit"></i>\
//                                    </a>';
//                                    exists = true;
//                                }
//                            });
//                            if (!exists) {
//                                langstr += '<a href="' + lang + '/admin/amenities/EditTranslation/' + lang + '/' + row.id + '" class="btn btn-sm btn-clean btn-icon mr-2" title="' + lang + '">\
//                                    <i class="la la-plus"></i>\
//                                    </a>';
//                            }
//                        });

//                        return langstr;
//                    },
//                },
//                {
//                    field: 'updatedAt',
//                    title: 'Update Date',
//                    type: 'datetime',
//                    template: function (row) {
//                        return new Date(row.updatedAt).toLocaleDateString();
//                    }
//                },
//                {
//                    field: 'Actions',
//                    title: 'Actions',
//                    sortable: false,
//                    width: 125,
//                    overflow: 'visible',
//                    autoHide: false,
//                    template: function (row) {
//                        return '\
//                        <a href="'+ lang + '/admin/amenities/editicon/' + row.id + '" class="btn btn-sm btn-clean btn-icon mr-2" title="Edit details">\
//                            <span class="svg-icon svg-icon-md">\
//                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
//                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
//                                        <rect x="0" y="0" width="24" height="24"/>\
//                                        <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero"\ transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>\
//                                        <rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>\
//                                    </g>\
//                                </svg>\
//                            </span>\
//                        </a>\
//                        <a id = "'+ row.id + '"class="btn btn-sm btn-clean btn-icon deleteAmenitieIcon"  title="Delete">\
//                            <span class="svg-icon svg-icon-md">\
//                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
//                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
//                                        <rect x="0" y="0" width="24" height="24"/>\
//                                        <path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero"/>\
//                                        <path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3"/>\
//                                    </g>\
//                                </svg>\
//                            </span>\
//                        </a>\
//                    ';
//                    },
//                }],
//            //from _layout.cshtml
//            translate: datatableTranslation,

//        });

//        $('#kt_datatable_search_language').on('change', function () {
//            datatable.search($(this).val().toLowerCase(), 'Language');
//        });

//        $('#kt_datatable_search_type').on('change', function () {
//            datatable.search($(this).val().toLowerCase(), 'Type');
//        });

//        $('#kt_datatable_search_language, #kt_datatable_search_type').selectpicker();

//        datatable.on('click', '.deleteAmenitieIcon', function () {

//            Swal.fire({
//                title: 'Are you sure?',
//                text: "This can't be returned.",
//                type: 'warning',
//                showCancelButton: true,
//                confirmButtonColor: '#3085d6',
//                cancelButtonColor: '#d33',
//                confirmButtonText: 'Yeah i am sure!'
//            }).then((result) => {
//                if (result.value) {
//                    var delButton = $(this);
//                    var id = delButton.attr("id");
//                    $.ajax({
//                        url: '/' + lang + '/admin/amenities/delete/' + id,
//                        type: 'POST',
//                        success: function (result, status, xhr) {
//                            datatable.row(delButton.closest('tr')).remove().draw();
//                        },
//                        error: function (resp, status, err) {
//                            if (resp.status == 200) {
//                                console.log("success");
//                            }
//                            Swal.fire({
//                                title: 'Not deleted',
//                                text: resp.responseJSON.message,
//                                type: 'danger',
//                                showCancelButton: true,
//                            });
//                        }
//                    });
//                }
//            });
//        });
//    };


//    return {
//        // public functions
//        init: function () {
//            demo();
//        },
//    };
//}();

//jQuery(document).ready(function () {
//    KTDatatableRemoteAjaxDemo.init();
//});

// Class definition
var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#kt_amenities_table").DataTable({
            searchDelay: 500,
            processing: true,
            serverSide: true,
            stateSave: true,
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            ajax: {
                url: '/' + lang + "/admin/amenities/datatable",
            },
            columns: [
                {
                    data: 'id',
                    name: 'Id',
                },
                {
                    data: 'icon',
                    name: 'Icon',
                },
                {
                    data: 'name',
                    name: 'Name',
                },
                {
                    data: 'translations',
                    name: 'Translations',
                },
                {
                    data: 'updatedAt',
                    name: 'UpdatedAt',
                },
                { data: null },
            ],
            order: [[1, 'asc']],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function (data) {
                        return `<div class="form-check form-check-sm form-check-custom form-check-solid">
                                                                                                                                                                                                <input class="form-check-input" type="checkbox" id="${data}" />
                                                                                                                                                                                        </div>`;
                    }
                },
                {
                    targets: 1,
                    orderable: false,
                    render: function (data) {
                        return `<img src="${data}" class="w-35px me-3">`;
                    }
                },
                {
                    targets: 3,
                    orderable: false,
                    render: function (data, type, row) {
                        var langstr = '';
                        languages.forEach(lang => {
                            var exists = false;
                            console.log(row);
                            row.translations.forEach(element => {
                                console.log(element);
                                if (element.langCode == lang) {
                                    langstr += '<a href="' + lang + '/admin/amenities/EditTranslation/' + element.langCode + '/' + row.id + '" class="btn btn-sm btn-clean btn-icon mr-2" title="' + element.langCode + '">\
                                    <i class="la la-edit"></i>\
                                    </a>';
                                    exists = true;
                                }
                            });
                            if (!exists) {
                                langstr += '<a href="' + lang + '/admin/amenities/EditTranslation/' + lang + '/' + row.id + '" class="btn btn-sm btn-clean btn-icon mr-2" title="' + lang + '">\
                                    <i class="la la-plus"></i>\
                                    </a>';
                            }
                        });

                        return langstr;
                    },
                },
                {
                    targets: 4,
                    render: function (data) {
                        return new Date(data).toLocaleDateString();
                    }
                },
                {
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        return `
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        <a class="btn btn-light btn-active-light-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            Actions
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            <span class="svg-icon svg-icon-5 m-0">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        <polygon points="0 0 24 0 24 24 0 24"></polygon>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        <path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    </g>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                </svg>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            </span>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        </a>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        <!--begin::Menu-->
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            <!--begin::Menu item-->
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            <div class="menu-item px-3">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                <a href="/admin/amenities/edit/${row.id}" class="menu-link px-3" data-kt-amenitie-table-filter="edit_row">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    Edit
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                </a>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            </div>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            <!--end::Menu item-->
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            <!--begin::Menu item-->
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            <div class="menu-item px-3">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                <a class="menu-link px-3" data-kt-amenitie-table-filter="delete_row">
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    Delete
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                </a>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            </div>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            <!--end::Menu item-->
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        </div>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        <!--end::Menu-->
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    `;
                    },
                },
            ],
            // Add data-filter attribute
            createdRow: function (row, data, dataIndex) {
                $(row).find('td:eq(4)').attr('data-filter', data.CreatedAt);
            }
        });

        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {
            initToggleToolbar();
            toggleToolbars();
            handleDeleteRows();
            KTMenu.createInstances();
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = function () {
        const filterSearch = document.querySelector('[data-kt-amenitie-table-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            dt.search(e.target.value).draw();
        });
    }

    // Delete amenitie
    var handleDeleteRows = () => {
        // Select all delete buttons
        const deleteButtons = document.querySelectorAll('[data-kt-amenitie-table-filter="delete_row"]');

        deleteButtons.forEach(d => {
            // Delete button on click
            d.addEventListener('click', function (e) {
                e.preventDefault();

                // Select parent row
                const parent = e.target.closest('tr');
                const ids = Array.from(parent.querySelectorAll('[type="checkbox"]')).map(x => x.id);


                const amenitieName = parent.querySelectorAll('td')[1].innerText;

                Swal.fire({
                    text: "Are you sure you want to delete selected Amenities?",
                    icon: "warning",
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: "Yes, delete!",
                    cancelButtonText: "No, cancel",
                    customClass: {
                        confirmButton: "btn fw-bold btn-danger",
                        cancelButton: "btn fw-bold btn-active-light-primary"
                    }
                }).then(function (result) {
                    if (result.value) {
                        deleteAmenities(ids);
                    }
                });
            })
        });
    }

    // Init toggle toolbar
    var initToggleToolbar = function () {
        // Toggle selected action toolbar
        // Select all checkboxes
        const container = document.querySelector('#kt_amenities_table');
        const checkboxes = container.querySelectorAll('[type="checkbox"]');

        // Select elements
        const deleteSelected = document.querySelector('[data-kt-amenitie-table-select="delete_selected"]');

        // Toggle delete selected toolbar
        checkboxes.forEach(c => {
            // Checkbox on click event
            c.addEventListener('click', function () {
                setTimeout(function () {
                    toggleToolbars();
                }, 50);
            });
        });

        // Deleted selected rows
        deleteSelected.addEventListener('click', function () {

            var selectedCheckboxes = container.querySelectorAll('[type="checkbox"]');

            selectedCheckboxes[0].checked = false;

            var ids = Array.from(selectedCheckboxes)
                .filter(function (el) {
                    return el.checked && el != null;
                })
                .map(x => x.id);


            Swal.fire({
                text: "Are you sure you want to delete selected items?",
                icon: "warning",
                showCancelButton: true,
                buttonsStyling: false,
                showLoaderOnConfirm: true,
                confirmButtonText: "Yes, delete!",
                cancelButtonText: "No, cancel",
                customClass: {
                    confirmButton: "btn fw-bold btn-danger",
                    cancelButton: "btn fw-bold btn-active-light-primary"
                },
            }).then(function (result) {
                if (result.value) {
                    deleteAmenities(ids);
                } else if (result.dismiss === 'cancel') {
                    Swal.fire({
                        text: "Selected Amenities were not deleted.",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn fw-bold btn-primary",
                        }
                    });
                }
            });
        });
    }

    function deleteAmenities(ids) {
        // Simulate delete request -- for demo purpose only
        Swal.fire({
            text: "Deleting selected Amenities",
            icon: "info",
            buttonsStyling: false,
            showConfirmButton: false,
            timer: 2000
        });
        $.ajax({
            url: "/admin/amenities/delete",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ "ids": ids }),
            success: function (res) {
                Swal.fire({
                    text: "You have deleted all selected Amenities!.",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn fw-bold btn-primary",
                    }
                }).then(function () {
                    // delete row data from server and re-draw
                    dt.draw();
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire({
                    text: "Something went wrong",
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Ok!",
                    customClass: {
                        confirmButton: "btn fw-bold btn-primary",
                    }
                })
            }
        });
    }
    // Toggle toolbars
    var toggleToolbars = function () {
        // Define variables
        const container = document.querySelector('#kt_amenities_table');
        const toolbarBase = document.querySelector('[data-kt-amenitie-table-toolbar="base"]');
        const toolbarSelected = document.querySelector('[data-kt-amenitie-table-toolbar="selected"]');
        const selectedCount = document.querySelector('[data-kt-amenitie-table-select="selected_count"]');

        // Select refreshed checkbox DOM elements
        const allCheckboxes = container.querySelectorAll('tbody [type="checkbox"]');

        // Detect checkboxes state & count
        let checkedState = false;
        let count = 0;

        // Count checked boxes
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
        });

        // Toggle toolbars
        if (checkedState) {
            selectedCount.innerHTML = count;
            toolbarBase.classList.add('d-none');
            toolbarSelected.classList.remove('d-none');
        } else {
            toolbarBase.classList.remove('d-none');
            toolbarSelected.classList.add('d-none');
        }
    }

    // Public methods
    return {
        init: function () {
            initDatatable();
            handleSearchDatatable();
            initToggleToolbar();
            handleDeleteRows();
        }
    }
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTDatatablesServerSide.init();
});