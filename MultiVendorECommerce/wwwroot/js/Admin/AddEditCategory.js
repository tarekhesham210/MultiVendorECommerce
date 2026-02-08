let attrIndex = parseInt($("#attributes-container").data("start-index") || 0);

// إضافة Attribute جديد
$("#add-attribute-btn").on("click", function () {
    const html = `
    <div class="attribute-item border p-2 mb-2" data-index="${attrIndex}">
    <input type="hidden" name="Attributes.Index" value="${attrIndex}" />
    
    <input type="hidden" name="Attributes[${attrIndex}].Id" value="0" />
    <input type="hidden" name="Attributes[${attrIndex}].IsDeleted" value="false" class="is-deleted-input" />
    
    <div class="form-group">
        <label>Name</label>
        <input name="Attributes[${attrIndex}].Name" class="form-control" />
    </div>
  
        <div class="form-group form-check">
            <input type="checkbox" name="Attributes[${attrIndex}].IsRequired" value="true" class="form-check-input" />
            <input type="hidden" name="Attributes[${attrIndex}].IsRequired" value="false" />
            <label class="form-check-label">Required</label>
        </div>
          <div class="form-group form-check">
            <input type="checkbox" name="Attributes[${attrIndex}].IsVariant" value="true" class="form-check-input" />
            <input type="hidden" name="Attributes[${attrIndex}].IsVariant" value="false" />
            <label class="form-check-label">Special for piece</label>
        </div>

        <div class="options-container mb-2" >
            <label>Options</label>
            <div class="options-list"></div>
            <button type="button" class="btn btn-secondary btn-sm add-option mt-1" ">+ Add Option</button>
        </div>

        <button type="button" class="btn btn-danger btn-sm remove-attribute" data-mode="add">Remove</button>
    </div>`;

    $("#attributes-container").append(html);
    attrIndex++;
});

// تغيير Type → إظهار Options إذا FixedSelect/CustomSelect
$(document).on("change", ".type-select", function () {
    const item = $(this).closest(".attribute-item");
    const container = item.find(".options-container");
    const addOptionBtn = item.find(".add-option");
    const type = $(this).val();
   // if (type === "FixedSelect") {
        container.show();
        addOptionBtn.show();
    //}
    //else if (type === "CustomSelect") {
    //    container.show();
    //    addOptionBtn.attr('style', 'display: none !important');
    //    item.find(".options-list").empty();
    //}
    //else {
    //    container.hide();
    //    addOptionBtn.hide();
    //}
});

// إضافة Option
$(document).on("click", ".add-option", function () {
    const optionsList = $(this).siblings(".options-list");
    const attrItem = $(this).closest(".attribute-item");
    const attrIndex = attrItem.data("index");
    const optionIndex = optionsList.children().length;

    const html = `
    <div class="input-group mb-1 option-item">
        <input type="text" name="Attributes[${attrIndex}].Options[${optionIndex}].Value" class="form-control" />
        <div class="input-group-append">
            <button type="button" class="btn btn-danger remove-option">X</button>
        </div>
    </div>`;

    optionsList.append(html);
});

// إزالة Option
$(document).on("click", ".remove-option", function () {
    $(this).closest(".option-item").remove();
});

// إزالة Attribute
$(document).on("click", ".remove-attribute", function () {
    const item = $(this).closest(".attribute-item");
    const mode = $(this).data("mode");

    if (mode === "add") {
        item.remove(); // الجديد اتمسح خالص مفيش مشكلة
    }
    else {
        item.hide();
        // تأكد من الوصول للـ input الصح
        item.find(".is-deleted-input").val("true");
    }
});

























//let lastAttrIndex = $("#attributes-container .attribute-item").length;

//// Add Attribute
//$("#add-attribute-btn").on("click", function () {
//    const index = lastAttrIndex;
//    const html = `
//    <div class="attribute-item border p-2 mb-2" data-attr-index="${index}">
//        <input type="hidden" name="Attributes[${index}].Id" value="0" />
//        <input type="hidden" name="Attributes[${index}].IsDeleted" value="false" />

//        <div class="form-group">
//            <label>Name</label>
//            <input name="Attributes[${index}].Name" class="form-control mb-1" />
//        </div>

//        <div class="form-group">
//            <label>Type</label>
//            <select name="Attributes[${index}].Type" class="form-control type-select">
//                <option value="Text">Text</option>
//                <option value="Number">Number</option>
//                <option value="Boolean">Boolean</option>
//                <option value="FixedSelect">FixedSelect</option>
//                <option value="CustomSelect">CustomSelect</option>
//            </select>
//        </div>

//        <div class="form-check mb-2">
//            <input type="checkbox" name="Attributes[${index}].IsRequired" value="true" class="form-check-input" />
//            <label class="form-check-label">Required</label>
//        </div>

//        <div class="options-container mb-2" style="display:none;">
//            <div class="options-list"></div>
//            <button type="button" class="btn btn-secondary btn-sm add-option mt-1">+ Add Option</button>
//        </div>

//        <button type="button" class="btn btn-danger btn-sm remove-attribute" data-mode="add">Remove Attribute</button>
//    </div>`;
//    $("#attributes-container").append(html);
//    lastAttrIndex++;
//});


//// Show / Hide Options
//$(document).on("change", ".type-select", function () {
//    const container = $(this).closest(".attribute-item").find(".options-container");
//    const type = $(this).val();
//    if (type === "FixedSelect" || type === "CustomSelect") container.show();
//    else container.hide();
//});

