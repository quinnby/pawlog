function showAddVaccinationRecordModal() {
    $.get('/Vehicle/GetAddVaccinationRecordPartialView', function (data) {
        if (data) {
            $("#vaccinationRecordModalContent").html(data);
            initDatePicker($('#vaccinationDate'));
            initDatePicker($('#vaccinationNextDueDate'), false, true);
            initTagSelector($("#vaccinationTag"));
            $('#vaccinationRecordModal').modal('show');
        }
    });
}

function showEditVaccinationRecordModal(vaccinationRecordId, nocache) {
    $.get(`/Vehicle/GetVaccinationRecordForEditById?vaccinationRecordId=${vaccinationRecordId}`, function (data) {
        if (data) {
            $("#vaccinationRecordModalContent").html(data);
            initDatePicker($('#vaccinationDate'));
            initDatePicker($('#vaccinationNextDueDate'), false, true);
            initTagSelector($("#vaccinationTag"));
            $('#vaccinationRecordModal').modal('show');
            bindModalInputChanges('vaccinationRecordModal');
            $('#vaccinationRecordModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                if (getGlobalConfig().useMarkDown) {
                    toggleMarkDownOverlay("vaccinationNotes");
                }
            });
        }
    });
}

function hideAddVaccinationRecordModal() {
    $('#vaccinationRecordModal').modal('hide');
}

function deleteVaccinationRecord(vaccinationRecordId) {
    $("#workAroundInput").show();
    confirmDelete("Deleted Vaccination Records cannot be restored.", (result) => {
        if (result.isConfirmed) {
            $.post(`/Vehicle/DeleteVaccinationRecordById?vaccinationRecordId=${vaccinationRecordId}`, function (data) {
                if (data.success) {
                    hideAddVaccinationRecordModal();
                    successToast("Vaccination Record Deleted");
                    var vehicleId = GetVehicleId().vehicleId;
                    getVehicleVaccinationRecords(vehicleId);
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

function saveVaccinationRecordToVehicle(isEdit) {
    var formValues = getAndValidateVaccinationRecordValues();
    if (formValues.hasError) {
        errorToast("Please check the form data");
        return;
    }
    $.post('/Vehicle/SaveVaccinationRecordToVehicleId', formValues, function (data) {
        if (data.success) {
            successToast(isEdit ? "Vaccination Record Updated" : "Vaccination Record Added");
            hideAddVaccinationRecordModal();
            saveScrollPosition();
            getVehicleVaccinationRecords(formValues.vehicleId);
        } else {
            errorToast(data.message);
        }
    });
}

function getAndValidateVaccinationRecordValues() {
    var date           = $("#vaccinationDate").val();
    var name           = $("#vaccinationName").val();
    var nextDueDate    = $("#vaccinationNextDueDate").val();
    var lotNumber      = $("#vaccinationLotNumber").val();
    var administeredBy = $("#vaccinationAdministeredBy").val();
    var clinic         = $("#vaccinationClinic").val();
    var cost           = $("#vaccinationCost").val();
    var notes          = $("#vaccinationNotes").val();
    var reminderEnabled = $("#vaccinationReminderEnabled").is(":checked");
    var tags           = $("#vaccinationTag").val();
    var vehicleId      = GetVehicleId().vehicleId;
    var recordId       = getVaccinationRecordModelData().id;
    var extraFields    = getAndValidateExtraFields();

    var hasError = false;
    if (extraFields.hasError) { hasError = true; }

    if (date.trim() == '') {
        hasError = true;
        $("#vaccinationDate").addClass("is-invalid");
    } else {
        $("#vaccinationDate").removeClass("is-invalid");
    }
    if (name.trim() == '') {
        hasError = true;
        $("#vaccinationName").addClass("is-invalid");
    } else {
        $("#vaccinationName").removeClass("is-invalid");
    }
    if (cost.trim() != '' && !isValidMoney(cost)) {
        hasError = true;
        $("#vaccinationCost").addClass("is-invalid");
    } else {
        $("#vaccinationCost").removeClass("is-invalid");
    }

    return {
        hasError: hasError,
        id: recordId,
        vehicleId: vehicleId,
        date: date,
        vaccineName: name,
        nextDueDate: nextDueDate,
        lotNumber: lotNumber,
        administeredBy: administeredBy,
        clinic: clinic,
        cost: cost == '' ? 0 : cost,
        notes: notes,
        reminderEnabled: reminderEnabled,
        tags: tags,
        extraFields: extraFields.extraFields
    };
}
