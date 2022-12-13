// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#r11').on('click', function () {
    $(this).parent().find('a').trigger('click')
})

$('#r12').on('click', function () {
    $(this).parent().find('a').trigger('click')
})

function updatingPrices() {

    let elements = document.getElementsByName("Cantidades");
    let precio = document.getElementsByName("Precios");
    let descuentos = document.getElementsByName("PrecioDescuento");
    let total = 0;
    for (let i = 0; i < elements.length; i++) {
        if (parseInt(elements[i].value) > -1) { 
            total = total + parseInt(elements[i].value) * parseFloat(precio[i].value);
        }

    }
    document.getElementById("CantidadTotal").innerHTML = formatter.format(total);

    let idsDescuento = document.getElementsByName("IdSandwDescuento");
    for (let i = 0; i < descuentos.length; i++) {
        let b = "Sandwich_Quantity_" + String(idsDescuento[i].value);
        let cantidad = document.getElementById(b).value
        total = total - parseFloat(parseFloat(descuentos[i].value) * parseInt(cantidad));
    }

    document.getElementById("CantidadTotalconOferta").innerHTML = formatter.format(total);

}




const formatter = new Intl.NumberFormat('es-ES', {
    style: 'currency',
    currency: 'EUR',

    // These options are needed to round to whole numbers if that's what you want.
    minimumFractionDigits: 2, // (this suffices for whole numbers, but will print 2500.10 as $2,500.1)
    maximumFractionDigits: 2, // (causes 2500.99 to be printed as $2,501)
});