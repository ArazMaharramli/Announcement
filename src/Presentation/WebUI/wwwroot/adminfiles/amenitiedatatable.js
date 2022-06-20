"use strict";
// Class definition
var pathname = window.location.pathname;
pathname = pathname.split("/");
var lang = pathname[1];
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
                        languages.forEach(locale => {
                            var exists = false;

                            row.translations.forEach(element => {

                                if (element.langCode === locale) {
                                    langstr += '<a href="/' + lang + '/admin/amenities/EditTranslation/' + element.langCode + '/' + row.id + '" class="btn btn-sm btn-clean btn-icon mr-2" title="' + element.langCode + '">\
                                    <i class="la la-edit"></i>\
                                    </a>';
                                    exists = true;
                                }
                            });
                            if (!exists) {
                                langstr += '<a href="/' + lang + '/admin/amenities/EditTranslation/' + lang + '/' + row.id + '" class="btn btn-sm btn-clean btn-icon mr-2" title="' + lang + '">\
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
                                        <a href="/${lang}/admin/amenities/edit/${row.id}" class="menu-link px-3" data-kt-amenitie-table-filter="edit_row">
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
            url: '/' + lang + "/admin/amenities/delete",
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