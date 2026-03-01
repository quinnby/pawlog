function showAddLicensingRecordModal() {
    $.get('/Vehicle/GetAddLicensingRecordPartialView', function (data) {
        if (data) {
            $("#licensingRecordModalContent").html(data);
            initDatePicker($('#licensingIssueDate'));
            initDatePicker($('#licensingExpiryDate'));
            initTagSelector($("#licensingTag"));
            $('#licensingRecordModal').modal('show');
        }
    });
}

function showEditLicensingRecordModal(licensingRecordId, nocache) {
    $.get(`/Vehicle/GetLicensingRecordForEditById?licensingRecordId=${licensingRecordId}`, function (data) {
        if (data) {
            $("#licensingRecordModalContent").html(data);
            initDatePicker($('#licensingIssueDate'));
            initDatePicker($('#licensingExpiryDate'));
            initTagSelector($("#licensingTag"));
            $('#licensingRecordModal').modal('show');
            bindModalInputChanges('licensingRecordModal');
            $('#licensingRecordModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                if (getGlobalConfig().useMarkDown) {
                    toggleMarkDownOverlay("licensingNotes");
                }
            });
        }
    });
}

function hideAddLicensingRecordModal() {
    $('#licensingRecordModal').modal('hide');
}

function deleteLicensingRecord(licensingRecordId) {
    $("#workAroundInput").show();
    confirmDelete("Deleted Licensing Records cannot be restored.", (result) => {
        if (result.isConfirmed) {
            $.post(`/Vehicle/DeleteLicensingRecordById?licensingRecordId=${licensingRecordId}`, function (data) {
                if (data.success) {
                    hideAddLicensingRecordModal();
                    successToast("Licensing Record Deleted");
                    var vehicleId = GetVehicleId().vehicleId;
                    getVehicleLicensingRecords(vehicleId);
                } else {
                    errorToast(data.message);
                    $("#workAroundInput").hide();
                }
            });
        } else {
            $("#workAroundInput").hide();
        }
    });
}

function saveLicensingRecordToVehicle(isEdit) {
    var formValues = getAndValidateLicensingRecordValues();
    if (formValues.hasError) {
        errorToast("Please check the form data");
        return;
    }
    $.post('/Vehicle/SaveLicensingRecordToVehicleId', formValues, function (data) {
        if (data.success) {
            successToast(isEdit ? "Licensing Record Updated" : "Licensing Record Added");
            hideAddLicensingRecordModal();
            saveScrollPosition();
            getVehicleLicensingRecords(formValues.vehicleId);
        } else {
            errorToast(data.message);
        }
    });
}

function getAndValidateLicensingRecordValues() {
    var date                   = $("#licensingIssueDate").val();
    var licenseNumber          = $("#licensingNumber").val();
    var issuer                 = $("#licensingIssuer").val();
    var expiryDate             = $("#licensingExpiryDate").val();
    var cost                   = $("#licensingCost").val();
    var notes                  = $("#licensingNotes").val();
    var renewalReminderEnabled = $("#licensingRenewalReminderEnabled").is(":checked");
    var tags                   = $("#licensingTag").val();
    var vehicleId              = GetVehicleId().vehicleId;
    var recordId               = getLicensingRecordModelData().id;
    var extraFields            = getAndValidateExtraFields();

    var hasError = false;
    if (extraFields.hasError) { hasError = true; }

    if (date.trim() == '') {
        hasError = true;
        $("#licensingIssueDate").addClass("is-invalid");
    } else {
        $("#licensingIssueDate").removeClass("is-invalid");
    }
    if (licenseNumber.trim() == '') {
        hasError = true;
        $("#licensingNumber").addClass("is-invalid");
    } else {
        $("#licensingNumber").removeClass("is-invalid");
    }
    if (cost.trim() != '' && !isValidMoney(cost)) {
        hasError = true;
        $("#licensingCost").addClass("is-invalid");
    } else {
        $("#licensingCost").removeClass("is-invalid");
    }

    return {
        hasError: hasError,
        id: recordId,
        vehicleId: vehicleId,
        date: date,
        licenseNumber: licenseNumber,
        issuer: issuer,
        expiryDate: expiryDate,
        cost: cost == '' ? 0 : cost,
        notes: notes,
        renewalReminderEnabled: renewalReminderEnabled,
        tags: tags,
        extraFields: extraFields.extraFields
    };
}
