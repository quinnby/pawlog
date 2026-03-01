function showAddMedicationRecordModal() {
    $.get('/Vehicle/GetAddMedicationRecordPartialView', function (data) {
        if (data) {
            $("#medicationRecordModalContent").html(data);
            initDatePicker($('#medicationStartDate'));
            initDatePicker($('#medicationEndDate'));
            initDatePicker($('#medicationRefillDate'));
            initTagSelector($("#medicationTag"));
            $('#medicationRecordModal').modal('show');
        }
    });
}

function showEditMedicationRecordModal(medicationRecordId, nocache) {
    $.get(`/Vehicle/GetMedicationRecordForEditById?medicationRecordId=${medicationRecordId}`, function (data) {
        if (data) {
            $("#medicationRecordModalContent").html(data);
            initDatePicker($('#medicationStartDate'));
            initDatePicker($('#medicationEndDate'));
            initDatePicker($('#medicationRefillDate'));
            initTagSelector($("#medicationTag"));
            $('#medicationRecordModal').modal('show');
            bindModalInputChanges('medicationRecordModal');
            $('#medicationRecordModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                if (getGlobalConfig().useMarkDown) {
                    toggleMarkDownOverlay("medicationNotes");
                }
            });
        }
    });
}

function hideAddMedicationRecordModal() {
    $('#medicationRecordModal').modal('hide');
}

function deleteMedicationRecord(medicationRecordId) {
    $("#workAroundInput").show();
    confirmDelete("Deleted Medication Records cannot be restored.", (result) => {
        if (result.isConfirmed) {
            $.post(`/Vehicle/DeleteMedicationRecordById?medicationRecordId=${medicationRecordId}`, function (data) {
                if (data.success) {
                    hideAddMedicationRecordModal();
                    successToast("Medication Record Deleted");
                    var vehicleId = GetVehicleId().vehicleId;
                    getVehicleMedicationRecords(vehicleId);
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

function saveMedicationRecordToVehicle(isEdit) {
    var formValues = getAndValidateMedicationRecordValues();
    if (formValues.hasError) {
        errorToast("Please check the form data");
        return;
    }
    $.post('/Vehicle/SaveMedicationRecordToVehicleId', formValues, function (data) {
        if (data.success) {
            successToast(isEdit ? "Medication Record Updated" : "Medication Record Added");
            hideAddMedicationRecordModal();
            saveScrollPosition();
            getVehicleMedicationRecords(formValues.vehicleId);
        } else {
            errorToast(data.message);
        }
    });
}

function getAndValidateMedicationRecordValues() {
    var date            = $("#medicationStartDate").val();
    var name            = $("#medicationName").val();
    var dosage          = $("#medicationDosage").val();
    var unit            = $("#medicationUnit").val();
    var frequency       = $("#medicationFrequency").val();
    var route           = $("#medicationRoute").val();
    var endDate         = $("#medicationEndDate").val();
    var prescribingVet  = $("#medicationPrescribingVet").val();
    var purpose         = $("#medicationPurpose").val();
    var refillDate      = $("#medicationRefillDate").val();
    var cost            = $("#medicationCost").val();
    var notes           = $("#medicationNotes").val();
    var isActive        = $("#medicationIsActive").is(":checked");
    var reminderEnabled = $("#medicationReminderEnabled").is(":checked");
    var tags            = $("#medicationTag").val();
    var vehicleId       = GetVehicleId().vehicleId;
    var recordId        = getMedicationRecordModelData().id;
    var extraFields     = getAndValidateExtraFields();

    var hasError = false;
    if (extraFields.hasError) { hasError = true; }

    if (date.trim() == '') {
        hasError = true;
        $("#medicationStartDate").addClass("is-invalid");
    } else {
        $("#medicationStartDate").removeClass("is-invalid");
    }
    if (name.trim() == '') {
        hasError = true;
        $("#medicationName").addClass("is-invalid");
    } else {
        $("#medicationName").removeClass("is-invalid");
    }
    if (cost.trim() != '' && !isValidMoney(cost)) {
        hasError = true;
        $("#medicationCost").addClass("is-invalid");
    } else {
        $("#medicationCost").removeClass("is-invalid");
    }

    return {
        hasError: hasError,
        id: recordId,
        vehicleId: vehicleId,
        date: date,
        medicationName: name,
        dosage: dosage,
        unit: unit,
        frequency: frequency,
        route: route,
        endDate: endDate,
        prescribingVet: prescribingVet,
        purpose: purpose,
        refillDate: refillDate,
        cost: cost == '' ? 0 : cost,
        notes: notes,
        isActive: isActive,
        reminderEnabled: reminderEnabled,
        tags: tags,
        extraFields: extraFields.extraFields
    };
}
