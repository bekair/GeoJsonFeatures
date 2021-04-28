// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function removeAllValidations() {
    //Removes validation from input-fields
    $('.input-validation-error').addClass('input-validation-valid');
    $('.input-validation-error').removeClass('input-validation-error');
    $('.editorforError').removeClass('editorforError');
    //Removes validation message after input-fields
    $('.field-validation-error').addClass('field-validation-valid');
    $('.field-validation-error').removeClass('field-validation-error');
    //Removes validation summary 
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-errors').removeClass('validation-summary-errors');
}

function addFeaturesIntoTableRows(features, geoJsonDataTable) {
    geoJsonDataTable.clear();
    
    features.forEach(function (feature) {
        var featureGeometry = feature.geometry;
        var featureProperties = feature.properties;
        var coordinatesArray = featureGeometry.coordinates[0][0] && feature.geometry.coordinates[0][0][0]
            ? featureGeometry.coordinates[0][0].slice(0, 50)
            : [];

        geoJsonDataTable.row.add([
            feature.id ? feature.id : '-',
            featureProperties.name ? featureProperties.name : '-',
            featureProperties.changeset ? featureProperties.changeset : '-',
            featureGeometry.type ? featureGeometry.type : '-',
            coordinatesArray.length !== 0 ? coordinatesArray[0] : '-',
            featureProperties.natural ? featureProperties.natural : '-',
            featureProperties.water ? featureProperties.water : '-',
            featureProperties.user ? featureProperties.user : '-',
            featureProperties.timestamp ? featureProperties.timestamp : '-'
        ]).draw(false);
    });
}

$(document).ajaxStart(function () {
    $(".ajax-loader").show();
});

$(document).ajaxStop(function () {
    $(".ajax-loader").hide();
});