//// Add Option
//$(document).on("click", ".add-option", function () {
//    const optionsList = $(this).siblings(".options-list");
//    const attrItem = $(this).closest(".attribute-item");
//    const attrIndex = attrItem.data("attr-index");
//    const optionIndex = optionsList.children().length;

//    const html = `
//    <div class="input-group mb-1 option-item">
//        <input type="text" name="Attributes[${attrIndex}].Options[${optionIndex}].Value" class="form-control" />
//        <div class="input-group-append">
//            <button type="button" class="btn btn-danger remove-option">X</button>
//        </div>
//    </div>`;
//    optionsList.append(html);
//});

//// Remove Option
//$(document).on("click", ".remove-option", function () {
//    $(this).closest(".option-item").remove();
//});

//// Remove Attribute
//$(document).on("click", ".remove-attribute", function () {
//    const item = $(this).closest(".attribute-item");
//    const mode = $(this).data("mode");

//    if (mode === "add") {
//        item.remove(); // فقط DOM
//    } else {
//        item.hide();
//        item.find("input[name$='.IsDeleted']").val("true"); // soft delete
//    }
//});

//// Initialize options container on load
//$(".type-select").each(function () {
//    const type = $(this).val();
//    const container = $(this).closest(".attribute-item").find(".options-container");
//    if (type === "FixedSelect" || type === "CustomSelect") container.show();
//});























//let attrIndex = 0;

//document.addEventListener("DOMContentLoaded", function () {
//    const container = document.getElementById("attributes-container");
//    if (container) {
//        attrIndex = parseInt(container.dataset.startIndex || "0");
//    }
//});

//    // =========================
//    // 2️⃣ Add Attribute
//    // =========================
//    $("#add-attribute-btn").on("click", function () {

//        const html = `
//    <div class="attribute-item mb-3 border p-2">

//        <input type="hidden" name="Attributes[${attrIndex}].Id" value="0" />
//        <input type="hidden" name="Attributes[${attrIndex}].IsDeleted" value="false" />

//        <div class="form-group">
//            <label>Name</label>
//            <input name="Attributes[${attrIndex}].Name" class="form-control" />
//        </div>

//        <div class="form-group">
//            <label>Type</label>
//            <select name="Attributes[${attrIndex}].Type"
//                class="form-control type-select">
//                <option value="Text">Text</option>
//                <option value="Number">Number</option>
//                <option value="Boolean">Boolean</option>
//                <option value="FixedSelect">FixedSelect</option>
//                <option value="CustomSelect">CustomSelect</option>
//            </select>
//        </div>

//        <div class="form-group form-check">
//            <input type="checkbox"
//                name="Attributes[${attrIndex}].IsRequired"
//                value="true"
//                class="form-check-input" />
//            <label class="form-check-label">Required</label>
//        </div>

//        <div class="options-container mb-2" style="display:none;">
//            <label>Options</label>
//            <div class="options-list"></div>
//            <button type="button"
//                class="btn btn-secondary btn-sm add-option mt-1">
//                + Add Option
//            </button>
//        </div>

//        <button type="button"
//            class="btn btn-danger btn-sm remove-attribute"
//            data-mode="add">
//            Remove
//        </button>
//    </div>`;

//    $("#attributes-container").append(html);
//    attrIndex++;
//    });

//    // =========================
//    // 3️⃣ Show / Hide Options
//    // =========================
//    $(document).on("change", ".type-select", function () {
//        const container = $(this).closest(".attribute-item")
//    .find(".options-container");

//    const type = $(this).val();
//    if (type === "FixedSelect" || type === "CustomSelect") {
//        container.show();
//        } else {
//        container.hide();
//        }
//    });

//    // =========================
//    // 4️⃣ Add Option
//    // =========================
//    $(document).on("click", ".add-option", function () {
//        const optionsList = $(this).siblings(".options-list");
//    const attrItem = $(this).closest(".attribute-item");
//    const attrIndex = attrItem.index();
//    const optionIndex = optionsList.children().length;

//    const html = `
//    <div class="input-group mb-1 option-item">
//        <input type="text"
//            name="Attributes[${attrIndex}].Options[${optionIndex}].Value"
//            class="form-control" />
//        <div class="input-group-append">
//            <button type="button"
//                class="btn btn-danger remove-option">
//                X
//            </button>
//        </div>
//    </div>`;

//    optionsList.append(html);
//    });

//    // =========================
//    // 5️⃣ Remove Option
//    // =========================
//    $(document).on("click", ".remove-option", function () {
//        $(this).closest(".option-item").remove();
//    });

//    // =========================
//    // 6️⃣ Remove Attribute
//    // =========================
//    $(document).on("click", ".remove-attribute", function () {
//        const item = $(this).closest(".attribute-item");
//    const mode = $(this).data("mode");

//    if (mode === "add") {
//        // Create → remove DOM
//        item.remove();
//        } else {
//        // Edit → soft delete
//        item.hide();
//    item.find("input[name$='.IsDeleted']").val("true");
//        }
//    });

//    // =========================
//    // 7️⃣ Init on Page Load
//    // =========================
//    $(".type-select").each(function () {
//        const type = $(this).val();
//    const container = $(this).closest(".attribute-item")
//    .find(".options-container");

//    if (type === "FixedSelect" || type === "CustomSelect") {
//        container.show();
//        }
//    });

