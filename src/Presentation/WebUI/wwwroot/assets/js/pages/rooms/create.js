"use strict";
// Class definition
var KTWizard3 = function () {
    // Base elements
    var _wizardEl;
    var _formEl;
    var _wizardObj;
    // Private functions
    var _initWizard = function () {
        // Initialize form wizard
        _wizardObj = new KTWizard(_wizardEl, {
            startStep: 1, // initial active step number
            clickableSteps: true  // allow step clicking
        });
        // Validation before going to next page
        _wizardObj.on('change', function (wizard) {
            if (wizard.getStep() > wizard.getNewStep()) {
                return; // Skip if stepped back
            }
            wizard.goTo(wizard.getNewStep());
            KTUtil.scrollTop();
            return false;  // Do not change wizard step, further action will be handled by he validator
        });
        // Changed event
        _wizardObj.on('changed', function (wizard) {
            KTUtil.scrollTop();
        });
        // Submit event
        _wizardObj.on('submit', function (wizard) {
            _formEl.submit(); // submit form
        });
    }
    return {
        // public functions
        init: function () {
            _wizardEl = KTUtil.getById('kt_wizard_v3');
            _formEl = KTUtil.getById('kt_form');
            _initWizard();
        }
    };
}();
jQuery(document).ready(function () {
    KTWizard3.init();
});

$('#CategoryId').select2({
    placeholder: "Select a value",
});
$.ajax({
    url: "/rooms/categories",
    type: 'GET',
    success: function (res) {
        res.forEach(function (data, index, arr) {
            var newOption = new Option(data.name, data.id, false, false);
            $("#CategoryId").append(newOption);
        });
        $("#CategoryId").trigger('change');
    }
});

function template(strings, ...keys) {
    return (function (...values) {
        let dict = values[values.length - 1] || {};
        let result = [strings[0]];
        keys.forEach(function (key, i) {
            let value = Number.isInteger(key) ? values[key] : dict[key];
            result.push(value, strings[i + 1]);
        });
        return result.join('');
    });
}
var amenitieAndRequirementOption = template` <div class="col-lg-4">
        <label class="option">
            <span class="option-control">
                <span class="checkbox checkbox-outline checkbox-success">
                    <input type="checkbox" name="${'inputName'}" value="${'id'}" checked="checked" />
                    <span></span>
                </span>
            </span>
            <span class="option-label">
                <span class="option-head">
                    <span class="option-title">
                        ${'title'}
                    </span>
                </span>
                <span class="option-body">
                    ${'decription'}
                </span>
            </span>
        </label >
    </div >`;

$.ajax({
    url: "/rooms/amenities",
    type: 'GET',
    success: function (res) {
        res.forEach(function (data, index, arr) {
            var newOption = amenitieAndRequirementOption({
                inputName: "AmenitieIds",
                id: data.id,
                title: data.name,
                decription: ''
            });

            $("#Amenities").append(newOption);
        });
        $("#Amenities").trigger('change');
    }
});

$.ajax({
    url: "/rooms/requirements",
    type: 'GET',
    success: function (res) {
        res.forEach(function (data, index, arr) {
            var newOption = amenitieAndRequirementOption({
                inputName: "RequirementIds",
                id: data.id,
                title: data.name,
                decription: ''
            });

            $("#Requirements").append(newOption);
        });
        $("#Requirements").trigger('change');
    }
});